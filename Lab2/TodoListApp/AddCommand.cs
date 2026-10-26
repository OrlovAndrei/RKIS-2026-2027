using System;

namespace TodoListApp
{
    // Пункт 3: Класс конкретной команды для добавления задачи
    public class AddCommand : ICommand
    {
       // Оформляем свойства и зависимости по требованиям методички (Пункт 3)
        private readonly TodoList _todoList;
        private readonly string _input;
        // Поле для запоминания созданной задачи (Пункт 4)
        private TodoItem? _addedItem;
        public AddCommand(TodoList todoList, string input)
        {
            _todoList = todoList;
            _input = input;
        }

        public void Execute()
        {
            bool isMultiline = _input.Contains("--multiline") || _input.Contains(" -m");
            string taskText = "";

            if (isMultiline)
            {
                Console.WriteLine("Введите строки задачи (для окончания введите !end):");
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                while (true)
                {
                    Console.Write("> ");
                    string? line = Console.ReadLine();
                    if (line == "!end") break;
                    if (line != null) sb.AppendLine(line);
                }
                taskText = sb.ToString().TrimEnd();
            }
            else
            {
                string[] parts = _input.Split(new[] { ' ' }, 2);
                if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
                {
                    Console.WriteLine("Ошибка: введите текст задачи после 'add'.\n");
                    return;
                }
                taskText = parts[1];
            }
            // Создаем задачу, запоминаем её в поле класса и добавляем в список (Пункт 4)
            _addedItem = new TodoItem(taskText);
            _todoList.Add(_addedItem);
            // Пункт 4: Автоматическое сохранение изменений в файл
            FileManager.SaveTodos(_todoList, Path.Combine("data", $"todos_{AppInfo.CurrentProfileId}.csv"));
            Console.WriteLine("Задача успешно добавлена!\n");
        }
    // Пункт 4: Реализация отмены для отменяемой команды AddCommand
        public void Unexecute()
        {
            if (_addedItem != null)
            {
                // Ищем индекс нашей запомненной задачи в списке
                int index = -1;
                for (int i = 0; i < _todoList.Count; i++)
                {
                    if (_todoList[i] == _addedItem)
                    {
                        index = i;
                        break;
                    }
                }

                // Если задача найдена — удаляем её через встроенный метод
                if (index != _todoList.Count - 1 && index >= 0)
                {
                    _todoList.Delete(index);
                    
                    // Перезаписываем CSV-файл, чтобы изменения сохранились на диске
                    FileManager.SaveTodos(_todoList, Path.Combine("data", $"todos_{AppInfo.CurrentProfileId}.csv"));
                    Console.WriteLine("Действие 'add' успешно отменено.");
                }
            }
        }
    }
}
