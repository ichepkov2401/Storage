namespace Storage.Data.Entity
{
    /// <summary>
    /// Сущность единицы хранения.
    /// </summary>
    public abstract class StorageUnitEntity : BaseEntity<int>
    {
        public double Width { get; set; }

        public double Height { get; set; }

        public double Deep { get; set; }


        // Представленные ниже свойства имеют смысл, если в системе потенциально (например в будущем) возможно обращение к производным классам по имени абстрактного
        // Иначе от них можно отказаться в пользу упращения кода.

        public abstract DateTime? ExpirationDate { get; }

        public abstract double Volume { get; }
    }
}
