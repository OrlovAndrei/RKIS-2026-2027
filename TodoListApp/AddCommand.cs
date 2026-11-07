namespace TodoListApp
{
    internal class AddCommand : ICommand
    {
        private const string MultilinePrompt = "> ";
        private const string MultilineEndMarker = "!end";
        private const string MultilineJoiner = "\n";

        public bool IsMultiline { get; set; }

        public string Text { get; set; } = "";

        public TodoList? TodoList { get; set; }

        public void Execute()
        {
            if (TodoList == null)
            {
                return;
            }

            string text = IsMultiline ? ReadMultilineText() : Text;
            if (text == "")
            {
                Console.WriteLine("Задача не добавлена: текст пустой.");
                return;
            }

            TodoList.Add(new TodoItem(text));
            FileManager.SaveTodos(TodoList, FileManager.TodoFilePath);
            Console.WriteLine("Задача добавлена.");
        }

        private static string ReadMultilineText()
        {
            Console.WriteLine($"Вводите строки задачи, для завершения введите {MultilineEndMarker}");

            List<string> lines = new List<string>();
            while (true)
            {
                Console.Write(MultilinePrompt);
                string? line = Console.ReadLine();
                if (line == null || line == MultilineEndMarker)
                {
                    break;
                }

                lines.Add(line);
            }

            return string.Join(MultilineJoiner, lines);
        }
    }
}