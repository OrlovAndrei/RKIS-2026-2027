using System;

namespace TodoListApp
{
    public class UpdateCommand : ICommand
    {
        public int Index { get; set; }
        public string NewText { get; set; } = "";
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            if (TodoList != null)
            {
                TodoItem item = TodoList.GetItem(Index);
                if (item != null)
                {
                    item.UpdateText(NewText);
                    Console.WriteLine($"Задача {Index} обновлена.");
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный индекс.");
                }
            }
        }
    }
}