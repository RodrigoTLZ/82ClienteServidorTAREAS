using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BinarioUDPCliente.Models.DTOs;

namespace BinarioUDPCliente.Services
{
    public class BinarioServer
    {
        private UdpClient cliente = new();
        public string Servidor { get; set; } = "0.0.0.0";
   
        public void EnviarRespuesta(BinarioDTO dto)
        {
            var ipe = new IPEndPoint(IPAddress.Parse(Servidor), 5000);
            var json = JsonSerializer.Serialize(dto);
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            cliente.Send(buffer, buffer.Length, ipe);
        }
    }
}
