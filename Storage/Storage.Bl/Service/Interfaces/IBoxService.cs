using Storage.Data.Models.Input;
using Storage.Data.Models.Output;

namespace Storage.Bl.Service.Interfaces
{
    public interface IBoxService
    {
        public Task Add(BoxInputDto boxDto);

        public Task<BoxOutputDto[]> GetAll();

        public Task Update(BoxInputDto boxDto, int id);

        public Task Delete(int id);
    }
}
