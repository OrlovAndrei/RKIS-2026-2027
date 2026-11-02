namespace TodoListApp
{
    internal class UpdateCommand : ICommand
    {
        public int Index { get; set; }

        public string Text { get; set; } = "";

        public TodoList? TodoList { get; set; }

        public void Execute()
        {
            if (TodoList == null)
            {
                return;
            }

            TodoItem? item = TodoList.GetItem(Index);
            if (item == null)
            {
                Console.WriteLine($"Задача №{Index + 1} не найдена.");
                return;
            }

            item.UpdateText(Text);
            Console.WriteLine("Задача обновлена.");
        }
    }
}