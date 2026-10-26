using System;
using System.Collections.Generic;
namespace TodoListApp
{
    // Пункт 3: Класс-хранилище для управления списком задач
    public class TodoList
    {
        private const int INITIAL_CAPACITY = 2;
        private const int CAPACITY_MULTIPLIER = 2;

        // Приватное поле: внутренний массив объектов TodoItem (Пункт 3, дефис 1)
         // Пункт 1: Задачи теперь хранятся в списке, а не в массиве
        private readonly List<TodoItem> _items = new List<TodoItem>();
        public TodoList()
        {
            // Конструктор оставляем пустым, список инициализируется сам
        }
        public event Action<TodoItem?>? OnTodoAdded;
        public event Action<TodoItem?>? OnTodoDeleted;
        public event Action<TodoItem?>? OnTodoUpdated;
        public event Action<TodoItem?>? OnStatusChanged;
// Методы класса (Пункт 3, дефис 2)
       public void Add(TodoItem item)
        {
            // Просто добавляем элемент во встроенный список
            _items.Add(item);
            OnTodoAdded?.Invoke(item);
        }
    
        public void Delete(int index)
        {
            if (index >= 0 && index < _items.Count)
            {
            var deletedItem = _items[index];
            _items.RemoveAt(index);
            Console.WriteLine($"Задача [{index}] успешно удалена.\n");
            OnTodoDeleted?.Invoke(deletedItem);
            _items.RemoveAt(index);
                Console.WriteLine($"Задача [{index}] успешно удалена.\n");
            }
            else
            {
                Console.WriteLine("Ошибка: некорректный индекс задачи.\n");
            }
        }
                public void View(bool showIndex, bool showStatus, bool showDate, string rawInput)
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("(список задач пуст)\n");
                return;
            }

            for (int i = 0; i < _items.Count; i++)
            {
                string info = _items[i].GetShortInfo();
                string row = _items[i].Text;

                // Обрезка текста до 30 символов для таблицы
                string textClean = row.Replace(Environment.NewLine, " ");
                if (textClean.Length > 30)
                {
                    textClean = textClean.Substring(0, 30) + "...";
                }

                row = textClean;

                // Форматируем вывод в зависимости от флагов
                if (showIndex) row = $"[{i}] " + row;
                if (showStatus) row += " | " + _items[i].Status.ToString();
                if (showDate) row += $" | {_items[i].LastUpdate:dd.MM.yyyy HH:mm:ss}";

                Console.WriteLine(row);
            }
            Console.WriteLine("--------------------------\n");
        }
        public TodoItem? GetItem(int index)
        {
            if (index >= 0 && index < _items.Count)
            {
                return _items[index];
            }
            return null;
        }
        // Пункт 1: Реализуйте индексатор для доступа к задачам по индексу
        public TodoItem? this[int index]
        {
            get
            {
                if (index >= 0 && index < _items.Count)
                {
                    return _items[index];
                }
                return null;
            }
        }
        // Публичный доступ к количеству элементов в списке
public int Count => _items.Count;
        // Пункт 1: Реализуйте метод-итератор с использованием yield return
        public System.Collections.IEnumerator GetEnumerator()
        {
            foreach (var item in _items)
            {
                yield return item;
            }
        }
    }
}
