using Storage.Data.Models.Box;

namespace Storage.Data.Models.Pallet;
public class PalletPrintModel
{
    public int Id { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public double Deep { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public double Volume { get; set; }

    public double Weight { get; set; }

    public BoxPrintModel[] Boxes { get; set; }
}
