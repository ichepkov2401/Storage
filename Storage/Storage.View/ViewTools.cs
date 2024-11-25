using Storage.Data.Models.Input;
using Storage.Data.Models.Output;

namespace Storage.View
{
    internal static class ViewTools
    {
        internal static PalletInputDto InputPalletDto()
        {
            while (true)
            {
                try
                {
                    PalletInputDto dto = new PalletInputDto();
                    Console.Write($"Ширина паллета - ");
                    dto.Width = double.Parse(Console.ReadLine());
                    Console.Write($"Высота паллета  - ");
                    dto.Height = double.Parse(Console.ReadLine());
                    Console.Write($"Глубина паллета - ");
                    dto.Deep = double.Parse(Console.ReadLine());
                    return dto;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неожиданный формат данных. Начнем сначала");
                }
            }
        }

        internal static BoxInputDto InputBoxDto()
        {
            while (true)
            {
                try
                {
                    BoxInputDto dto = new BoxInputDto();
                    Console.Write($"Ширина коробки - ");
                    dto.Width = double.Parse(Console.ReadLine());
                    Console.Write($"Высота коробки - ");
                    dto.Height = double.Parse(Console.ReadLine());
                    Console.Write($"Глубина коробки - ");
                    dto.Deep = double.Parse(Console.ReadLine());
                    Console.Write($"Вес коорбки - ");
                    dto.Weight = double.Parse(Console.ReadLine());
                    Console.Write($"Номер, паллета на котором лежит коробка - ");
                    int palletNumber = int.Parse(Console.ReadLine());
                    dto.PalletId = palletNumber;
                    Console.Write($"Дата изготовления коробки (если не известно null) - ");
                    DateTime parseDataTime;
                    if (DateTime.TryParse(Console.ReadLine(), out parseDataTime))
                        dto.ProductionDate = parseDataTime;
                    Console.Write($"Срок годности коробки (если не известно null) - ");
                    if (DateTime.TryParse(Console.ReadLine(), out parseDataTime))
                        dto.ExpirationDate = parseDataTime;
                    else if (!dto.ProductionDate.HasValue)
                    {
                        // Если пользователь не ввел ни дату производства, ни срок годности, будем считать, что изготовили коробку сегодня (для простоты проверки)
                        dto.ProductionDate = DateTime.Now.Date;
                    }
                    return dto;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неожиданный формат данных. Начнем сначала");
                }
            }
        }

        internal static void Output(List<List<PalletOutputDto>> sortedPallet, List<PalletOutputDto> longestLifeTimePallet)
        {
            foreach (var group in sortedPallet)
            {
                if (group.First().ExpirationDate > DateTime.Now) Console.ForegroundColor = ConsoleColor.Green;
                else Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Срок годности - {group.First().ExpirationDate?.ToString("dd.MM.yyyy")}");
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var pallet in group)
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

        public static string ToString(PalletOutputDto pallet)
            => $"Паллет номер - {pallet.Id}, Масса - {Math.Round(pallet.Weight, 2)}, Объем - {Math.Round(pallet.Volume, 2)}{pallet.Boxes.Aggregate("", (x, y) => x + $"\n    {ToString(y)}")}";

        public static string ToString(BoxOutputDto box)
            => $"Коробка #{box.Id}, Масса - {Math.Round(box.Weight, 2)}, Объем - {Math.Round(box.Volume, 2)}, Срок годности - {box.ExpirationDate?.ToString("dd.MM.yyyy")}";
    }
}
