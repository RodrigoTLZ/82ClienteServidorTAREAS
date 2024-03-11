using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using BinarioUDPCliente.Models.DTOs;
using System.Windows;

namespace BinarioUDPCliente.Services
{
    public class BinarioServer
    {
        private UdpClient cliente = new();
        public event EventHandler<BinarioDTO>? RespuestaServidorRecibida;
        public string Servidor { get; set; } = "0.0.0.0";

        public BinarioServer()
        {
            var hilo = new Thread(new ThreadStart(EscucharRespuestaServidor))
            {
                IsBackground = true
            };
            hilo.Start();
        }

        private void EscucharRespuestaServidor()
        {
            cliente = new UdpClient(10001);

            try
            {
                while (true)
                {
                    IPEndPoint remoto = new(IPAddress.Any, 10001);
                    byte[] buffer = cliente.Receive(ref remoto);
                    BinarioDTO? dto = JsonSerializer.Deserialize<BinarioDTO>(Encoding.UTF8.GetString(buffer));

                    if (dto != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            RespuestaServidorRecibida?.Invoke(this, dto);
                        });
                    }
                }
                
            }
            catch (Exception)
            {

            }
        }

        public void EnviarRespuesta(BinarioDTO dto)
        {

            var ips = Dns.GetHostAddresses(Dns.GetHostName());

            dto.IPCliente = ips.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).
                Select(x => x.ToString()).FirstOrDefault() ?? "0.0.0.0";
            var ipe = new IPEndPoint(IPAddress.Parse(Servidor), 10000);
            
            dto.NombreUsuario = Dns.GetHostName();
            var json = JsonSerializer.Serialize(dto);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            
     


            cliente.Send(buffer, buffer.Length, ipe);
        }
    }
}
