using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Data.Entity
{
    /// <summary>
    /// Базовая сущность для всех записей в БД
    /// </summary>
    /// <typeparam name="T">Тип идентификатора (int или GUID)</typeparam>
    public abstract class BaseEntity<T>
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedDate { get; set; }

        // TODO: Если с системой могут работать несколько пользователей, ввести дополнительные поля для БД

        /*
         *         public Guid? CreatedBy { get; set; }
         * 
         *         public Guid? ModifiedBy { get; set; }
         *
         *         public Guid? DeletedBy { get; set; }
         */
    }
}
