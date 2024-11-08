using AutoMapper;
using Storage.Data.Entity;
using Storage.Data.Models;
using Storage.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Bl.Service.Interfaces
{
    public interface IPalletService
    {
        public Task Add(PalletDto palletDto);

        public Task<Pallet> GetById(int id);

        public Task<IQueryable<Pallet>> GetAll();


        public Task Update(PalletDto palletDto, int id);

        public Task Delete(int id);

        /// <summary>
        /// Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу.
        /// </summary>
        public Task<List<List<Pallet>>> GetSortedPallet(List<Pallet> pallets = null);

        /// <summary>
        /// 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.
        /// </summary>
        public Task<List<Pallet>> GetLongestLifePallets();

        /// <summary>
        /// 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.
        /// </summary>
        public List<Pallet> GetLongestLifePallets(List<Pallet> pallets);
    }
}
