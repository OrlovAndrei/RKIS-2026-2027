using System;

namespace TodoListApp
{
    class TodoList
    {
        private const int InitialCapacity = 2;
        private const int GrowthFactor = 2;

        private TodoItem[] _items = new TodoItem[InitialCapacity];
        private int _count = 0;

        public void Add(TodoItem item)
        {
            if (_count >= _items.Length)
            {
                IncreaseArray(_items, item);
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
                Console.WriteLine("Задача не найдена.");
                return;
            }

            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }

            _count--;

            Console.WriteLine("Задача удалена!");
        }

        public TodoItem? GetItem(int index)
        {
            if (index < 0 || index >= _count)
            {
                return null;
            }

            return _items[index];
        }

        private void IncreaseArray(TodoItem[] items, TodoItem item)
        {
            Array.Resize(ref items, items.Length * GrowthFactor);

            _items = items;
            _items[_count] = item;
            _count++;
        }

        public void View(bool showIndex, bool showDone, bool showDate)
        {
            if (_count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            if (showIndex)
                Console.Write($"{"Индекс",-8}");

            Console.Write($"{"Задача",-35}");

            if (showDone)
                Console.Write($"{"Статус",-15}");

            if (showDate)
                Console.Write($"{"Дата",-20}");

            Console.WriteLine();

            for (int i = 0; i < _count; i++)
            {
                string text = _items[i].Text
                    .Replace("\r", "")
                    .Replace("\n", " ");

                if (text.Length > 30)
                {
                    text = text.Substring(0, 30) + "...";
                }

                if (showIndex)
                    Console.Write($"{i + 1,-8}");

                Console.Write($"{text,-35}");

                if (showDone)
                {
                    string status = _items[i].IsDone
                        ? "Сделано" : "Не сделано";

                    Console.Write($"{status,-15}");
                }

                if (showDate)
                    Console.Write($"{_items[i].LastUpdate:dd.MM.yyyy HH:mm}");

                Console.WriteLine();
            }
        }
    }
}