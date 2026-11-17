using System;

class ProfileCommand : ICommand
{
    public Profile? Profile { get; set; }
    public string? ProfileFilePath { get; set; }

    public void Execute()
    {
        if (Profile == null)
        {
            Console.WriteLine("Пользователь не зарегистрирован.");
            return;
        }

        Console.WriteLine(Profile.GetInfo());

        if (!string.IsNullOrEmpty(ProfileFilePath))
        {
            FileManager.SaveProfile(Profile, ProfileFilePath);
        }
    }
}

