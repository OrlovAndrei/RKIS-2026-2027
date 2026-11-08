using System;

namespace TodoListApp
{
    public class DoneCommand : ICommand
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
                    item.MarkDone();
                    Console.WriteLine($"Задача {Index} выполнена.");
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный индекс.");
                }
            }
        }
    }
}