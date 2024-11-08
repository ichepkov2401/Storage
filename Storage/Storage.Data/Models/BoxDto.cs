using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Data.Models
{
    public class BoxDto
    {
        public int PalletId { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? ProductionDate { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Deep { get; set; }

        public double Weight { get; set; }
    }
}
