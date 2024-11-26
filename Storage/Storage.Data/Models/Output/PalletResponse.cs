namespace Storage.Data.Models.Output
{
    public class PalletResponse
    {
        public int Id { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Deep { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public double Volume { get; set; }

        public double Weight { get; set; }

        public BoxResponse[] Boxes { get; set; }
    }
}
