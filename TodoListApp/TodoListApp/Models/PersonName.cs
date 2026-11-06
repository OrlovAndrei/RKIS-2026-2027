namespace TodoListApp.Models;

internal readonly record struct PersonName(string FirstName, string LastName)
{
    public override string ToString() => $"{FirstName} {LastName}";
}
