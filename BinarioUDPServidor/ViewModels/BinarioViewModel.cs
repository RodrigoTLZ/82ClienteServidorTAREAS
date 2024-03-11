using BinarioUDPServidor.Models;
using BinarioUDPServidor.Models.DTOs;
using BinarioUDPServidor.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Windows.Input;
using System.Net.Sockets;
using System.Text.Json;
using System.Windows;


namespace BinarioUDPServidor.ViewModels
{
    public class BinarioViewModel:INotifyPropertyChanged
    {
        public string IP { get; set; } = "0.0.0.0";

        public string BinarioGenerado { get; set; }
        public int NumeroEnteroDecimal { get; set; }
        public bool MostrarBinario { get; set; } = true;
        public bool AceptarRespuestas { get; set; } = true;
        public string MensajeServidor { get; set; }
        BinarioServer servidor = new();
        private UdpClient udpClient;

        public ObservableCollection<Usuario> UsuariosGanadores { get; set; } = new();
        private ObservableCollection<BinarioDTO> RespuestasCorrectas { get; set; } = new();


        public ICommand GenerarBinarioCommand { get; set; }
        public ICommand ReiniciarCommand {get; set; }

        public BinarioViewModel()
        {
            var ips = Dns.GetHostAddresses(Dns.GetHostName());
            udpClient = new UdpClient();

            IP = ips.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).
                Select(x => x.ToString()).FirstOrDefault() ?? "0.0.0.0";

            GenerarBinarioCommand = new RelayCommand(GenerarBinario);
            ReiniciarCommand = new RelayCommand(Reiniciar);

            servidor.RespuestaRecibida += Servidor_RespuestaRecibida;
            UsuariosGanadores.Clear();
            RespuestasCorrectas.Clear();
            ActualizarDatos();
        }

        private void Servidor_RespuestaRecibida(object? sender, Models.DTOs.BinarioDTO e)
        {
            if (AceptarRespuestas)
            {
                if (e.RespuestaUsuario == NumeroEnteroDecimal)
                {
                    var proye = RespuestasCorrectas.Any(x => x.IPCliente == e.IPCliente);
                    if(proye == false)
                    {
                        RespuestasCorrectas.Add(e);
                    }
                }
                ActualizarDatos();
            }
        }

        private void GenerarBinario()
        {
            Random random = new Random();
            NumeroEnteroDecimal = random.Next(1, 256);
            BinarioGenerado = Convert.ToString(NumeroEnteroDecimal, 2);
            AceptarRespuestas = true;

            MostrarBinario = true;
            Timer timer = new Timer(5000);
            timer.AutoReset = false;
            Timer timerRespuestaRecibir = new Timer(30000);
            timerRespuestaRecibir.AutoReset = false;
            timerRespuestaRecibir.Elapsed += (sender, e) =>
            {
                AceptarRespuestas = false;
                timerRespuestaRecibir.Stop();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var item in RespuestasCorrectas)
                    {
                        UsuariosGanadores.Add(new Usuario { Nombre = item.NombreUsuario });
                        string mensaje = "¡Felicidades, conseguiste adivinar el número!.";

                        var ipe = new IPEndPoint(IPAddress.Parse(item.IPCliente), 10001);
                        var json = JsonSerializer.Serialize(new BinarioDTO { Mensaje = mensaje });
                        byte[] buffer = Encoding.UTF8.GetBytes(json);
                        udpClient.Send(buffer, buffer.Length, ipe);
                    }
                });
                ActualizarDatos();
            };

            timer.Elapsed += (sender, e) =>
            {
                MostrarBinario = false;
                timer.Stop();
                timerRespuestaRecibir.Start();
                ActualizarDatos();
            };
            timer.Start();
            ActualizarDatos();



        }

        private void Reiniciar()
        {
            foreach (var item in RespuestasCorrectas)
            {
                string mensaje = "";

                var ipe = new IPEndPoint(IPAddress.Parse(item.IPCliente), 10001);
                var json = JsonSerializer.Serialize(new BinarioDTO { Mensaje = mensaje });
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                udpClient.Send(buffer, buffer.Length, ipe);
            }
           
            UsuariosGanadores.Clear();
            RespuestasCorrectas.Clear();
            GenerarBinario();
             ActualizarDatos();
            
        }

        private void ActualizarDatos(string? propiedad = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
