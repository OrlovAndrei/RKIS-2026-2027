using System.Text;

public class AddCommand : UndoableCommand
{
    public string Text { get; set; } = "";

    // Флаг --multiline / -m: текст задачи вводится построчно уже при выполнении команды
    public bool Multiline { get; set; }

    // Созданную задачу запоминаем: undo уберёт её из списка, а redo вернёт именно её же
    private TodoItem? _item;

    protected override bool Apply()
    {
        // При redo задача уже создана, поэтому текст заново не вводим и не проверяем
        if (_item is null)
        {
            string text = Multiline ? ReadMultiline() : Text;

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Текст задачи не может быть пустым.");
                return false;
            }
            if (AppInfo.Todos.Contains(text))
            {
                Console.WriteLine("Такая задача уже есть в списке.");
                return false;
            }
            _item = new TodoItem(text);
        }

        AppInfo.Todos.Add(_item);
        Console.WriteLine($"Задача добавлена под номером {AppInfo.Todos.Count}.");
        return true;
    }

    public override void Unexecute()
    {
        // Отмена идёт строго в обратном порядке, поэтому добавленная задача сейчас последняя в списке
        AppInfo.Todos.Delete(AppInfo.Todos.Count - 1);
        Console.WriteLine("Добавление задачи отменено.");
    }

    // Читаем строки до !end и склеиваем их через \n в один текст задачи
    private static string ReadMultiline()
    {
        Console.WriteLine("Вводите строки задачи, для завершения введите !end");
        var sb = new StringBuilder();
        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line is null || line.Trim() == "!end")
                break;

            if (sb.Length > 0)
                sb.Append('\n');
            sb.Append(line);
        }
        return sb.ToString().Trim();
    }
}
