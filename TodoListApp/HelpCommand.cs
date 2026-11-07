namespace TodoListApp
{
    internal class HelpCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - вывести список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add \"текст задачи\" - добавить задачу в одну строку");
            Console.WriteLine("add --multiline (или -m) - добавить многострочную задачу, ввод заканчивается строкой !end");
            Console.WriteLine("view - показать задачи в виде таблицы (только текст)");
            Console.WriteLine("view [флаги] - флаги можно комбинировать, например -is:");
            Console.WriteLine("  --index, -i - показать индекс задачи");
            Console.WriteLine("  --status, -s - показать статус задачи");
            Console.WriteLine("  --update-date, -d - показать дату последнего изменения");
            Console.WriteLine("  --all, -a - показать все данные");
            Console.WriteLine("read <номер> - показать полный текст задачи, статус и дату");
            Console.WriteLine("status <номер> <статус> - изменить статус задачи (notstarted, inprogress, completed, postponed, failed)");
            Console.WriteLine("delete <номер> - удалить задачу");
            Console.WriteLine("update <номер> \"новый текст\" - изменить текст задачи");
            Console.WriteLine("exit - завершить программу");
        }
    }
}