using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.Data.Models.Box;

namespace Storage.Data.Models.Pallet
{
    public class PalletStoreModel
    {
        public int Id { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Depth { get; set; }

        public IReadOnlyList<BoxStoreModel> Boxes { get; } = new List<BoxStoreModel>();
    }
}
