namespace TodoListApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Работу выполнил Добрынин");

            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine() ?? "";

            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine() ?? "";

            Console.Write("Введите год рождения: ");
            string birthYearText = Console.ReadLine() ?? "";

            int birthYear = int.Parse(birthYearText);
            int age = DateTime.Now.Year - birthYear;

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

            Profile profile = new Profile(firstName, lastName, birthYear);
            TodoList todoList = new TodoList();

            while (true)
            {
                Console.Write("Введите команду: ");
                string? input = Console.ReadLine();
                if (input == null)
                {
                    break;
                }

                ICommand command = CommandParser.Parse(input, todoList, profile);
                command.Execute();

                if (command is ExitCommand)
                {
                    break;
                }
            }
        }
    }
}