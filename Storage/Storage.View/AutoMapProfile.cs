using AutoMapper;
using Storage.Data.Entity;
using Storage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.View
{
    public class AutoMapProfile : Profile
    {
        public AutoMapProfile()
        {
            CreateMap<PalletDto, Pallet>();
            CreateMap<BoxDto, Box>();
        }
    }
}
