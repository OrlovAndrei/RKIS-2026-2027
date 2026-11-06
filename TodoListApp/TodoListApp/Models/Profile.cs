using System.Security.Cryptography;

namespace TodoListApp.Models;

internal sealed class Profile
{
    public Guid Id { get; }
    public string Login { get; }
    public string Password { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public int BirthYear { get; }
    public string FullName => $"{FirstName} {LastName}";
    public int Age => DateTime.Now.Year - BirthYear;

    private Profile(
        Guid id,
        string login,
        string password,
        string firstName,
        string lastName,
        int birthYear)
    {
        Id = id;
        Login = login;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }

    public static Profile Create(
        string login,
        string password,
        string firstName,
        string lastName,
        int birthYear)
    {
        return new Profile(
            Guid.NewGuid(),
            login.Trim(),
            HashPassword(password),
            firstName.Trim(),
            lastName.Trim(),
            birthYear);
    }

    public static Profile FromStorage(
        Guid id,
        string login,
        string passwordHash,
        string firstName,
        string lastName,
        int birthYear)
    {
        return new Profile(id, login, passwordHash, firstName, lastName, birthYear);
    }

    public bool VerifyPassword(string password)
    {
        string[] parts = Password.Split('$');
        if (parts.Length != 3 || parts[0] != "v1")
        {
            return false;
        }

        byte[] salt;
        byte[] expectedHash;
        try
        {
            salt = Convert.FromBase64String(parts[1]);
            expectedHash = Convert.FromBase64String(parts[2]);
        }
        catch (FormatException)
        {
            return false;
        }

        if (salt.Length != 16 || expectedHash.Length != 32)
        {
            return false;
        }

        byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            120_000,
            HashAlgorithmName.SHA256,
            expectedHash.Length);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }

    public string GetInfo() => $"{FullName}, возраст {Age}";

    private static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            120_000,
            HashAlgorithmName.SHA256,
            32);

        return $"v1${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }
}
