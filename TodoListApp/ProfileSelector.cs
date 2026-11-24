// Вход в профиль или регистрация нового. Вызывается, пока в программу никто не вошёл
public static class ProfileSelector
{
    public static void Select()
    {
        while (true)
        {
            Profile? profile;
            if (AppInfo.Profiles.Count == 0)
            {
                Console.WriteLine("Профилей пока нет, создадим новый.");
                profile = Register();
            }
            else if (AskYesNo("Войти в существующий профиль? [y/n]"))
            {
                profile = Login(); // null — пользователь передумал и вернулся к вопросу
            }
            else
            {
                profile = Register();
            }

            if (profile is not null)
            {
                Enter(profile);
                return;
            }
        }
    }

    // Входим в профиль: запоминаем Id, загружаем его задачи и сбрасываем историю команд
    private static void Enter(Profile profile)
    {
        AppInfo.CurrentProfileId = profile.Id;

        string path = FileManager.GetTodosFilePath(profile.Id);
        List<TodoItem> todos = FileManager.LoadTodos(path);
        AppInfo.AllTodos[profile.Id] = todos;
        AppInfo.Todos = new TodoList(todos);
        if (!File.Exists(path))
            FileManager.SaveTodos(todos, path); // у нового пользователя файл создаём пустым

        // Любое изменение списка автоматически сохраняется в файл. Если когда-нибудь данные понадобится
        // хранить в базе, достаточно подписать на эти события другой метод — TodoList и команды менять не придётся
        AppInfo.Todos.OnTodoAdded += FileManager.SaveTodoList;
        AppInfo.Todos.OnTodoDeleted += FileManager.SaveTodoList;
        AppInfo.Todos.OnTodoUpdated += FileManager.SaveTodoList;
        AppInfo.Todos.OnStatusChanged += FileManager.SaveTodoList;

        // История команд принадлежит прошлому пользователю — undo/redo между профилями не переносим
        AppInfo.UndoStack.Clear();
        AppInfo.RedoStack.Clear();

        Console.WriteLine($"Вы вошли как {profile.Login}. Введите help, чтобы увидеть список команд.");
    }

    // Возвращает null, если пользователь ввёл пустой логин (передумал входить)
    private static Profile? Login()
    {
        while (true)
        {
            string login = ReadLine("Логин (пустая строка — назад): ").Trim();
            if (login.Length == 0)
                return null;
            string password = ReadLine("Пароль: ");

            foreach (Profile profile in AppInfo.Profiles)
            {
                if (profile.Login == login && profile.Password == password)
                    return profile;
            }
            Console.WriteLine("Неверный логин или пароль.");
        }
    }

    private static Profile Register()
    {
        string login = ReadField("Логин: ");
        while (LoginExists(login))
        {
            Console.WriteLine("Такой логин уже занят.");
            login = ReadField("Логин: ");
        }

        string password = ReadField("Пароль: ");
        string firstName = ReadField("Имя: ");
        string lastName = ReadField("Фамилия: ");

        int currentYear = DateTime.Now.Year;
        int birthYear;
        while (true)
        {
            // TryParse не бросает исключение на некорректной строке, в отличие от int.Parse
            if (int.TryParse(ReadLine("Год рождения: "), out birthYear) && birthYear >= 1900 && birthYear <= currentYear)
                break;

            Console.WriteLine($"Год рождения должен быть числом от 1900 до {currentYear}!");
        }

        var profile = new Profile(login, password, firstName, lastName, birthYear);
        AppInfo.Profiles.Add(profile);
        FileManager.SaveProfiles(AppInfo.Profiles, FileManager.ProfilesFilePath);
        Console.WriteLine($"Профиль создан: {profile.GetInfo()}");
        return profile;
    }

    private static bool LoginExists(string login)
    {
        foreach (Profile profile in AppInfo.Profiles)
        {
            if (profile.Login == login)
                return true;
        }
        return false;
    }

    private static bool AskYesNo(string question)
    {
        while (true)
        {
            string answer = ReadLine(question + " ").Trim().ToLower();
            if (answer == "y")
                return true;
            if (answer == "n")
                return false;
        }
    }

    // Поле профиля не должно быть пустым и содержать ';', иначе сломается разбор profile.csv
    private static string ReadField(string prompt)
    {
        while (true)
        {
            string value = ReadLine(prompt).Trim();
            if (value.Length > 0 && !value.Contains(';'))
                return value;

            Console.WriteLine("Поле не должно быть пустым и не может содержать «;».");
        }
    }

    // ReadLine вернёт null, если ввод закрыт (Ctrl+D / Ctrl+Z) — тогда выходим из программы
    private static string ReadLine(string prompt)
    {
        Console.Write(prompt);
        string? line = Console.ReadLine();
        if (line is null)
            Environment.Exit(0);
        return line;
    }
}
