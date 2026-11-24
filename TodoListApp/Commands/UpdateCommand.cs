public class UpdateCommand : TaskCommand
{
    public string Text { get; set; } = "";

    // Прежний текст нужен, чтобы undo мог его вернуть
    private string _oldText = "";

    protected override bool Apply()
    {
        if (!CheckIndex())
            return false;

        if (string.IsNullOrWhiteSpace(Text))
        {
            Console.WriteLine("Укажите новый текст: update <номер> \"текст\"");
            return false;
        }

        _oldText = AppInfo.Todos[Index].Text;
        AppInfo.Todos.Update(Index, Text);
        Console.WriteLine($"Задача №{Index + 1} изменена.");
        return true;
    }

    public override void Unexecute()
    {
        AppInfo.Todos.Update(Index, _oldText);
        Console.WriteLine($"Изменение задачи №{Index + 1} отменено.");
    }
}
