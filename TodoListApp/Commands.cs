using System;

namespace TodoList
{
    // 1. Команда добавления
    public class AddCommand : ICommand
    {
        public string Text { get; set; } = "";
        public bool Multiline { get; set; } = false;
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            if (Multiline)
            {
                Console.WriteLine("Введите текст (для завершения введите пустую строку):");
                string fullText = "";
                while (true)
                {
                    string line = Console.ReadLine();
                    if (string.IsNullOrEmpty(line)) break;
                    fullText += line + "\n";
                }
                Text = fullText.Trim();
            }

            if (!string.IsNullOrEmpty(Text))
            {
                TodoList.Add(new TodoItem(Text));
                Console.WriteLine("Задача добавлена.");
            }
            else
            {
                Console.WriteLine("Ошибка: текст задачи пуст.");
            }
        }
    }

    // 2. Команда просмотра
    public class ViewCommand : ICommand
    {
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            // По заданию выводим с индексами, статусами и датами
            TodoList.View(true, true, true);
        }
    }

    // 3. Команда отметки "выполнено"
    public class DoneCommand : ICommand
    {
        public int Index { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            var item = TodoList.GetItem(Index);
            if (item != null)
            {
                item.MarkDone();
                Console.WriteLine("Задача выполнена.");
            }
        }
    }

    // 4. Команда обновления текста
    public class UpdateCommand : ICommand
    {
        public int Index { get; set; }
        public string Text { get; set; } = "";
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            var item = TodoList.GetItem(Index);
            if (item != null)
            {
                item.UpdateText(Text);
                Console.WriteLine("Текст обновлен.");
            }
        }
    }

    // 5. Команда удаления
    public class DeleteCommand : ICommand
    {
        public int Index { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            TodoList.Delete(Index);
            Console.WriteLine("Задача удалена.");
        }
    }

    // 6. Команда чтения подробной информации
    public class ReadCommand : ICommand
    {
        public int Index { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            var item = TodoList.GetItem(Index);
            if (item != null)
            {
                Console.WriteLine(item.GetFullInfo());
            }
        }
    }

    // 7. Команда профиля
    public class ProfileCommand : ICommand
    {
        public Profile Profile { get; set; }

        public void Execute()
        {
            Console.WriteLine(Profile.GetInfo());
        }
    }

    // 8. Команда выхода
    public class ExitCommand : ICommand
    {
        public void Execute()
        {
            Environment.Exit(0);
        }
    }
}