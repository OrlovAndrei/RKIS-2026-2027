using System;

namespace TodoListApp;

public class ProfileCommand : ICommand
{
    public Profile Profile { get; set; } = null!;

    public void Execute()
    {
        Console.WriteLine(Profile.GetInfo());
    }
}
