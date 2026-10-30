// Подключаем функции консольного ввода и вывода.
using System;

// Создаём команду изменения задачи.
public class UpdateCommand : ICommand
{
    // Храним список задач приложения.
    public TodoList TodoList { get; set; } = null!;

    // Храним номер изменяемой задачи.
    public int Index { get; set; }

    // Храним новый текст задачи.
    public string Text { get; set; } = "";

    // Выполняем команду изменения задачи.
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

        // Проверяем, что новый текст не пустой.
        if (Text.Trim() == "")
        {
            // Сообщаем об ошибке ввода.
            Console.WriteLine("Текст задачи не может быть пустым.");

            // Прекращаем выполнение команды.
            return;
        }

        // Изменяем текст существующей задачи.
        item.UpdateText(Text);

        // Сообщаем об успешном изменении.
        Console.WriteLine("Задача изменена.");
    }
}