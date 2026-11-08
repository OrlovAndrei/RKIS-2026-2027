using System;

namespace TodoListApp
{
    public class HelpCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("  help            - Список команд");
            Console.WriteLine("  profile         - Данные пользователя");
            Console.WriteLine("  add \"текст\"      - Добавить задачу");
            Console.WriteLine("  add -m          - Многострочный ввод (!end для завершения)");
            Console.WriteLine("  view            - Показать задачи (текст)");
            Console.WriteLine("  view -a         - Показать всё (индекс, текст, статус, дата)");
            Console.WriteLine("  read <idx>      - Полный текст задачи");
            Console.WriteLine("  done <idx>      - Отметить задачу выполненной");
            Console.WriteLine("  delete <idx>    - Удалить задачу");
            Console.WriteLine("  update <idx> \"т\" - Обновить текст задачи");
            Console.WriteLine("  exit            - Выход");
        }
    }
}