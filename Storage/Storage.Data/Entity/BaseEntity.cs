using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Data.Entity
{
    /// <summary>
    /// Базовая сущность для всех записей в БД.
    /// </summary>
    /// <typeparam name="T">Тип идентификатора (int или GUID).</typeparam>
    public abstract class BaseEntity<T> : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
