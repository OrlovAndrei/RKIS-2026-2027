using System;

namespace TodoListApp;

public class UnknownCommand : ICommand
{
    public string Message { get; set; } = "Неизвестная команда. Введите 'help' для списка команд.";

    public void Execute()
    {
        Console.WriteLine(Message);
    }
}
