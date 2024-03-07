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

        public BinarioViewModel()
        {
            var ips = Dns.GetHostAddresses(Dns.GetHostName());

            IP = ips.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).
                Select(x => x.ToString()).FirstOrDefault() ?? "0.0.0.0";

            GenerarBinario();
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
