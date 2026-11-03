namespace TodoListApp
{
    internal class DoneCommand : ICommand
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

            item.MarkDone();
            FileManager.SaveTodos(TodoList, FileManager.TodoFilePath);
            Console.WriteLine("Задача отмечена выполненной.");
        }
    }
}