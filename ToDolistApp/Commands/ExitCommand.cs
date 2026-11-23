using System;

namespace TodoListApp;

public class ExitCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Программа завершена. До свидания!");
        Environment.Exit(0);
    }
}