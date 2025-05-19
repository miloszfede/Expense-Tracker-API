namespace ExpenseTracker.Models;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; set; }

    public Category(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
    }
}