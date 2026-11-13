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

        // Приватный метод для увеличения массива
        private TodoItem[] IncreaseArray(TodoItem[] items)
        {
            TodoItem[] newItems = new TodoItem[items.Length * 2];
            for (int i = 0; i < items.Length; i++)
            {
                newItems[i] = items[i];
            }
            return newItems;
        }

        public void Add(TodoItem item)
        {
            if (_count >= _items.Length)
            {
                _items = IncreaseArray(_items); // Вызов приватного метода
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
            _items[_count - 1] = null;
            _count--;
        }

        public TodoItem GetItem(int index)
        {
            if (index < 0 || index >= _count)
            {
                Console.WriteLine("Ошибка: Задачи с таким индексом не существует.");
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

            for (int i = 0; i < _count; i++)
            {
                string output = "";
                if (showIndex) output += $"{i}. ";
                
                string shortText = _items[i].Text.Length > 30 
                    ? _items[i].Text.Substring(0, 30) + "..." 
                    : _items[i].Text;

                output += $"{shortText,-35}";

                if (showDone) output += $" | {(_items[i].IsDone ? "[V]" : "[ ]")}";
                if (showDate) output += $" | {_items[i].LastUpdate:dd.MM.yyyy HH:mm}";

                Console.WriteLine(output);
            }
        }
    }
}