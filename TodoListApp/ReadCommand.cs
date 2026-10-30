// Подключаем функции консольного ввода и вывода.
using System;

// Создаём команду просмотра подробностей задачи.
public class ReadCommand : ICommand
{
    // Храним список задач приложения.
    public TodoList TodoList { get; set; } = null!;

    // Храним номер задачи для просмотра.
    public int Index { get; set; }

    // Выполняем команду просмотра задачи.
    public void Execute()
    {
        // Получаем задачу по указанному номеру.
        TodoItem? item = TodoList.GetItem(Index);

        // Проверяем, существует ли задача.
        if (item == null)
        {
            // Сообщаем, что задача не найдена.
            Console.WriteLine("Задача не найдена.");

            // Прекращаем выполнение команды.
            return;
        }

        // Выводим полную информацию о задаче.
        Console.WriteLine(item.GetFullInfo());
    }
}