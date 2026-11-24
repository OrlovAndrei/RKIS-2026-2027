using System;

// Информация о пользователе.
public class Profile
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int BirthYear { get; private set; }

    public Profile(string firstName, string lastName, int birthYear)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Имя и фамилия не должны быть пустыми");
        if (birthYear < 1900 || birthYear > DateTime.Now.Year)
            throw new ArgumentException("Некорректный год рождения", nameof(birthYear));

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
