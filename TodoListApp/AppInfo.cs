// Общие данные программы. Класс статический, поэтому команды берут нужное отсюда
// и список задач с профилем не надо передавать в каждый метод
public static class AppInfo
{
    // Все профили из файла profile.csv
    public static List<Profile> Profiles { get; set; } = new();

    // Id вошедшего пользователя; null, пока никто не вошёл (при запуске и после profile --out)
    public static Guid? CurrentProfileId { get; set; }

    // Задачи всех пользователей, которых мы загружали: ключ — Id профиля, значение — список его задач
    public static Dictionary<Guid, List<TodoItem>> AllTodos { get; } = new();

    // Задачи текущего пользователя. TodoList — обёртка над его списком из AllTodos,
    // поэтому изменения через Todos сразу видны и в словаре
    public static TodoList Todos { get; set; } = new TodoList();

    // Профиль вошедшего пользователя (или null, если никто не вошёл)
    public static Profile? CurrentProfile
    {
        get
        {
            foreach (Profile profile in Profiles)
            {
                if (profile.Id == CurrentProfileId)
                    return profile;
            }
            return null;
        }
    }

    // Выполненные команды (для undo) и отменённые команды (для redo).
    // Стек подходит идеально: отменять нужно последнюю команду первой
    public static Stack<UndoableCommand> UndoStack { get; } = new();
    public static Stack<UndoableCommand> RedoStack { get; } = new();
}
