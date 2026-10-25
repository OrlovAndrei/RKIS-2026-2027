using System;

namespace TodoListApp
{
    public class TodoList
    {
        private TodoItem[] _items;
        private int _count;

        public int Count => _count;

        public TodoList(int initialCapacity = 2)
        {
            _items = new TodoItem[initialCapacity];
            _count = 0;
        }

        public void Add(TodoItem item)
        {
            if (_count >= _items.Length)
            {
                IncreaseArray();
            }
            _items[_count] = item;
            _count++;
        }

        public void Delete(int index)
        {
            if (index < 0  index >= _count) return;

            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }

            _items[_count - 1] = null;
            _count--;
        }

        public TodoItem GetItem(int index)
        {
            if (index < 0  index >= _count) return null;
            return _items[index];
        }

        public void View(bool showIndex, bool showStatus, bool showDate)
        {
            if (_count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            Console.WriteLine("\n--- Список задач ---");
            for (int i = 0; i < _count; i++)
            {
                if (_items[i] == null) continue;

                string line = "";
                if (showIndex) line += $"[{i + 1}] ";

                // Получаем базовую информацию и форматируем под флаги
                string shortInfo = _items[i].GetShortInfo();
                // Для упрощения выведем целиком краткую инфо или соберем по флагам
                Console.WriteLine((showIndex ? $"[{i + 1}] " : "") + shortInfo);
            }
            Console.WriteLine("--------------------");
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