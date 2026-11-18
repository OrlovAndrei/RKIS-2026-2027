using System;

namespace TodoListApp
{
    internal class TodoList
    {
        private TodoItem[] _items;
        private int _count;

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
            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }
            _items[_count - 1] = null;
            _count--;
        }

        public void View(bool showIndex, bool showStatus, bool showDate)
        {
            if (_count == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            if (showIndex) Console.Write($" {"ID",-4} ");
            if (showStatus) Console.Write($" {"Статус",-12} ");
            if (showDate) Console.Write($" {"Дата изменения",-16} ");
            Console.WriteLine($" {"Текст задачи",-30} ");

            for (int i = 0; i < _count;  i++)
            {
                if (showIndex) Console.Write($" {i + 1,-4} ");

                if (showStatus)
                {
                    string st = _items[i].IsDone ? "Выполнена" : "Не выполнена";
                    Console.Write($" {st,-12} ");
                }

                if (showDate)
                {
                    Console.Write($" {_items[i].LastUpdate,-16:dd.MM.yyyy HH:mm} ");
                }

                string shortInfo = _items[i].GetShortInfo();
                Console.WriteLine($" {shortInfo,-30} ");
            }
        }

        public TodoItem GetItem( int index )
        {
            if (index < 0 || index >= _count)
            {
                Console.WriteLine("Задачи с таким номером нет");
                return null;
            }
            return _items[index];
        }

        private void IncreaseArray()
        {
            TodoItem[] newItems = new TodoItem[_items.Length * 2];
            for (int i = 0; i < _items.Length; i++)
            {
                newItems[i] = _items[i];
            }

            _items = newItems;
            Console.WriteLine("Массив расширен");
        }
    }
}
