// Данные пользователя; задаются один раз при регистрации
public class Profile
{
    public string FirstName { get; }
    public string LastName { get; }
    public int BirthYear { get; }

    public Profile(string firstName, string lastName, int birthYear)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }

    public string GetInfo()
    {
        return $"{FirstName} {LastName}, возраст {DateTime.Now.Year - BirthYear}";
    }
}
