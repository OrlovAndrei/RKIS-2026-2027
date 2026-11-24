// Папку для данных проверяем до начала работы, затем загружаем все профили
FileManager.EnsureDataDirectory(FileManager.DataDirectory);
AppInfo.Profiles = FileManager.LoadProfiles(FileManager.ProfilesFilePath);
if (!File.Exists(FileManager.ProfilesFilePath))
    File.WriteAllText(FileManager.ProfilesFilePath, "");

// Главный цикл: строку разбирает CommandParser, а выполняет полученная команда
while (true)
{
    // Пока никто не вошёл (первый запуск или после profile --out), предлагаем войти или зарегистрироваться
    if (AppInfo.CurrentProfileId is null)
        ProfileSelector.Select();

    Console.Write("> ");
    string? input = Console.ReadLine();
    if (input is null) // ввод закрыт (Ctrl+D / Ctrl+Z) — завершаем работу
        break;
    if (string.IsNullOrWhiteSpace(input))
        continue;

    ICommand command = CommandParser.Parse(input);
    command.Execute();
}
