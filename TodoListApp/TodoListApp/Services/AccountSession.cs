using TodoListApp.Models;
using TodoListApp.Persistence;

namespace TodoListApp.Services;

internal static class AccountSession
{
    public static Profile SelectProfile()
    {
        while (true)
        {
            if (AppInfo.Profiles.Count == 0)
            {
                Console.WriteLine("Профилей пока нет. Создайте первый профиль.");
                return CreateProfile();
            }

            Console.Write("Войти в существующий профиль? [y/n]: ");
            string choice = ReadInput()?.Trim().ToLowerInvariant() ?? string.Empty;
            if (choice == "y")
            {
                Profile? profile = TryLogin();
                if (profile is not null)
                {
                    return profile;
                }
            }
            else if (choice == "n")
            {
                return CreateProfile();
            }
            else
            {
                Console.WriteLine("Введите y или n.");
            }
        }
    }

    private static Profile? TryLogin()
    {
        Console.Write("Логин: ");
        string login = ReadInput().Trim();
        Console.Write("Пароль: ");
        string password = ReadInput();

        Profile? profile = AppInfo.Profiles.FirstOrDefault(
            candidate => string.Equals(candidate.Login, login, StringComparison.OrdinalIgnoreCase));
        if (profile is null || !profile.VerifyPassword(password))
        {
            Console.WriteLine("Неверный логин или пароль.");
            return null;
        }

        return profile;
    }

    private static Profile CreateProfile()
    {
        string login;
        while (true)
        {
            login = ReadNonEmpty("Новый логин: ");
            if (!AppInfo.Profiles.Any(profile =>
                    string.Equals(profile.Login, login, StringComparison.OrdinalIgnoreCase)))
            {
                break;
            }

            Console.WriteLine("Этот логин уже занят.");
        }

        string password = ReadNonEmpty("Пароль: ");
        string firstName = ReadNonEmpty("Имя: ");
        string lastName = ReadNonEmpty("Фамилия: ");
        int birthYear = ReadBirthYear();

        Profile profile = Profile.Create(login, password, firstName, lastName, birthYear);
        AppInfo.Profiles.Add(profile);
        FileManager.SaveProfiles(AppInfo.Profiles);
        Console.WriteLine($"Добавлен пользователь {profile.FullName}, возраст - {profile.Age}");
        return profile;
    }

    private static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = ReadInput().Trim();
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            Console.WriteLine("Значение не должно быть пустым.");
        }
    }

    private static int ReadBirthYear()
    {
        int currentYear = DateTime.Now.Year;
        while (true)
        {
            Console.Write("Год рождения: ");
            if (int.TryParse(ReadInput(), out int year) && year is >= 1900 && year <= currentYear)
            {
                return year;
            }

            Console.WriteLine($"Введите год от 1900 до {currentYear}.");
        }
    }

    private static string ReadInput() =>
        Console.ReadLine() ?? throw new EndOfStreamException("Ввод завершён во время создания профиля.");
}
