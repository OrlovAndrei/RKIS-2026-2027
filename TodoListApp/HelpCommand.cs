namespace TodoListApp;

public class HelpCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("  add [текст]             - добавить задачу");
        Console.WriteLine("  add --multiline (-m)    - добавить многострочную задачу");
        Console.WriteLine("  view                    - показать задачи");
        Console.WriteLine("  view --index (-i)       - показать номера задач");
        Console.WriteLine("  view --status (-s)      - показать статусы");
        Console.WriteLine("  view --update-date (-d) - показать дату изменения");
        Console.WriteLine("  view --all (-a)         - показать все данные");
        Console.WriteLine("  read <idx>              - прочитать задачу полностью");
        Console.WriteLine("  status <idx> <status>   - изменить статус задачи");
        Console.WriteLine("  Статусы: notstarted, inprogress, completed, postponed, failed");
        Console.WriteLine("  update <idx> \"текст\"    - изменить текст задачи");
        Console.WriteLine("  delete <idx>            - удалить задачу");
        Console.WriteLine("  profile                 - показать профиль");
        Console.WriteLine("  help                    - список команд");
        Console.WriteLine("  exit                    - выход");
    }
}