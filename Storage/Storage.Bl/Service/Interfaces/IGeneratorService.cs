using Storage.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Bl.Service.Interfaces
{
    internal interface IGeneratorService
    {
        /// <summary>
        /// Генератор паллетов
        /// </summary>
        public Pallet[] GeneratePallets();

        /// <summary>
        /// Генератор коробок
        /// </summary>
        /// <param name="pallets">Палетты на которые будут раздожены коробки</param>
        public Box[] GenerateBoxes(Pallet[] pallets);
    }
}
