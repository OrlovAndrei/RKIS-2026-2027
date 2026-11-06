using System;

class HelpCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Список команд:");
        Console.WriteLine("  help - показать список доступных команд");
        Console.WriteLine("  profile - вывести данные пользователя");
        Console.WriteLine("  add \"текст\" - добавить новую задачу");
        Console.WriteLine("  add --multiline (-m) - добавить многострочную задачу, ввод завершается строкой !end");
        Console.WriteLine("  view [флаги] - показать задачи в виде таблицы");
        Console.WriteLine("      --index (-i) - показывать номер задачи");
        Console.WriteLine("      --status (-s) - показывать статус задачи");
        Console.WriteLine("      --update-date (-d) - показывать дату изменения");
        Console.WriteLine("      --all (-a) - показывать все данные");
        Console.WriteLine("      короткие флаги можно объединять, например: view -is");
        Console.WriteLine("  done <номер> - отметить задачу выполненной");
        Console.WriteLine("  delete <номер> - удалить задачу");
        Console.WriteLine("  update <номер> \"текст\" - изменить задачу");
        Console.WriteLine("  read <номер> - показать полный текст задачи, статус и дату изменения");
        Console.WriteLine("  exit - завершить работу программы");
    }
}

