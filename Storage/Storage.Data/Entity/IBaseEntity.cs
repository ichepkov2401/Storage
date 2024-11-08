using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Data.Entity
{
    public interface IBaseEntity
    {
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedDate { get; set; }

        // TODO: Если с системой могут работать несколько пользователей, ввести дополнительные поля для БД.

        /*
         *         public Guid? CreatedBy { get; set; }
         * 
         *         public Guid? ModifiedBy { get; set; }
         *
         *         public Guid? DeletedBy { get; set; }
         */
    }
}
