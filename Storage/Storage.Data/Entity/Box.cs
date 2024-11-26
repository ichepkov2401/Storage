using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Data.Entity
{
    /// <summary>
    /// Сущность коробки на складе.
    /// </summary>
    public class Box : StorageUnitEntity
    {
        public int PalletId { get; set; }
        
        public virtual Pallet Pallet { get; set; }

        public DateTime? ProductionDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public double Weight { get; set; }
    }
}
