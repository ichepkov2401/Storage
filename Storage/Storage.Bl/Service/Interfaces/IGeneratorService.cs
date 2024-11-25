using Storage.Data.Models.Input;

namespace Storage.Bl.Service.Interfaces
{
    public interface IGeneratorService
    {
        /// <summary>
        /// Генератор паллетов
        /// </summary>
        public PalletInputDto[] GeneratePallets();

        /// <summary>
        /// Генератор коробок
        /// </summary>
        /// <param name="pallets">Палетты на которые будут раздожены коробки</param>
        public BoxInputDto[] GenerateBoxes(PalletInputDto[] pallets);
    }
}
