public class UpdateCommand : TaskCommand
{
    public string Text { get; set; } = "";

    protected override void ExecuteOnItem(TodoItem item)
    {
        if (string.IsNullOrWhiteSpace(Text))
        {
            Console.WriteLine("Укажите новый текст: update <номер> \"текст\"");
            return;
        }

        item.UpdateText(Text);
        Console.WriteLine($"Задача №{Index + 1} изменена.");
    }
}
