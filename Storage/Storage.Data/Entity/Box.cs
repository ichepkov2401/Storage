using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Data.Entity
{
    /// <summary>
    /// Сущность коробки на складе
    /// </summary>
    public class Box : StorageUnitEntity
    {
        private DateTime? expirationDate;

        [ForeignKey(nameof(PalletId))]
        public int PalletId { get; set; }
        public Pallet Pallet { get; set; }

        public DateTime? ProductionDate { get; set; }

        // Ленивое вычисление

        public override DateTime ExpirationDate 
        { 
            get
            {
                if (!expirationDate.HasValue) 
                    expirationDate = ProductionDate?.AddDays(100);
                return expirationDate.Value;
            }
        }

        public override double Volume { get => Width * Height * Deep; }

        public override double Weight { get; }
    }
}
