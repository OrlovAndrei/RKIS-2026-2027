// Подключаем функции консольного вывода.
using System;

// Создаём команду вывода справки.
public class HelpCommand : ICommand
{
    // Выполняем команду справки.
    public void Execute()
    {
        Console.WriteLine("help    - список команд");
        Console.WriteLine("profile - данные пользователя");
        Console.WriteLine("add \"текст\" - добавить задачу");
        Console.WriteLine("add --multiline (-m) - многострочная задача, конец: !end");
        Console.WriteLine("view - показать задачи");
        Console.WriteLine("view --index/-i --status/-s --update-date/-d");
        Console.WriteLine("view --all/-a - показать все столбцы");
        Console.WriteLine("read <номер> - полная информация о задаче");
        Console.WriteLine("status <номер> <статус> - изменить статус (notstarted, inprogress, completed, postponed, failed)");
        Console.WriteLine("update <номер> \"новый текст\" - изменить задачу");
        Console.WriteLine("delete <номер> - удалить задачу");
        Console.WriteLine("exit - завершить программу");
    }
}