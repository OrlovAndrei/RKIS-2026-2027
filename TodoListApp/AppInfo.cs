// Общие данные программы. Класс статический, поэтому команды берут нужное отсюда
// и список задач с профилем не надо передавать в каждый метод
public static class AppInfo
{
    // Задачи текущего пользователя
    public static TodoList Todos { get; set; } = new TodoList();

    // Текущий профиль; задаётся при запуске в Program.cs
    public static Profile CurrentProfile { get; set; } = null!;

    // Выполненные команды (для undo) и отменённые команды (для redo).
    // Стек подходит идеально: отменять нужно последнюю команду первой
    public static Stack<UndoableCommand> UndoStack { get; } = new();
    public static Stack<UndoableCommand> RedoStack { get; } = new();
}
