using System;

namespace TodoListApp
{
    // Пункт 3: Класс новой команды для изменения статуса задачи
    public class StatusCommand : ICommand
    {
        private readonly TodoList _todoList;
        private readonly string _input;
         // Поля для запоминания состояния до изменения (Пункт 6)
        private int _targetIndex = -1;
        private TodoStatus _oldStatus;
        private bool _isExecutedSuccessfully = false;
        public StatusCommand(TodoList todoList, string input)
        {
            _todoList = todoList;
            _input = input;
        }

        public void Execute()
        {
            // Сюда мы точечно добавим логику разбора строки в следующем шаге
             // Убираем имя команды "status " из строки (строка 7 символов)
            string args = _input.Substring(7).Trim();
            string[] parts = args.Split(' ');

            if (parts.Length == 2 && int.TryParse(parts[0], out int index))
            {
                // Преобразуем строковое имя в объект enum TodoStatus (Пункт 3, совет из методички)
                if (Enum.TryParse(parts[1], true, out TodoStatus newStatus))
                {
                    TodoItem? item = _todoList[index]; // Наш новый индексатор в деле!
                    if (item != null)
                    {
                        // Пункт 6: Запоминаем старый статус перед изменением
                        _targetIndex = index;
                        _oldStatus = item.Status;
                        _isExecutedSuccessfully = true;

                        item.SetStatus(newStatus);
                        Console.WriteLine($"Статус задачи [{index}] успешно изменен на: {newStatus}\n");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: задача с таким индексом не найдена.\n");
                    }         
                }
                else
                {
                    Console.WriteLine("Ошибка: указан неизвестный статус. Допустимые: NotStarted, InProgress, Completed, Postponed, Failed.\n");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат 'status <индекс> <статус>'.\n");
            }
        }
                // Пункт 6: Метод для отмены смены статуса задачи
        public void Unexecute()
        {
            if (_isExecutedSuccessfully && _targetIndex >= 0)
            {
                TodoItem? item = _todoList[_targetIndex];
                if (item != null)
                {
                    item.SetStatus(_oldStatus);
                    // Сохраняем изменения на диск в CSV (Пункт 6)
                    FileManager.SaveTodos(_todoList, System.IO.Path.Combine("data", $"todos_{AppInfo.CurrentProfileId}.csv"));
                    Console.WriteLine($"Действие 'status' успешно отменено. Возвращен статус: {_oldStatus}\n");
                }
            }
        }

    }
}