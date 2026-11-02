namespace TodoListApp
{
    internal class DeleteCommand : ICommand
    {
        public int Index { get; set; }

        public TodoList? TodoList { get; set; }

        public void Execute()
        {
            if (TodoList == null)
            {
                return;
            }

            if (!TodoList.Delete(Index))
            {
                Console.WriteLine($"Задача №{Index + 1} не найдена.");
                return;
            }

            Console.WriteLine("Задача удалена.");
        }
    }
}