using System;

namespace TodoList
{
    // 1. Команда добавления
    public class AddCommand : ICommand
    {
        public string Text { get; set; } = "";
        public bool Multiline { get; set; } = false;
        public TodoList TodoList { get; set; } = null!;

        public void Execute()
        {
            if (Multiline)
            {
                Console.WriteLine("Введите текст (для завершения введите пустую строку):");
                string fullText = "";
                while (true)
                {
                    string? line = Console.ReadLine();
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
        public TodoList TodoList { get; set; } = null!;

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
        public TodoList TodoList { get; set; } = null!;

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
        public TodoList TodoList { get; set; } = null!;

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
        public TodoList TodoList { get; set; } = null!;

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
        public TodoList TodoList { get; set; } = null!;

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
        public Profile Profile { get; set; } = null!;

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
    // 9. Команда помощи (вывод красивого списка команд)
    public class HelpCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("  help - список команд");
            Console.WriteLine("  profile - данные пользователя");
            Console.WriteLine("  add \"текст\" - добавить задачу");
            Console.WriteLine("  add -m , --multiline - добавить задачу в несколько строк");
            Console.WriteLine("  view [флаги] - показать задачи в виде таблицы");
            Console.WriteLine("         -i, --index       показать индексы");
            Console.WriteLine("         -s, --status      показать статусы");
            Console.WriteLine("         -d, --update-date показать даты изменения");
            Console.WriteLine("         -a, --all         показать все данные");
            Console.WriteLine("  read <idx> - прочитать полный текст задачи");
            Console.WriteLine("  done <idx> - отметить задачу выполненной");
            Console.WriteLine("  delete <idx> - удалить задачу");
            Console.WriteLine("  update <idx> \"текст\" - обновить текст задачи");
            Console.WriteLine("  exit - выйти");
        }
    }
}