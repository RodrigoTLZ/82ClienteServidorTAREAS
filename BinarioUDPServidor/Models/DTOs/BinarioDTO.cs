﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarioUDPServidor.Models.DTOs
{
    public class BinarioDTO
    {
        public Usuario NombreUsuario { get; set; } = null!;
        public string Respuesta { get; set; } = null!;
    }
}
