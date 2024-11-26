namespace Storage.Data.Models.Output
{
    public class BoxResponse
    {
        public int Id { get; set; }

        public int PalletId { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? ProductionDate { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Depth { get; set; }

        public double Weight { get; set; }

        public double Volume { get; set; }
    }
}
