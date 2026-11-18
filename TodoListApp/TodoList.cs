using System;

namespace TodoListApp
{
    public class TodoList
    {
        private const int InitialSize = 2;
        private TodoItem[] _items;
        private int _count;

        public TodoList()
        {
            _items = new TodoItem[InitialSize];
            _count = 0;
        }

        public void Add(TodoItem item)
        {
            if (_count == _items.Length)
            {
                IncreaseArray();
            }

            _items[_count] = item;
            _count++;
        }

        public bool Delete(int index)
        {
            if (index < 0 || index >= _count)
            {
                return false;
            }

            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }

            _items[_count - 1] = null!;
            _count--;
            return true;
        }

        public TodoItem? GetItem(int index)
        {
            if (index < 0 || index >= _count)
            {
                return null;
            }

            return _items[index];
        }

        public void View(bool showIndex, bool showDone, bool showDate)
        {
            if (_count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            string header = "";
            if (showIndex) header += $"{"Индекс",-8} | ";
            if (showDone) header += $"{"Статус",-14} | ";
            if (showDate) header += $"{"Дата изменения",-18} | ";
            header += $"{"Задачи",-34}";

            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            for (int i = 0; i < _count; i++)
            {
                TodoItem current = _items[i];
                string row = "";

                if (showIndex)
                    row += $"{i + 1,-8} | ";

                if (showDone)
                    row += $"{(current.IsDone ? "выполнена" : "не выполнена"),-14} | ";

                if (showDate)
                    row += $"{current.LastUpdate,-18:dd.MM.yyyy HH:mm} | ";

                string taskText = current.Text.Replace("\n", " ");
                if (taskText.Length > 30)
                {
                    taskText = taskText.Substring(0, 30) + "...";
                }

                row += $"{taskText,-34}";
                Console.WriteLine(row);
            }
        }

        private void IncreaseArray()
        {
            int newSize = _items.Length * 2;
            TodoItem[] newItems = new TodoItem[newSize];

            for (int i = 0; i < _items.Length; i++)
            {
                newItems[i] = _items[i];
            }

            _items = newItems;
        }
    }
}
