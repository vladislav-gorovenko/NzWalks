namespace NZWalks.Domain.Models;

public class BaseEntity<T>
{
    public required T Id { get; set; }
    public required string Name { get; set; }
}