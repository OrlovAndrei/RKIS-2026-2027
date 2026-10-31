using System;

// Каждая команда реализует один и тот же интерфейс ICommand.
public class HelpCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("help - список команд");
        Console.WriteLine("profile - данные пользователя");
        Console.WriteLine("add \"текст\" - добавить задачу");
        Console.WriteLine("add -m / --multiline - многострочная задача, конец: !end");
        Console.WriteLine("view - таблица с текстом задач");
        Console.WriteLine("view -i / --index - номера задач");
        Console.WriteLine("view -s / --status - статусы");
        Console.WriteLine("view -d / --update-date - даты изменений");
        Console.WriteLine("view -a / --all - все колонки");
        Console.WriteLine("view -is, -isd - сочетания коротких флагов");
        Console.WriteLine("read <номер> - полный текст задачи");
        Console.WriteLine("done <номер> - отметить выполненной");
        Console.WriteLine("update <номер> \"текст\" - изменить задачу");
        Console.WriteLine("delete <номер> - удалить задачу");
        Console.WriteLine("exit - выход");
    }
}

public class ProfileCommand : ICommand
{
    public Profile Profile { get; set; } = null!;

    public void Execute()
    {
        Console.WriteLine(Profile.GetInfo());
    }
}

public class ExitCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Выход из программы");
    }
}

// Сообщение об ошибке — тоже команда: это позволяет всегда возвращать ICommand.
public class MessageCommand : ICommand
{
    public string Text { get; set; } = "";

    public void Execute()
    {
        Console.WriteLine(Text);
    }
}
