using BinarioUDPServidor.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace BinarioUDPServidor.Services
{
    public class BinarioServer
    {
        private UdpClient cliente;
        public event EventHandler<BinarioDTO> RespuestaRecibida;

        public BinarioServer()
        {
            var hilo = new Thread(new ThreadStart(Iniciar));
            hilo.IsBackground = true;
            hilo.Start();
        }

        private void Iniciar()
        {
            UdpClient server = new(5000);
            IPEndPoint remoto = new(IPAddress.Any, 5000);
            byte[] buffer = server.Receive(ref remoto);


            BinarioDTO? dto = JsonSerializer.Deserialize<BinarioDTO>(Encoding.UTF8.GetString(buffer));

            if(dto != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    RespuestaRecibida?.Invoke(this, dto);
                });
            }
        }
    }
}
