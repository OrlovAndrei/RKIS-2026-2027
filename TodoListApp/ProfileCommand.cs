using System;

class ProfileCommand : ICommand
{
    public Profile? Profile { get; set; }

    public void Execute()
    {
        if (Profile == null)
        {
            Console.WriteLine("Пользователь не зарегистрирован.");
            return;
        }

        Console.WriteLine(Profile.GetInfo());
    }
}

