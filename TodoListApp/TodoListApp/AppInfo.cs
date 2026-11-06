using TodoListApp.Commands;
using TodoListApp.Models;
using TodoListApp.Persistence;

namespace TodoListApp;

internal static class AppInfo
{
    private static readonly Dictionary<Guid, TodoList> _todosByProfile = new();

    public static List<Profile> Profiles { get; private set; } = new();
    public static Guid? CurrentProfileId { get; private set; }
    public static Profile CurrentProfile =>
        Profiles.First(profile => profile.Id == CurrentProfileId);
    public static TodoList Todos =>
        CurrentProfileId is Guid id && _todosByProfile.TryGetValue(id, out TodoList? todos)
            ? todos
            : throw new InvalidOperationException("Войдите в профиль перед работой со списком.");
    public static Stack<ICommand> UndoStack { get; } = new();
    public static Stack<ICommand> RedoStack { get; } = new();
    public static bool ExitRequested { get; set; }

    public static void Initialize()
    {
        FileManager.EnsureDataDirectory();
        Profiles = FileManager.LoadProfiles();
    }

    public static void Activate(Profile profile)
    {
        CurrentProfileId = profile.Id;
        if (!_todosByProfile.TryGetValue(profile.Id, out TodoList? todos))
        {
            todos = FileManager.LoadTodos(profile.Id);
            todos.OnTodoAdded += FileManager.SaveTodoList;
            todos.OnTodoDeleted += FileManager.SaveTodoList;
            todos.OnTodoUpdated += FileManager.SaveTodoList;
            todos.OnStatusChanged += FileManager.SaveTodoList;
            _todosByProfile.Add(profile.Id, todos);
        }

        ClearHistory();
    }

    public static void SignOut()
    {
        CurrentProfileId = null;
        ClearHistory();
    }

    public static void ClearHistory()
    {
        UndoStack.Clear();
        RedoStack.Clear();
    }
}
