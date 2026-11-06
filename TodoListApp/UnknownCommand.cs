using System;

class UnknownCommand : ICommand
{
    public string Message { get; set; }

    public UnknownCommand(string message = "Неизвестная команда. Введите help для списка команд.")
    {
        Message = message;
    }

    public void Execute()
    {
        Console.WriteLine(Message);
    }
}

