using System; // Подключаем вывод информации в консоль.

public class TodoList // Класс для управления списком задач.
{
    private const int InitialCapacity = 2; // Начальный размер массива.
    private const int GrowthFactor = 2; // Во сколько раз увеличивается массив.
    private const int TextWidth = 33; // Ширина столбца текста с учётом многоточия.
    private const int IndexWidth = 5; // Ширина столбца номеров.
    private const int StatusWidth = 12; // Ширина столбца статусов.
    private const int DateWidth = 16; // Ширина столбца даты.

    private TodoItem[] items; // Массив объектов задач.
    private int count; // Количество добавленных задач.
    
    // Возвращает количество задач в списке.
    public int Count => count;

    public TodoList() // Конструктор создаёт пустой список.
    {
        items = new TodoItem[InitialCapacity]; // Выделяем память для двух задач.
        count = 0; // Изначально задач нет.
    }

    public void Add(TodoItem item) // Метод добавляет задачу в список.
    {
        if (count == items.Length) // Проверяем, заполнен ли массив.
        {
            items = IncreaseArray(items, item); // Увеличиваем массив и добавляем задачу.
        }
        else // Если свободное место ещё есть.
        {
            items[count] = item; // Записываем задачу в свободную ячейку.
        }

        count++; // Увеличиваем количество задач.
        Console.WriteLine("Задача добавлена"); // Сообщаем об успешном добавлении.
    }

    public void Delete(int index) // Метод удаляет задачу по номеру.
    {
        if (index < 1 || index > count) // Проверяем правильность номера.
        {
            Console.WriteLine("Ошибка. Неверный номер задачи."); // Выводим сообщение об ошибке.
            return; // Завершаем метод.
        }

        int position = index - 1; // Переводим номер пользователя в индекс массива.

        for (int i = position; i < count - 1; i++) // Перебираем элементы после удаляемого.
        {
            items[i] = items[i + 1]; // Сдвигаем следующую задачу на одну позицию влево.
        }

        count--; // Уменьшаем количество задач.
        items[count] = null!; // Очищаем последнюю занятую ячейку массива.

        Console.WriteLine("Задача удалена"); // Сообщаем об удалении.
    }

    public TodoItem? GetItem(int index) // Метод возвращает задачу по номеру.
    {
        if (index < 1 || index > count) // Проверяем, существует ли такой номер.
        {
            Console.WriteLine("Ошибка. Неверный номер задачи."); // Сообщаем о неправильном номере.
            return null; // Возвращаем отсутствие задачи.
        }

        return items[index - 1]; // Возвращаем нужный объект задачи.
    }

    public void View(bool showIndex, bool showDone, bool showDate) // Метод выводит таблицу задач.
    {
        if (count == 0) // Проверяем, есть ли задачи в списке.
        {
            Console.WriteLine("Список задач пуст"); // Сообщаем, что список пуст.
            return; // Завершаем вывод.
        }

        string BuildRow(string index, string text, string status, string date) // Формирует строку таблицы.
        {
            string row = ""; // Создаём пустую строку.

            if (showIndex) // Если нужно показать номера.
            {
                row += index.PadRight(IndexWidth) + " | "; // Добавляем номер и разделитель.
            }

            row += text.PadRight(TextWidth); // Добавляем текст задачи.

            if (showDone) // Если нужно показать статус.
            {
                row += " | " + status.PadRight(StatusWidth); // Добавляем статус и разделитель.
            }

            if (showDate) // Если нужно показать дату.
            {
                row += " | " + date.PadRight(DateWidth); // Добавляем дату изменения.
            }

            return row; // Возвращаем готовую строку таблицы.
        }

        string header = BuildRow("Index", "Text", "Status", "Updated"); // Формируем заголовок таблицы.
        Console.WriteLine(header); // Выводим заголовок.
        Console.WriteLine(new string('-', header.Length)); // Подчёркиваем заголовок.

        for (int i = 0; i < count; i++) // Перебираем все задачи.
        {
            TodoItem item = items[i]; // Получаем текущую задачу.

            string shortText = item.Text.Replace("\n", " "); // Убираем переносы строк из текста.

            if (shortText.Length > 30) // Проверяем длину текста.
            {
                shortText = shortText.Substring(0, 30) + "..."; // Сокращаем текст до 30 символов.
            }

            string status = item.IsDone ? "сделано" : "не сделано"; // Определяем статус задачи.
            string date = item.LastUpdate.ToString("dd.MM.yyyy HH:mm"); // Форматируем дату изменения.

            Console.WriteLine(BuildRow( // Формируем и выводим строку задачи.
                (i + 1).ToString(), // Показываем номер начиная с единицы.
                shortText, // Передаём сокращённый текст.
                status, // Передаём статус.
                date)); // Передаём дату изменения.
        }
    }

    private TodoItem[] IncreaseArray(TodoItem[] items, TodoItem item) // Увеличивает массив и добавляет задачу.
    {
        int newSize = items.Length * GrowthFactor; // Рассчитываем новый размер массива.

        TodoItem[] newItems = new TodoItem[newSize]; // Создаём массив увеличенного размера.

        for (int i = 0; i < items.Length; i++) // Перебираем старый массив.
        {
            newItems[i] = items[i]; // Копируем задачи в новый массив.
        }

        newItems[count] = item; // Добавляем новую задачу в свободную ячейку.

        Console.WriteLine($"Массив расширен до {newSize} элементов"); // Сообщаем об увеличении массива.

        return newItems; // Возвращаем новый массив.
    }
}