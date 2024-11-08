using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Data.Entity
{
    /// <summary>
    /// Сущность коробки на складе.
    /// </summary>
    public class Box : StorageUnitEntity
    {
        private DateTime? expirationDate;

        [ForeignKey(nameof(PalletId))]
        public int PalletId { get; set; }

        public virtual Pallet Pallet { get; set; }

        public DateTime? ProductionDate { get; set; }

        // Ленивое вычисление.

        public override DateTime? ExpirationDate
        {
            get
            {
                if (!expirationDate.HasValue)
                    expirationDate = ProductionDate?.AddDays(100);
                return expirationDate.Value;
            }
        }

        public DateTime ExpirationDateSet
        {
            set => expirationDate = value;
        }

        public override double Volume => Width * Height * Deep;

        public double Weight { get; set; }

        public override string ToString()
        {
            return $"Коробка #{Id}, Масса - {Weight}, Объем - {Volume}, Срок годности - {ExpirationDate.Value.ToString("dd.MM.yyyy")}";
        }
    }
}
