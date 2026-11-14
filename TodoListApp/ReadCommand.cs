using System;

namespace TodoListApp
{
    public class ReadCommand : ICommand
    {
        public int Index { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            if (TodoList != null)
            {
                TodoItem item = TodoList.GetItem(Index);
                if (item != null)
                {
                    Console.WriteLine(item.GetFullInfo());
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный индекс.");
                }
            }
        }
    }
}