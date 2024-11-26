using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.Data.Models.Pallet;

namespace Storage.Data.Models.Box
{
    internal class BoxStoreModel
    {
        public int Id { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Depth { get; set; }

        public double Weight { get; set; }

        public int PalletId { get; set; }

        public DateTime? ProductionDate { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
