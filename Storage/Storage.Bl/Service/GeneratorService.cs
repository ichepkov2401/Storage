using Storage.Bl.Service.Interfaces;
using Storage.Data.Models.Input;

namespace Storage.Bl.Service
{
    public class GeneratorService : IGeneratorService
    {
        readonly Random random = new Random(1);
        readonly DateTime start = new DateTime(2020, 1, 1);

        public PalletInputDto[] GeneratePallets()
        {
            PalletInputDto[] pallets = new PalletInputDto[(int)GetNaturalRandom(10)];
            for (int i = 0; i < pallets.Length;)
            {
                pallets[i] = new PalletInputDto() { Width = GetNaturalRandom(14), Height = GetNaturalRandom(6), Deep = GetNaturalRandom(14) };
                // Унификация паллетов (несколько может быть с одинаковыми параметрами)
                int count = random.Next(1, pallets.Length - i);
                for (int j = 1; j < count; j++)
                {
                    pallets[i + j] = new PalletInputDto() { Width = pallets[i].Width, Height = pallets[i].Height, Deep = pallets[i].Deep };
                }
                i += count;
            }
            return pallets;
        }

        public BoxInputDto[] GenerateBoxes(PalletInputDto[] pallets)
        {
            BoxInputDto[] boxes = new BoxInputDto[(int)(GetNaturalRandom(100))];
            for (int i = 0; i < boxes.Length;)
            {
                double width, deep;
                while (true)
                {
                    width = GetNaturalRandom(4);
                    deep = GetNaturalRandom(4);
                    if (pallets.Any(x => x.Deep >= deep && x.Width >= width))
                        break;
                }
                PalletInputDto[] goodPallet = pallets.Where(x => x.Deep >= deep && x.Width >= width).ToArray();
                boxes[i] = GenerateBox(i + 1, width, GetNaturalRandom(2), deep, GetNaturalRandom(30), goodPallet);
                // Унификация коробок (несколько может быть с одинаковыми параметрами)
                int count = random.Next(1, Math.Max(1, (boxes.Length - i) / 2));
                for (int j = 1; j < count; j++)
                {
                    boxes[i + j] = GenerateBox(i + j, width, boxes[i].Height, deep, boxes[i].Weight, goodPallet);
                }
                i += count;
            }
            return boxes;
        }

        /// <summary>
        /// Генерация коробки
        /// </summary>
        /// <param name="id">Id коробки</param>
        /// <param name="width">Ширина коробки</param>
        /// <param name="height">Высота коробки</param>
        /// <param name="deep">Глубина коробки</param>
        /// <param name="weight">Вес коробки</param>
        /// <param name="pallets">Палеты на которые может быть размещена коробка</param>
        /// <returns></returns>
        private BoxInputDto GenerateBox(int id, double width, double height, double deep, double weight, PalletInputDto[] pallets)
        {
            BoxInputDto box;
            while (true)
            {
                box = new BoxInputDto() { Width = width, Height = height, Deep = deep, Weight = weight, PalletId = random.Next(pallets.Length) };
                if (box.Deep <= pallets[box.PalletId].Deep && box.Width <= pallets[box.PalletId].Width)
                    break;
            }
            int days = (DateTime.Now - start).Days;
            int add = (int)GetNaturalRandom(days * 2);
            if (add > days) add = days * 2 - add;
            if (random.Next(2) == 0)
                box.ProductionDate = start.AddDays(add);
            else
                box.ExpirationDate = start.AddDays(add);
            return box;
        }

        /// <summary>
        /// Генератор случайных выличин с натуральным распределением по центральной предельной теореме
        /// </summary>
        /// <returns>Псевдослучайное число с натуральным распределением по центральной предельной теореме</returns>
        private double GetNaturalRandom(int max)
        {
            int value = 0;
            for (int i = 0; i < 128000; i++)
            {
                value += random.Next(max + 1);
            }
            return value / 128000.0;
        }
    }
}
