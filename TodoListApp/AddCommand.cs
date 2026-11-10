// Подключаем основные функции C#.
using System;

// Подключаем коллекцию для хранения строк задачи.
using System.Collections.Generic;

// Создаём команду добавления задачи.
public class AddCommand : ICommand
{
    // Храним список, в который добавляется задача.
    public TodoList TodoList { get; set; } = null!;

    // Храним текст добавляемой задачи.
    public string Text { get; set; } = "";

    // Определяем, используется ли многострочный ввод.
    public bool Multiline { get; set; }

    // Выполняем команду добавления задачи.
    public void Execute()
    {
        // Проверяем, включён ли многострочный режим.
        if (Multiline)
        {
            // Считываем многострочный текст задачи.
            Text = ReadMultilineText();
        }

        // Проверяем, что текст задачи не пустой.
        if (Text.Trim() == "")
        {
            // Сообщаем пользователю об ошибке.
            Console.WriteLine("Ошибка. Задача пустая.");

            // Прерываем выполнение команды.
            return;
        }

        // Создаём объект новой задачи.
        TodoItem item = new TodoItem(Text);

        // Добавляем задачу в существующий список.
        TodoList.Add(item);
    }

    // Считываем текст задачи из нескольких строк.
    private string ReadMultilineText()
    {
        // Создаём список для введённых строк.
        List<string> lines = new List<string>();

        // Объясняем, как завершить ввод.
        Console.WriteLine("Вводите строки. Для завершения введите !end");

        // Повторяем ввод до получения команды завершения.
        while (true)
        {
            // Выводим приглашение для ввода строки.
            Console.Write("> ");

            // Считываем очередную строку.
            string? line = Console.ReadLine();

            // Проверяем конец ввода.
            if (line == null || line.Trim() == "!end")
            {
                // Завершаем цикл ввода.
                break;
            }

            // Сохраняем введённую строку.
            lines.Add(line);
        }

        // Объединяем строки, сохраняя переносы.
        return string.Join("\n", lines);
    }
}