using Storage.Data.Entity;

namespace Storage.Bl
{
    public static class BoxExtetions
    {
        /// <summary>
        /// Срок годности коробки
        /// </summary>
        /// <param name="box">Коробка</param>
        public static DateTime? GetExpirationDate(this Box box)
        {
            if (!box.ExpirationDate.HasValue)
                box.ExpirationDate = box.ProductionDate?.AddDays(100);
            return box.ExpirationDate;
        }

        /// <summary>
        /// Объем занимаемый коробкой
        /// </summary>
        /// <param name="box">Коробка</param>
        public static double GetVolume(this Box box)
            => box.Width * box.Height * box.Deep;
    }
}
