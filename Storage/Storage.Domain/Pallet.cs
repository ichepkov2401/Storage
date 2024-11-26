namespace Storage.Data.Entity;
public class Pallet : StorageUnitEntity
{

    private const double basePalletWeight = 30;

    public IReadOnlyList<Box> Boxes { get; } = new List<Box>();

    public double Weight => Boxes.Sum(x => x.Weight) + basePalletWeight;

    public double Volume => Boxes.Sum(x => x.Volume) + Width * Height * Depth;

    public DateTime ExpirationDate => Boxes.Min(x => x.ExpirationDate);
}
