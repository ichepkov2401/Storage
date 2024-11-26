using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Data.Entity
{
    /// <summary>
    /// Сущность паллета на складе.
    /// </summary>
    public class Pallet : StorageUnitEntity
    {
        
        public virtual ICollection<Box> Boxes { get; } = new List<Box>();
        
        public IEnumerable<Box> RealBoxes => Boxes.Where(x => !x.DeletedDate.HasValue);
    }
}
