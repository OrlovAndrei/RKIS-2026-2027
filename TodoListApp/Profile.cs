namespace TodoListApp;

public sealed class Profile
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public int BirthYear { get; private set; }

    public Profile(string firstName, string lastName, int birthYear)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Имя не может быть пустым.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Фамилия не может быть пустой.", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }

    public string GetInfo()
    {
        int age = DateTime.Now.Year - BirthYear;
        return $"{FirstName} {LastName}, возраст {age}";
    }
}
