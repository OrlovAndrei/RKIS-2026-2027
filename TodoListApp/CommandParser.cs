// Превращает введённую строку в объект команды: разбирает имя, флаги и параметры
public static class CommandParser
{
    // Словарь «имя команды → метод, который разбирает аргументы и создаёт команду».
    // Благодаря ему в Parse не нужны длинные if или switch: новая команда — это одна строка в конструкторе
    private static readonly Dictionary<string, Func<string, ICommand>> _commandHandlers = new();

    // Статический конструктор выполняется один раз до первого использования класса — регистрируем все команды
    static CommandParser()
    {
        _commandHandlers["help"] = _ => new HelpCommand(); // лямбда: аргументы этой команде не нужны
        _commandHandlers["exit"] = _ => new ExitCommand();
        _commandHandlers["undo"] = _ => new UndoCommand();
        _commandHandlers["redo"] = _ => new RedoCommand();
        _commandHandlers["profile"] = ParseProfile;
        _commandHandlers["add"] = ParseAdd;
        _commandHandlers["view"] = ParseView;
        _commandHandlers["read"] = ParseRead;
        _commandHandlers["status"] = ParseStatus;
        _commandHandlers["update"] = ParseUpdate;
        _commandHandlers["delete"] = ParseDelete;
    }

    public static ICommand Parse(string inputString)
    {
        // Первое слово — имя команды, остальное — аргументы
        string[] parts = inputString.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            return Error("Введите команду, help - список команд.");

        string name = parts[0].ToLower();
        string args = parts.Length > 1 ? parts[1].Trim() : "";

        // Находим обработчик по имени команды и запускаем его с аргументами
        if (_commandHandlers.TryGetValue(name, out Func<string, ICommand>? handler))
            return handler(args);

        return Error($"Команды {name} нет, help - список команд.");
    }

    // profile — показать профиль, profile --out (-o) — выйти из профиля
    private static ICommand ParseProfile(string args)
    {
        var command = new ProfileCommand();

        foreach (string flag in args.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (flag == "--out" || flag == "-o")
                command.Out = true;
            else
                return Error($"Неизвестный флаг {flag}");
        }

        return command;
    }

    private static ICommand ParseAdd(string args)
    {
        if (args == "--multiline" || args == "-m")
            return new AddCommand { Multiline = true };

        // Кавычки вокруг текста необязательны: add "купить хлеб" == add купить хлеб
        return new AddCommand { Text = args.Trim('"').Trim() };
    }

    private static ICommand ParseView(string args)
    {
        var command = new ViewCommand();

        foreach (string flag in args.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (flag.StartsWith("--"))
            {
                switch (flag)
                {
                    case "--index": command.ShowIndex = true; break;
                    case "--status": command.ShowStatus = true; break;
                    case "--update-date": command.ShowDate = true; break;
                    case "--all": command.ShowIndex = command.ShowStatus = command.ShowDate = true; break;
                    default: return Error($"Неизвестный флаг {flag}");
                }
            }
            else if (flag.StartsWith("-") && flag.Length > 1)
            {
                // Короткие флаги можно склеивать: -isd — разбираем каждую букву отдельно
                foreach (char c in flag.Substring(1))
                {
                    switch (c)
                    {
                        case 'i': command.ShowIndex = true; break;
                        case 's': command.ShowStatus = true; break;
                        case 'd': command.ShowDate = true; break;
                        case 'a': command.ShowIndex = command.ShowStatus = command.ShowDate = true; break;
                        default: return Error($"Неизвестный флаг -{c}");
                    }
                }
            }
            else
            {
                return Error($"Неизвестный аргумент {flag}");
            }
        }

        return command;
    }

    private static ICommand ParseRead(string args)
    {
        if (!TryGetIndex(args, out int index))
            return Error("Номер задачи должен быть числом.");

        return new ReadCommand { Index = index };
    }

    private static ICommand ParseDelete(string args)
    {
        if (!TryGetIndex(args, out int index))
            return Error("Номер задачи должен быть числом.");

        return new DeleteCommand { Index = index };
    }

    // update <номер> "текст": первое слово — номер задачи, всё остальное — новый текст
    private static ICommand ParseUpdate(string args)
    {
        string[] parts = args.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0 || !TryGetIndex(parts[0], out int index))
            return Error("Номер задачи должен быть числом.");

        string text = parts.Length > 1 ? parts[1].Trim('"').Trim() : "";
        return new UpdateCommand { Index = index, Text = text };
    }

    // status <номер> <статус>, например: status 2 inprogress
    private static ICommand ParseStatus(string args)
    {
        string[] parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
            return Error("Формат: status <номер> <статус>, например status 2 inprogress");

        if (!TryGetIndex(parts[0], out int index))
            return Error("Номер задачи должен быть числом.");

        // TryParse принимает и числа ("7"), поэтому дополнительно проверяем, что такой статус есть
        if (!Enum.TryParse(parts[1], true, out TodoStatus status) || !Enum.IsDefined(status))
        {
            string names = string.Join(", ", Enum.GetNames<TodoStatus>()).ToLower();
            return Error($"Неизвестный статус «{parts[1]}». Доступны: {names}");
        }

        return new StatusCommand { Index = index, Status = status };
    }

    // Пользователь вводит номер с 1, а в списке индексы с 0 — переводим.
    // Существует ли задача с таким номером, проверит сама команда при выполнении
    private static bool TryGetIndex(string text, out int index)
    {
        bool isNumber = int.TryParse(text, out int number);
        index = number - 1;
        return isNumber;
    }

    private static ICommand Error(string message) => new ErrorCommand { Message = message };
}
