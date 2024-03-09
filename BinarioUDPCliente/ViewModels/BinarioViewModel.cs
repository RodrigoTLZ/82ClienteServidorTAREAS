using BinarioUDPCliente.Models.DTOs;
using BinarioUDPCliente.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BinarioUDPCliente.ViewModels
{
    public class BinarioViewModel : INotifyPropertyChanged
    {
        public BinarioDTO Datos { get; set; } = new();
        BinarioServer ClienteUDP = new();
        public ICommand EnviarRespuestaCommand { get; set; }
        public string IP { get; set; } = "0.0.0.0";
        public BinarioViewModel()
        {
            EnviarRespuestaCommand = new RelayCommand(EnviarRespuesta);
        }

        private void EnviarRespuesta()
        {
            ClienteUDP.Servidor = IP;
            ClienteUDP.EnviarRespuesta(Datos);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
