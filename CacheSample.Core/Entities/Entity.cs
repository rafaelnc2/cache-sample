
namespace CacheSample.Core.Entities;

public abstract class Entity
{
    public int Id { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime CreateDate { get; set; }
}
