using Storage.Bl.Service.Interfaces;
using Storage.Data.Models.Input;
using Storage.View.Configuration;
using Storage.View.Controllers;

namespace Storage.View
{
    public static class Program
    {

        static IPalletService palletService = AutofacIntegration.GetInstance<IPalletService>();
        static IGeneratorService generatorService = AutofacIntegration.GetInstance<IGeneratorService>();

        static void Main(string[] args)
        {
            Config.LoadSettings();
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
            PalletInputDto[] pallets = new PalletInputDto[int.Parse(Console.ReadLine())];
            for (int i = 0; i < pallets.Length; i++)
            {
                try
                {
                    pallets[i] = ViewTools.InputPalletDto();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неожиданный формат данных. Начнем сначала");
                    i--;
                }
            }
            Console.WriteLine("Сколько на складе коробок?");
            BoxInputDto[] boxes = new BoxInputDto[int.Parse(Console.ReadLine())];
            for (int i = 0; i < boxes.Length; i++)
            {
                try
                {
                    while (true)
                    {
                        boxes[i] = ViewTools.InputBoxDto();
                        boxes[i].PalletId--;
                        if (boxes[i].Deep > pallets[boxes[i].PalletId].Deep || boxes[i].Width > pallets[boxes[i].PalletId].Width)
                        {
                            Console.WriteLine("Коробка не должна превышать по размерам паллету (по ширине и глубине)");
                            continue;
                        }
                        break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неожиданный формат данных.Начнем сначала");
                    i--;
                }
            }
            var sortedPallet = await palletService.GetSortedPallet((pallets, boxes));
            var longestLifeTimePallet = await palletService.GetLongestLifePallets((pallets, boxes));
            ViewTools.Output(sortedPallet, longestLifeTimePallet);
        }

        static async void Generated()
        {
            Console.Clear();
            PalletInputDto[] pallets = generatorService.GeneratePallets();
            BoxInputDto[] boxes = generatorService.GenerateBoxes(pallets);
            var sortedPallet = await palletService.GetSortedPallet((pallets, boxes));
            var longestLifeTimePallet = await palletService.GetLongestLifePallets((pallets, boxes));
            ViewTools.Output(sortedPallet, longestLifeTimePallet);
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
                            PalletController.PalletCRUD();
                            break;
                        }
                    case "2":
                        {
                            BoxController.BoxCRUD();
                            break;
                        }
                    case "3":
                        {
                            var sortedPallet = await palletService.GetSortedPallet();
                            var longestLifeTimePallet = await palletService.GetLongestLifePallets();
                            ViewTools.Output(sortedPallet, longestLifeTimePallet);
                            break;
                        }
                    default: return;
                }
            }
        }


    }
}
