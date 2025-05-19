namespace NZWalks.Domain.Models;

public class BaseEntity<T>
{
    public T Id { get; set; }
    public required string Name { get; set; }
}