using System;

namespace TodoListApp
{
    // Пункт 2 задания 9: Расширение статусов через перечисление enum
    public enum TodoStatus
    {
        NotStarted,   // не начато
        InProgress,   // в процессе
        Completed,    // выполнено
        Postponed,    // отложено
        Failed        // провалено
    }
    // Пункт 2: Класс, описывающий одну задачу
    public class TodoItem
    {
        // Свойства задачи (Пункт 2, дефис 1)
        public string Text { get; set; }
        public TodoStatus Status { get; set; }
        public DateTime LastUpdate { get; set; }

        // Конструктор (Пункт 2, дефис 2)
        public TodoItem(string text)
        {
            Text = text;
            Status = TodoStatus.NotStarted;
            LastUpdate = DateTime.Now;
        }
// Методы класса (Пункт 2, дефис 3)
             // Обновление статуса задачи (Пункт 4, дефис 1)
     public void SetStatus(TodoStatus newStatus)
     {
         Status = newStatus;
         LastUpdate = DateTime.Now;
     }


        public void UpdateText(string newText)
        {
            Text = newText;
            LastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string shortText = Text.Replace(Environment.NewLine, " ");
            if (shortText.Length > 30)
            {
                shortText = shortText.Substring(0, 30) + "...";
            }
            string status = Status.ToString();
            return $"{shortText} | {status} | {LastUpdate:dd.MM.yyyy HH:mm:ss}";
        }

        public string GetFullInfo()
        {
            string status = Status.ToString();
            return $"\nПолный текст задачи:\n{Text}\nСтатус: {status}\nДата последнего изменения: {LastUpdate:dd.MM.yyyy HH:mm:ss}\n";
        }
    }
}