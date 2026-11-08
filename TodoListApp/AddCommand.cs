using System;

namespace TodoListApp
{
    public class AddCommand : ICommand
    {
        public string TaskText { get; set; } = "";
        public bool IsMultiline { get; set; } = false;
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            string text = TaskText;
            
            if (IsMultiline)
            {
                text = ReadMultiline();
            }

            if (!string.IsNullOrEmpty(text) && TodoList != null)
            {
                TodoList.Add(new TodoItem(text));
                Console.WriteLine("Задача добавлена.");
            }
        }

        private string ReadMultiline()
        {
            Console.WriteLine("Введите строки задачи (для завершения введите !end):");
            string result = "";
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line == null || line.Trim() == "!end") break;
                if (result != "") result += "\n";
                result += line;
            }
            return result;
        }
    }
}