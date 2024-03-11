using BinarioUDPServidor.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class BinarioServer : INotifyPropertyChanged
    {
        public UdpClient server;
        public event EventHandler<BinarioDTO>? RespuestaRecibida;
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool seguirEscuchando = true;

        public BinarioServer()
        {
            var hilo = new Thread(new ThreadStart(EscucharRespuestas))
            {
                IsBackground = true
            };
            hilo.Start();
        }

        private void EscucharRespuestas()
        {

            server = new UdpClient(10000);
            DispatcherTimer timerRespuestas = new DispatcherTimer();
            timerRespuestas.Interval = TimeSpan.FromSeconds(5);
            timerRespuestas.Tick += (sender, e) =>
            {
                seguirEscuchando = false;
                timerRespuestas.Stop();
                server.Close();
            };
            timerRespuestas.Start();

            try
            {
                while (seguirEscuchando)
                {
                    IPEndPoint remoto = new(IPAddress.Any, 10000);
                    byte[] buffer = server.Receive(ref remoto);

                    BinarioDTO? dto = JsonSerializer.Deserialize<BinarioDTO>(Encoding.UTF8.GetString(buffer));

                    if (dto != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            RespuestaRecibida?.Invoke(this, dto);
                        });
                    }
                }

            }
            catch (Exception)
            {

            }
        }

    }
}
