namespace Storage.Data.Entity
{
    /// <summary>
    /// Сущность единицы хранения.
    /// </summary>
    public abstract class StorageUnitEntity : BaseEntity<int>
    {
        public double Width { get; set; }

        public double Height { get; set; }

        public double Depth { get; set; }
    }
}
