namespace Storage.Data.Entity
{
    /// <summary>
    /// Сущность паллета на складе.
    /// </summary>
    public class Pallet : StorageUnitEntity
    {
        const double BASE_PALLET_WEIGHT = 30;

        public override DateTime ExpirationDate => Boxes.Min(x => x.ExpirationDate);

        public override double Volume => Boxes.Sum(x => x.Volume) + Width * Height * Deep;

        public override double Weight => Boxes.Sum(x => x.Weight) + 30;

        public ICollection<Box> Boxes { get; set; } = new List<Box>();
    }
}
