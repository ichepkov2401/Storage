using Storage.Bl.Service.Interfaces;
using Storage.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Bl.Service
{
    public class GeneratorService : IGeneratorService
    {
        readonly Random random = new Random(1);

        public Pallet[] GeneratePallets()
        {
            Pallet[] pallets = new Pallet[(int)GetNaturalRandom(10)];
            for (int i = 0; i < pallets.Length;)
            {
                pallets[i] = new Pallet() { Id = i + 1, Width = GetNaturalRandom(14), Height = GetNaturalRandom(6), Deep = GetNaturalRandom(14)};
                // Унификация паллетов (несколько может быть с одинаковыми параметрами)
                int count = random.Next(1, pallets.Length - i);
                for (int j = 1; j < count; j++)
                {
                    pallets[i + j] = new Pallet() { Id = i + j + 1, Width = pallets[i].Width, Height = pallets[i].Height, Deep = pallets[i].Deep };
                }
                i += count;
            }
            return pallets;
        }

        public Box[] GenerateBoxes(Pallet[] pallets)
        {
            Box[] boxes = new Box[(int)(GetNaturalRandom(100))];
            for (int i = 0; i < boxes.Length;)
            {
                boxes[i] = GenerateBox(i + 1, GetNaturalRandom(4), GetNaturalRandom(2), GetNaturalRandom(4), pallets);
                // Унификация коробок (несколько может быть с одинаковыми параметрами)
                int count = random.Next(1, boxes.Length - i);
                for (int j = 1; j < count; j++)
                {
                    boxes[i + j] = GenerateBox(i + j, boxes[i].Width, boxes[i].Height, boxes[i].Deep, pallets);
                }
                i += count;
            }
            return boxes;
        }

        private Box GenerateBox(int id, double width, double height, double deep, Pallet[] pallets)
        {
            Box box;
            while (true)
            {
                box= new Box() { Id = id, Width = width, Height = height, Deep = deep, Pallet = pallets[random.Next(pallets.Length)] };
                if (box.Deep <= box.Pallet.Deep && box.Width <= box.Pallet.Width)
                    break;
            }
            box.PalletId = box.Pallet.Id;
            box.Pallet.Boxes.Add(box);
            return box;
        }

        /// <summary>
        /// Генератор случайных выличин с натуральным распределением по центральной предельной теореме
        /// </summary>
        /// <returns>Псевдослучайное число с натуральным распределением по центральной предельной теореме</returns>
        private double GetNaturalRandom(int max)
        {
            int value = 0;
            for (int i = 0; i < 100; i++)
            {
                value += random.Next(max + 1);
            }
            return value / 100.0;
        }
    }
}
