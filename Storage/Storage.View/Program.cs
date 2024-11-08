using Storage.Bl.Service.Interfaces;
using Storage.Data.Entity;
using Storage.Data.Models;
using System.Data.Entity;

namespace Storage.View
{
    internal class Program
    {

        static IPalletService palletService = AutofacIntegration.GetInstance<IPalletService>();
        static IBoxService boxService = AutofacIntegration.GetInstance<IBoxService>();
        static IGeneratorService generatorService = AutofacIntegration.GetInstance<IGeneratorService>();

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Console.WriteLine("Выбирите режим:");
                Console.WriteLine("1 - Ручное тестирование");
                Console.WriteLine("2 - Генеративное тестирование");
                Console.WriteLine("3 - Продакшн");
                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Manual();
                            break;
                        }
                    case "2":
                        {
                            Generated();
                            break;
                        }
                    case "3":
                        {
                            Prod();
                            break;
                        }
                    default:
                        return;
                }
            }
        }

        /// <summary>
        /// Ручное тестирование, данные вводятся пользователем и анализируются в памяти
        /// </summary>
        static async void Manual()
        {
            Console.Clear();
            Console.WriteLine("Сколько на складе паллетов?");
            Pallet[] pallets = new Pallet[int.Parse(Console.ReadLine())];
            for (int i = 0; i < pallets.Length; i++)
            {
                try
                {
                    pallets[i] = new Pallet();
                    pallets[i].Id = i + 1;
                    Console.Write($"Ширина паллета {i + 1}  - ");
                    pallets[i].Width = double.Parse(Console.ReadLine());
                    Console.Write($"Высота паллета {i + 1}  - ");
                    pallets[i].Height = double.Parse(Console.ReadLine());
                    Console.Write($"Глубина паллета {i + 1} - ");
                    pallets[i].Deep = double.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неожиданный формат данных. Начнем сначала");
                    i--;
                }
            }
            Console.WriteLine("Сколько на складе коробок?");
            Box[] boxes = new Box[int.Parse(Console.ReadLine())];
            for (int i = 0; i < boxes.Length; i++)
            {
                try
                {
                    boxes[i] = new Box();
                    boxes[i].Id = i + 1;
                    while (true)
                    {
                        Console.Write($"Ширина коробки {i + 1}  - ");
                        boxes[i].Width = double.Parse(Console.ReadLine());
                        Console.Write($"Высота коробки {i + 1}  - ");
                        boxes[i].Height = double.Parse(Console.ReadLine());
                        Console.Write($"Глубина коробки {i + 1} - ");
                        boxes[i].Deep = double.Parse(Console.ReadLine());
                        Console.Write($"Вес коорбки {i + 1}     - ");
                        boxes[i].Weight = double.Parse(Console.ReadLine());

                        Console.Write($"Номер, паллета на котором лежит коробка {i + 1} - ");
                        int palletNumber = int.Parse(Console.ReadLine());
                        boxes[i].Pallet = pallets.FirstOrDefault(x => x.Id == palletNumber);
                        boxes[i].PalletId = boxes[i].Pallet.Id;
                        boxes[i].Pallet.Boxes.Add(boxes[i]);
                        if (boxes[i].Deep > boxes[i].Pallet.Deep || boxes[i].Width > boxes[i].Pallet.Width)
                        {
                            Console.WriteLine("Коробка не должна превышать по размерам паллету (по ширине и глубине)");
                            continue;
                        }
                        break;
                    }
                    Console.Write($"Дата изготовления коробки {i + 1} (если не известно null) - ");
                    DateTime parseDataTime;
                    if (DateTime.TryParse(Console.ReadLine(), out parseDataTime))
                        boxes[i].ProductionDate = parseDataTime;
                    Console.Write($"Срок годности коробки {i + 1} (если не известно null) - ");
                    if (DateTime.TryParse(Console.ReadLine(), out parseDataTime))
                        boxes[i].ExpirationDateSet = parseDataTime;
                    else if (!boxes[i].ProductionDate.HasValue)
                    {
                        // Если пользователь не ввел ни дату производства, ни срок годности, будем считать, что изготовили коробку сегодня (для простоты проверки)
                        boxes[i].ProductionDate = DateTime.Now.Date;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неожиданный формат данных.Начнем сначала");
                    i--;
                }
            }
            var sortedPallet = await palletService.GetSortedPallet(pallets.ToList());
            var longestLifeTimePallet = palletService.GetLongestLifePallets(pallets.ToList());
            Output(sortedPallet, longestLifeTimePallet);
        }

        static async void Generated()
        {
            Console.Clear();
            Pallet[] pallets = generatorService.GeneratePallets();
            generatorService.GenerateBoxes(pallets);
            var sortedPallet = await palletService.GetSortedPallet(pallets.ToList());
            var longestLifeTimePallet = palletService.GetLongestLifePallets(pallets.ToList());
            Output(sortedPallet, longestLifeTimePallet);
        }

        static async void Prod()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("1 - Работать с паллетами");
                Console.WriteLine("2 - Работать с коробками");
                Console.WriteLine("3 - Анализировать склад");
                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.WriteLine("1 - Create");
                            Console.WriteLine("2 - Read");
                            Console.WriteLine("3 - Update");
                            Console.WriteLine("4 - Delete");
                            try
                            {
                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        await palletService.Add(InputPalletDto());
                                        break;
                                    case "2":
                                        {
                                            foreach (Pallet pallet in await palletService.GetAll())
                                            {
                                                Console.WriteLine(pallet);
                                            }
                                        }
                                        break;
                                    case "3":
                                        {
                                            Console.Write($"Номер обновляемого паллета - ");
                                            int id = int.Parse(Console.ReadLine());
                                            await palletService.Update(InputPalletDto(), id);
                                        }
                                        break;
                                    case "4":
                                        {
                                            Console.Write($"Номер удаляемого паллета - ");
                                            await palletService.Delete(int.Parse(Console.ReadLine()));
                                        }
                                        break;
                                }
                            }
                            catch (ArgumentException ex) { Console.WriteLine(ex.Message); }
                            break;
                        }
                    case "2":
                        {
                            Console.WriteLine("1 - Create");
                            Console.WriteLine("2 - Read");
                            Console.WriteLine("3 - Update");
                            Console.WriteLine("4 - Delete");
                            try
                            {
                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        await boxService.Add(InputBoxDto());
                                        break;
                                    case "2":
                                        {
                                            foreach (Box box in await boxService.GetAll())
                                            {
                                                Console.WriteLine(box);
                                            }
                                        }
                                        break;
                                    case "3":
                                        {
                                            Console.Write($"Номер обновляемой коробки - ");
                                            int id = int.Parse(Console.ReadLine());
                                            await boxService.Update(InputBoxDto(), id);
                                        }
                                        break;
                                    case "4":
                                        {
                                            Console.Write($"Номер удаляемой коробки - ");
                                            await boxService.Delete(int.Parse(Console.ReadLine()));
                                        }
                                        break;
                                }
                            }
                            catch (ArgumentException ex) { Console.WriteLine(ex.Message); }
                            break;
                        }
                    case "3":
                        {
                            var sortedPallet = await palletService.GetSortedPallet();
                            var longestLifeTimePallet = await palletService.GetLongestLifePallets();
                            Output(sortedPallet, longestLifeTimePallet);
                            break;
                        }
                    default: return;
                }
            }
        }

        private static PalletDto InputPalletDto()
        {
            while (true)
            {
                try
                {
                    PalletDto dto = new PalletDto();
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

        private static BoxDto InputBoxDto()
        {
            while (true)
            {
                try
                {
                    BoxDto dto = new BoxDto();
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

        private static void Output(List<List<Pallet>> sortedPallet, List<Pallet> longestLifeTimePallet)
        {
            foreach (var group in sortedPallet)
            {
                if (group.First().ExpirationDate > DateTime.Now) Console.ForegroundColor = ConsoleColor.Green;
                else Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Срок годности - {group.First().ExpirationDate?.ToString("dd.MM.yyyy")}");
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var pallet in group)
                {
                    Console.WriteLine(pallet);
                }
            }

            Console.WriteLine();

            foreach (var pallet in longestLifeTimePallet)
            {
                Console.WriteLine(pallet);
            }
        }
    }
}
