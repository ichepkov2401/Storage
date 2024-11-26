namespace Storage.Data.Entity;
/// <summary>
/// Сущность коробки на складе.
/// </summary>
public class Box : StorageUnitEntity
{

    private const int defaultExpirationDay = 100;

    private DateTime? expirationDate;

    public int PalletId { get; set; }

    public virtual Pallet Pallet { get; set; }

    public DateTime? ProductionDate { get; set; }

    public DateTime ExpirationDate
    {
        get
        {
            if (!expirationDate.HasValue)
            {
                expirationDate = ProductionDate?.AddDays(defaultExpirationDay);
            }
            return expirationDate.Value;
        }
        set
        {
            expirationDate = value;
        }
    }

    public double Volume => Width * Height * Depth;

    public double Weight { get; set; }
}
