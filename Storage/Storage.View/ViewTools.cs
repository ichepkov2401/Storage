using Storage.Data.Models.Box;
using Storage.Data.Models.Pallet;

namespace Storage.View;
internal static class ViewTools
{
    internal static void Output(IReadOnlyCollection<(DateTime? Key, IReadOnlyList<PalletPrintModel> Values)> sortedPallet, IReadOnlyCollection<PalletPrintModel> longestLifeTimePallet)
    {
        foreach (var group in sortedPallet)
        {
            if (group.Key > DateTime.Now) Console.ForegroundColor = ConsoleColor.Green;
            else Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Срок годности - {group.Key?.ToString("dd.MM.yyyy")}");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var pallet in group.Values)
            {
                Console.WriteLine(ToString(pallet));
            }
        }

        Console.WriteLine();

        foreach (var pallet in longestLifeTimePallet)
        {
            Console.WriteLine(ToString(pallet));
        }
    }

    public static string ToString(PalletPrintModel pallet)
        => $"Паллет номер - {pallet.Id}, Масса - {Math.Round(pallet.Weight, 2)}, Объем - {Math.Round(pallet.Volume, 2)}{pallet.Boxes.Aggregate("", (x, y) => x + $"\n    {ToString(y)}")}";

    public static string ToString(BoxPrintModel box)
        => $"Коробка #{box.Id}, Масса - {Math.Round(box.Weight, 2)}, Объем - {Math.Round(box.Volume, 2)}, Срок годности - {box.ExpirationDate?.ToString("dd.MM.yyyy")}";
}
