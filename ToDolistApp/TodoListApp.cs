using System;

namespace TodoListApp;

partial class Program
{
    private const int InitialCapacity = 2;
    private const int CurrentYear = 2026;

    private static string[] _todos = new string[InitialCapacity];
    private static bool[] _statuses = new bool[InitialCapacity];
    private static DateTime[] _dates = new DateTime[InitialCapacity];

    private static int _todoCount = 0;

    static void Main(string[] args) 
    {
        Console.WriteLine("Работу выполнил Вахрамеев");
        
        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine() ?? "";

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine() ?? "";

        Console.Write("Введите год рождения: ");
        string yearInput = Console.ReadLine() ?? "";

        int birthYear = Convert.ToInt32(string.IsNullOrWhiteSpace(yearInput) ? "0" : yearInput);
        int age = CurrentYear - birthYear;

        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст {age}");
        Console.WriteLine("--------------------------------------------------");

        while (true)
        {
            Console.Write("\nВведите команду: ");
            string input = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrEmpty(input)) continue;

            string command = input.Split(' ')[0].ToLower();

            switch (command)
            {
                case "help":
                    HandleHelp();
                    break;

                case "profile":
                    Console.WriteLine($"{firstName} {lastName}, {birthYear}");
                    break;

                case "add":
                    HandleAdd(input);
                    break;

                case "view":
                    HandleView();
                    break;

                case "exit":
                    Console.WriteLine("Программа завершена. До свидания!");
                    return;

                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                    break;
                case "done":
                    HandleDone(input);
                    break;

                case "delete":
                    HandleDelete(input);
                    break;

                case "update":
                    HandleUpdate(input);
                    break;

            }
        }
    }


    private static void HandleHelp()
    {
        Console.WriteLine(@"Доступные команды:
  help                    - список команд
  profile                 - данные пользователя
  add <текст>             - добавить новую задачу
  view                    - показать список всех задач
  done <индекс>           - отметить задачу выполненной
  delete <индекс>         - удалить задачу по индексу
  update <индекс> <текст> - обновить текст задачи
  exit                    - выход");
    }


    private static void HandleAdd(string input)
    {
        if (input.Length <= 4)
        {
            Console.WriteLine("Ошибка: введите текст задачи после add.");
            return;
        }

        string taskText = input.Substring(4).Trim('"');

        if (_todoCount >= _todos.Length)
        {
            ResizeArrays();
        }

        _todos[_todoCount] = taskText;
        _statuses[_todoCount] = false;  
        _dates[_todoCount] = DateTime.Now;
        _todoCount++; 

        Console.WriteLine("Задача успешно добавлена!");
    }

    private static void HandleView()
    {
        if (_todoCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        for (int i = 0; i < _todoCount; i++)
        {
            string statusText = _statuses[i] ? "сделано" : "не сделано";
             Console.WriteLine($"{i + 1}. {_todos[i]} <{statusText}> <{_dates[i]:dd.MM.yyyy HH:mm:ss}>");
        }
    }
    private static void HandleDone(string input)
    {
        string[] parts = input.Split(' ');
        if (parts.Length < 2 || !int.TryParse(parts[1], out int index))
        {
            Console.WriteLine("Ошибка: укажите числовой индекс. Пример: done 1");
            return;
        }

        int actualIndex = index - 1;
        if (actualIndex < 0 || actualIndex >= _todoCount)
        {
            Console.WriteLine("Ошибка: неверный индекс задачи.");
            return;
        }

        _statuses[actualIndex] = true; 
        _dates[actualIndex] = DateTime.Now; 
        Console.WriteLine($"Задача №{index} отмечена выполненной.");
    }
    private static void HandleDelete(string input)
    {
        string[] parts = input.Split(' ');
        if (parts.Length < 2 || !int.TryParse(parts[1], out int index))
        {
            Console.WriteLine("Ошибка: укажите числовой индекс. Пример: delete 1");
            return;
        }

        int actualIndex = index - 1;
        if (actualIndex < 0 || actualIndex >= _todoCount)
        {
            Console.WriteLine("Ошибка: неверный индекс задачи.");
            return;
        }

        for (int i = actualIndex; i < _todoCount - 1; i++)
        {
            _todos[i] = _todos[i + 1];
            _statuses[i] = _statuses[i + 1];
            _dates[i] = _dates[i + 1];
        }

        _todoCount--;
        _todos[_todoCount] = null!;
        _statuses[_todoCount] = false;
        _dates[_todoCount] = default;

        Console.WriteLine($"Задача №{index} успешно удалена со сдвигом.");
    }
     private static void HandleUpdate(string input)
    {
        string[] parts = input.Split(' ', 3);
        if (parts.Length < 3 || !int.TryParse(parts[1], out int index))
        {
            Console.WriteLine("Ошибка: формат команды: update <idx> \"new_text\"");
            return;
        }

        int actualIndex = index - 1;
        if (actualIndex < 0 || actualIndex >= _todoCount)
        {
            Console.WriteLine("Ошибка: неверный индекс задачи.");
            return;
        }

        string newText = parts[2].Trim('"');
        _todos[actualIndex] = newText; // В todos записывается новый текст
        _dates[actualIndex] = DateTime.Now; // В dates обновляется текущая дата

        Console.WriteLine($"Задача №{index} успешно обновлена.");
    }
    private static void ResizeArrays()
    {
        int newLength = _todos.Length * 2;
        string[] newTodos = new string[newLength];
        bool[] newStatuses = new bool[newLength];
        DateTime[] newDates = new DateTime[newLength];

        for (int i = 0; i < _todos.Length; i++)
        {
            newTodos[i] = _todos[i];
            newStatuses[i] = _statuses[i];
            newDates[i] = _dates[i];
        }

        _todos = newTodos;
        _statuses = newStatuses;
        _dates = newDates;

        Console.WriteLine($"Все массивы расширены до {_todos.Length} элементов.");
    }
}
 