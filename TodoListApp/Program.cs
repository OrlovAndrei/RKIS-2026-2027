using System;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("Работу выполнили Кузнецов и Курбатов");

        string? firstName = ReadRequired("Введите имя: ");
        if (firstName == null) return;

        string? lastName = ReadRequired("Введите фамилию: ");
        if (lastName == null) return;

        int? birthYear = ReadBirthYear();
        if (birthYear == null) return;

        Profile profile = new Profile(firstName, lastName, birthYear.Value);
        TodoList todoList = new TodoList();

        Console.WriteLine("Добавлен пользователь " + profile.GetInfo());

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (input == null) break;

            string command = input.Trim();
            if (command.Length == 0) continue;

            string name = command.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];
            switch (name)
            {
                case "help": PrintHelp(); break;
                case "profile": Console.WriteLine(profile.GetInfo()); break;
                case "add": AddTask(command, todoList); break;
                case "view": ViewTasks(command, todoList); break;
                case "read": ReadTask(command, todoList); break;
                case "done": DoneTask(command, todoList); break;
                case "delete": DeleteTask(command, todoList); break;
                case "update": UpdateTask(command, todoList); break;
                case "exit":
                    Console.WriteLine("Выход из программы");
                    return;
                default:
                    Console.WriteLine("Неизвестная команда. Введите help.");
                    break;
            }
        }
    }

    static string? ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? value = Console.ReadLine();
            if (value == null) return null;
            if (!string.IsNullOrWhiteSpace(value)) return value.Trim();
            Console.WriteLine("Поле не должно быть пустым");
        }
    }

    static int? ReadBirthYear()
    {
        while (true)
        {
            Console.Write("Введите год рождения: ");
            string? text = Console.ReadLine();
            if (text == null) return null;
            if (int.TryParse(text, out int year) && year >= 1900 && year <= DateTime.Now.Year)
                return year;
            Console.WriteLine("Введите корректный год рождения числом");
        }
    }

    static void PrintHelp()
    {
        Console.WriteLine("help - список команд");
        Console.WriteLine("profile - данные пользователя (имя, фамилия, возраст)");
        Console.WriteLine("add \"текст\" - добавить однострочную задачу");
        Console.WriteLine("add -m / --multiline - многострочная задача, завершение !end");
        Console.WriteLine("view - таблица только с текстом");
        Console.WriteLine("view -i/--index - номера задач");
        Console.WriteLine("view -s/--status - статусы");
        Console.WriteLine("view -d/--update-date - даты изменений");
        Console.WriteLine("view -a/--all - все колонки");
        Console.WriteLine("view -is / -isd - сочетания коротких флагов");
        Console.WriteLine("read <номер> - полный текст, статус, дата");
        Console.WriteLine("done <номер> - отметить выполненной");
        Console.WriteLine("delete <номер> - удалить задачу");
        Console.WriteLine("update <номер> \"текст\" - изменить задачу");
        Console.WriteLine("exit - выход");
    }

    static void AddTask(string command, TodoList todoList)
    {
        string arguments = command.Substring(3).Trim();
        if (arguments == "-m" || arguments == "--multiline" || arguments == "--multi-line")
        {
            AddMultilineTask(todoList);
            return;
        }

        string[] parts = command.Split('"');
        if (parts.Length != 3 || parts[0].Trim() != "add" ||
            parts[2].Trim() != "" || string.IsNullOrWhiteSpace(parts[1]))
        {
            Console.WriteLine("Формат: add \"текст задачи\" или add -m");
            return;
        }

        todoList.Add(new TodoItem(parts[1]));
        Console.WriteLine("Задача добавлена");
    }

    static void AddMultilineTask(TodoList todoList)
    {
        Console.WriteLine("Введите строки задачи. Для завершения напишите !end");
        StringBuilder text = new StringBuilder();
        bool firstLine = true;

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line == null)
            {
                Console.WriteLine("Ввод прерван, задача не добавлена");
                return;
            }
            if (line == "!end") break;
            if (!firstLine) text.Append('\n');
            text.Append(line);
            firstLine = false;
        }

        if (string.IsNullOrWhiteSpace(text.ToString()))
        {
            Console.WriteLine("Нельзя добавить пустую задачу");
            return;
        }

        todoList.Add(new TodoItem(text.ToString()));
        Console.WriteLine("Задача добавлена");
    }

    static void ViewTasks(string command, TodoList todoList)
    {
        bool showIndex = false;
        bool showDone = false;
        bool showDate = false;
        bool showAll = false;

        string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < parts.Length; i++)
        {
            string flag = parts[i];
            if (flag == "--all" || flag == "-a") showAll = true;
            else if (flag == "--index" || flag == "-i") showIndex = true;
            else if (flag == "--status" || flag == "-s") showDone = true;
            else if (flag == "--update-date" || flag == "-d") showDate = true;
            else if (flag.Length > 2 && flag[0] == '-' && flag[1] != '-')
            {
                foreach (char letter in flag.Substring(1))
                {
                    if (letter == 'a') showAll = true;
                    else if (letter == 'i') showIndex = true;
                    else if (letter == 's') showDone = true;
                    else if (letter == 'd') showDate = true;
                    else
                    {
                        Console.WriteLine("Неизвестный флаг: -" + letter);
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Неизвестный флаг: " + flag);
                return;
            }
        }

        if (showAll)
        {
            showIndex = true;
            showDone = true;
            showDate = true;
        }
        todoList.View(showIndex, showDone, showDate);
    }

    // В команде пользователь вводит номер с 1, а массив индексируется с 0.
    static bool TryGetIndex(string numberText, TodoList todoList, out int index)
    {
        index = -1;
        if (!int.TryParse(numberText, out int number) ||
            number < 1 || todoList.GetItem(number - 1) == null)
        {
            Console.WriteLine("Задачи с таким номером нет");
            return false;
        }
        index = number - 1;
        return true;
    }

    static void ReadTask(string command, TodoList todoList)
    {
        string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            Console.WriteLine("Формат: read 1");
            return;
        }
        if (!TryGetIndex(parts[1], todoList, out int index)) return;

        TodoItem? item = todoList.GetItem(index);
        if (item == null) return;
        Console.WriteLine("Задача №" + (index + 1));
        Console.WriteLine(item.GetFullInfo());
    }

    static void DoneTask(string command, TodoList todoList)
    {
        string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            Console.WriteLine("Формат: done 1");
            return;
        }
        if (!TryGetIndex(parts[1], todoList, out int index)) return;

        TodoItem? item = todoList.GetItem(index);
        if (item == null) return;
        item.MarkDone();
        Console.WriteLine("Задача выполнена");
    }

    static void DeleteTask(string command, TodoList todoList)
    {
        string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            Console.WriteLine("Формат: delete 1");
            return;
        }
        if (!TryGetIndex(parts[1], todoList, out int index)) return;

        if (todoList.Delete(index))
            Console.WriteLine("Задача удалена");
    }

    static void UpdateTask(string command, TodoList todoList)
    {
        string[] parts = command.Split('"');
        string[] head = parts[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3 || head.Length != 2 || head[0] != "update" ||
            parts[2].Trim() != "" || string.IsNullOrWhiteSpace(parts[1]))
        {
            Console.WriteLine("Формат: update 1 \"новый текст\"");
            return;
        }
        if (!TryGetIndex(head[1], todoList, out int index)) return;

        TodoItem? item = todoList.GetItem(index);
        if (item == null) return;
        item.UpdateText(parts[1]);
        Console.WriteLine("Задача изменена");
    }
}
