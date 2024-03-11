﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarioUDPCliente.Models.DTOs
{
    public class BinarioDTO
    {
        public string NombreUsuario { get; set; } = null!;
        public string IPCliente { get; set; } = null!;
        public int RespuestaUsuario { get; set; }
        public string Mensaje { get; set; } = null!;
    }
}
