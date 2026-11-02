namespace TodoListApp
{
    internal class ReadCommand : ICommand
    {
        public int Index { get; set; }

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

            Console.WriteLine(item.GetFullInfo());
        }
    }
}