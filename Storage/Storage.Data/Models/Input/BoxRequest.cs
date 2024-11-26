namespace Storage.Data.Models.Input
{
    public class BoxRequest
    {
        public int PalletId { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? ProductionDate { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Depth { get; set; }

        public double Weight { get; set; }
    }
}
