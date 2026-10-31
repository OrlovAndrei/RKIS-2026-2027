using System;

namespace TodoListApp
{
    public class TodoList
    {
        private TodoItem[] _items;
        private int _count;
        private const int InitialSize = 2;

        public TodoList()
        {
            _items = new TodoItem[InitialSize];
            _count = 0;
        }

        public void Add(TodoItem item)
        {
            if (_count >= _items.Length)
            {
                IncreaseArray();
            }
            else
            {
                _items[_count] = item;
                _count++;
            }
        }

        public void Delete(int index)
        {
            if (index < 0 || index >= _count)
            {
                Console.WriteLine("Ошибка: неверный индекс.");
                return;
            }

            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }
            
            _items[_count - 1] = null;
            _count--;
            Console.WriteLine($"Задача {index} удалена.");
        }

        public TodoItem GetItem(int index)
        {
            if (index < 0 || index >= _count)
            {
                return null;
            }
            return _items[index];
        }

        public void View(bool showIndex, bool showDone, bool showDate)
        {
            Console.WriteLine("Ваши задачи:");
            
            if (showIndex) Console.Write($"{"Idx",-4} | ");
            Console.Write($"{"Задача",-32} | ");
            if (showDone) Console.Write($"{"Статус",-12} | ");
            if (showDate) Console.Write($"{"Дата",-10} | ");
            Console.WriteLine();
            Console.WriteLine(new string('-', 75));

            if (_count == 0)
            {
                Console.WriteLine("  (Список пуст)");
                return;
            }

            for (int i = 0; i < _count; i++)
            {
                TodoItem item = _items[i];
                
                if (showIndex) Console.Write($"{i,-4} | ");

                string shortText = item.Text.Replace("\r", " ").Replace("\n", " ");
                if (shortText.Length > 30) shortText = shortText.Substring(0, 27) + "...";
                
                Console.Write($"{shortText,-32} | ");
                
                if (showDone) 
                {
                    string status = item.IsDone ? "сделано" : "не сделано";
                    Console.Write($"{status,-12} | ");
                }
                if (showDate) 
                {
                    Console.Write($"{item.LastUpdate:dd.MM.yyyy,-10} | ");
                }
                Console.WriteLine();
            }
        }

        private void IncreaseArray()
        {
            TodoItem[] newItems = new TodoItem[_items.Length * 2];
            for (int i = 0; i < _items.Length; i++)
            {
                newItems[i] = _items[i];
            }
            
            _items = newItems;
        }
    }
}