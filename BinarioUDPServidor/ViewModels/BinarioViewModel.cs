using BinarioUDPServidor.Models;
using BinarioUDPServidor.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BinarioUDPServidor.ViewModels
{
    public class BinarioViewModel:INotifyPropertyChanged
    {
        public string IP { get; set; } = "0.0.0.0";
        public string BinarioGenerado { get; set; }
        BinarioServer servidor = new();
        public List<Usuario> UsuariosGanadores { get; set; }
        public List<Usuario> UsuariosPerdedores { get; set; }

        public BinarioViewModel()
        {
            var ips = Dns.GetHostAddresses(Dns.GetHostName());

            IP = ips.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).
                Select(x => x.ToString()).FirstOrDefault() ?? "0.0.0.0";
            GenerarBinario();
            servidor.RespuestaRecibida += Servidor_RespuestaRecibida;
        }

        private void Servidor_RespuestaRecibida(object? sender, Models.DTOs.BinarioDTO e)
        {
            
            if(e.Respuesta == BinarioGenerado)
            {
                UsuariosGanadores.Add(e.NombreUsuario);
            }
            else
            {
                UsuariosPerdedores.Add(e.NombreUsuario);
            }
        }

        private void GenerarBinario()
        {
            Random random = new Random();
            int numeroEntero = random.Next(1, 256);
            BinarioGenerado = Convert.ToString(numeroEntero, 2);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
