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
using System.Windows.Threading;

namespace BinarioUDPServidor.Services
{
    public class BinarioServer
    {

        private UdpClient server;
        private DispatcherTimer timer;
        public event EventHandler<BinarioDTO> RespuestaRecibida;
        private bool seguirEscuchando;

        public BinarioServer()
        {
            Iniciar();
        }

        private void Iniciar()
        {
            server = new UdpClient(10000);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(30);
            timer.Tick += Timer_Tick;
            timer.Start();

            Thread thread = new Thread(EscucharRespuestas);
            thread.IsBackground = true;
            thread.Start();
        }

        private void EscucharRespuestas(object? obj)
        {
            try
            {
                while (seguirEscuchando)
                {
                    IPEndPoint remoto = new IPEndPoint(IPAddress.Any, 10000);
                    byte[] buffer = server.Receive(ref remoto);
                    BinarioDTO dto = JsonSerializer.Deserialize<BinarioDTO>(Encoding.UTF8.GetString(buffer));
                    EnviarRespuestaRecibida(dto);
                }
            }
            catch (Exception)
            {
                
            }
            finally
            {
                server.Close();
            }
        }

        private void EnviarRespuestaRecibida(BinarioDTO? dto)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                RespuestaRecibida?.Invoke(this, dto);
            });
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            timer.Stop();
            seguirEscuchando = false;
        }
    }
}
