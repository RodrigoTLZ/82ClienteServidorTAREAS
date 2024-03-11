using BinarioUDPCliente.Models.DTOs;
using BinarioUDPCliente.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Input;

namespace BinarioUDPCliente.ViewModels
{
    public class BinarioViewModel : INotifyPropertyChanged
    {
        public BinarioDTO Datos { get; set; } = new();
        BinarioServer ClienteUDP = new();
        public string Mensaje { get; set; }
        public ICommand EnviarRespuestaCommand { get; set; }
        public string IP { get; set; } = "0.0.0.0";
        public BinarioViewModel()
        {
            EnviarRespuestaCommand = new RelayCommand(EnviarRespuesta);
            ClienteUDP.RespuestaServidorRecibida += Servidor_RespuestaServidorRecibida;
        }

        private void Servidor_RespuestaServidorRecibida(object? sender, BinarioDTO e)
        {
                Mensaje = e.Mensaje;
                ActualizarDatos(nameof(Mensaje));
        }

        private void EnviarRespuesta()
        {
            ActualizarDatos();
            ClienteUDP.Servidor = IP;
            ClienteUDP.EnviarRespuesta(Datos);
            
        }


        private void ActualizarDatos(string? propiedad = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
        }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
