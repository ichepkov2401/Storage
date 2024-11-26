using Storage.Data.Models.Input;

namespace Storage.BusinessLogic.Service.Interfaces
{
    /// <summary>
    /// Сервис генераии данных
    /// </summary>
    public interface IGeneratorService
    {
        /// <summary>
        /// Генератор паллетов
        /// </summary>
        public PalletRequest[] GeneratePallets();

        /// <summary>
        /// Генератор коробок
        /// </summary>
        /// <param name="pallets">Палетты на которые будут раздожены коробки</param>
        public BoxRequest[] GenerateBoxes(PalletRequest[] pallets);
    }
}
