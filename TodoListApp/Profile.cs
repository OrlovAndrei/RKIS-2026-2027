// Профиль пользователя; задаётся один раз при регистрации
public class Profile
{
    public Guid Id { get; }
    public string Login { get; }
    public string Password { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public int BirthYear { get; }

    // Новый профиль: уникальный Id создаём сами
    public Profile(string login, string password, string firstName, string lastName, int birthYear)
        : this(Guid.NewGuid(), login, password, firstName, lastName, birthYear)
    {
    }

    // Профиль, прочитанный из файла: Id берём из файла
    public Profile(Guid id, string login, string password, string firstName, string lastName, int birthYear)
    {
        Id = id;
        Login = login;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }

    public string GetInfo()
    {
        return $"{FirstName} {LastName}, возраст {DateTime.Now.Year - BirthYear}";
    }
}
