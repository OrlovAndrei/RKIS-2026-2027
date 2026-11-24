public class UpdateCommand : TaskCommand
{
    public string Text { get; set; } = "";

    protected override void Run()
    {
        if (string.IsNullOrWhiteSpace(Text))
        {
            Console.WriteLine("Укажите новый текст: update <номер> \"текст\"");
            return;
        }

        TodoList.Update(Index, Text);
        FileManager.SaveTodos(TodoList, FileManager.TodoFilePath);
        Console.WriteLine($"Задача №{Index + 1} изменена.");
    }
}
