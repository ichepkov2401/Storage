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
    public interface IBoxService
    {
        public Task Add(BoxDto boxDto);

        public Task<Box> GetById(int id);

        public Task<IQueryable<Box>> GetAll();

        public Task Update(BoxDto boxDto, int id);

        public Task Delete(int id);
    }
}
