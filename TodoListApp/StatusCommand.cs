namespace TodoListApp
{
    internal class StatusCommand : ICommand
    {
        public int Index { get; set; }

        public TodoStatus Status { get; set; }

        public TodoList? TodoList { get; set; }

        public void Execute()
        {
            if (TodoList == null)
            {
                return;
            }

            if (!TodoList.SetStatus(Index, Status))
            {
                Console.WriteLine($"Задача №{Index + 1} не найдена.");
                return;
            }

            FileManager.SaveTodos(TodoList, FileManager.TodoFilePath);
            Console.WriteLine($"Статус задачи №{Index + 1} изменён.");
        }
    }
}