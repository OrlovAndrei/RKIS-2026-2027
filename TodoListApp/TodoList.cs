using System;

namespace TodoList
{
    public class TodoList
    {
        // Приватный массив и счетчик
        private TodoItem[] _items;
        private int _count;

        public TodoList()
        {
            _items = new TodoItem[2]; // Начальный размер для теста IncreaseArray
            _count = 0;
        }
        public int Count => _count; // свойство для получения количества задач
        
        public void Add(TodoItem item)
        {
            if (_count >= _items.Length)
            {
                _items = IncreaseArray(_items, item);
            }
            _items[_count] = item;
            _count++;
        }

        public void Delete(int index)
        {
            if (index < 0 || index >= _count)
            {
                Console.WriteLine("Ошибка: Неверный индекс задачи.");
                return;
            }
            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }
            _items[_count - 1] = null!;
            _count--;
        }

        public TodoItem GetItem(int index)
        {
            return _items[index];
        }

        public void View(bool showIndex, bool showDone, bool showDate)
        {
            if (_count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }
            
            // Формируем заголовок таблицы
            string header = "";
            if (showIndex) header += "| " + "№".PadRight(3) + " ";
            header += "| " + "Текст задачи".PadRight(33) + " ";
            if (showDone) header += "| " + "Статус".PadRight(17) + " ";
            if (showDate) header += "| " + "Дата изменения".PadRight(16) + " ";
            header += "|";

            string separator = new string('-', header.Length);
            Console.WriteLine(separator);
            Console.WriteLine(header);
            Console.WriteLine(separator);

            // выводим строки
            for (int i = 0; i < _count; i++)
            {
                string row = "";
                
                if (showIndex) 
                    row += "| " + (i + 1).ToString().PadRight(3) + " ";

                // Заменяем перенос строки на пробел, чтобы не ломать таблицу
                string displayText = _items[i].Text.Replace("\n", " ");
                if (displayText.Length > 30) 
                    displayText = displayText.Substring(0, 30) + "...";
                
                row += "| " + displayText.PadRight(33) + " ";

                if (showDone) 
                {
                    string status = _items[i].IsDone ? "[V] Выполнено" : "[ ] Не выполнено";
                    row += "| " + status.PadRight(17) + " ";
                }
                
                if (showDate) 
                {
                    row += "| " + _items[i].LastUpdate.ToString("dd.MM.yyyy HH:mm").PadRight(16) + " ";
                }
                
                row += "|";
                Console.WriteLine(row);
            }
            Console.WriteLine(separator);
        }
        
        // метод для увеличения массива
        private TodoItem[] IncreaseArray(TodoItem[] items, TodoItem item)
        {
            TodoItem[] newItems = new TodoItem[items.Length * 2];
            for (int i = 0; i < items.Length; i++)
            {
                newItems[i] = items[i];
            }
            Console.WriteLine($"[Массив расширен до {newItems.Length} элементов]");
            return newItems;
        }
        
    }
}