using Storage.Data.Entity;

namespace Storage.Bl
{
    public static class PalletExtetions
    {
        /// <summary>
        /// Срок годности паллета
        /// </summary>
        public static DateTime? GetExpirationDate(this Pallet pallet)
            => pallet.RealBoxes.Min(x => x.GetExpirationDate());

        /// <summary>
        /// Объем занимаемый паллетой
        /// </summary>
        public static double GetVolume(this Pallet pallet)
            => pallet.RealBoxes.Sum(x => x.GetVolume()) + pallet.Width * pallet.Height * pallet.Deep;

        /// <summary>
        /// Масса паллеты (вместе с коробками на ней)
        /// </summary>
        public static double GetWeight(this Pallet pallet) => pallet.RealBoxes.Sum(x => x.Weight) + Constants.BASE_PALLET_WEIGHT;
    }
}
