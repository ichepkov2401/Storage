using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Storage.Data.Entity
{
    /// <summary>
    /// Сущность паллета на складе.
    /// </summary>
    public class Pallet : StorageUnitEntity
    {
        const double BASE_PALLET_WEIGHT = 30;

        public override DateTime? ExpirationDate => RealBoxes.Min(x => x.ExpirationDate);

        public override double Volume => RealBoxes.Sum(x => x.Volume) + Width * Height * Deep;

        public double Weight => RealBoxes.Sum(x => x.Weight) + 30;

        public virtual ICollection<Box> Boxes { get; } = new List<Box>();

        [NotMapped]
        public IEnumerable<Box> RealBoxes => Boxes.Where(x => !x.DeletedDate.HasValue);

        public override string ToString()
        {
            return $"Паллет номер - {Id}, Масса - {Weight}, Объем - {Volume}{RealBoxes.Aggregate("", (x, y) => x + $"\n    {y}")}";
        }
    }
}
