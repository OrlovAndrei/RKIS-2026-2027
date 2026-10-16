namespace TodoListApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Работу выполнили Киснер и Маркелов");

        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine();
        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine();
        Console.Write("Введите год рождения: ");
        int birthYear = int.Parse(Console.ReadLine());

        int age = DateTime.Now.Year - birthYear;

        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

        string[] todos = new string[2];
        int count = 0;

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            string[] parts = input.Split(' ', 2);
            string command = parts[0];

            if (command == "help")
            {
                Console.WriteLine("Доступные команды:");
                Console.WriteLine("  help    - список команд");
                Console.WriteLine("  profile - данные пользователя");
                Console.WriteLine("  add \"текст задачи\" - добавить задачу");
                Console.WriteLine("  view    - показать все задачи");
                Console.WriteLine("  exit    - выход из программы");
            }
            else if (command == "profile")
            {
                Console.WriteLine($"{firstName} {lastName}, {birthYear}");
            }
            else if (command == "add")
            {
                if (parts.Length < 2)
                {
                    Console.WriteLine("Укажите текст задачи: add \"текст задачи\"");
                    continue;
                }

                if (count == todos.Length)
                {
                    string[] newTodos = new string[todos.Length * 2];
                    for (int i = 0; i < todos.Length; i++)
                    {
                        newTodos[i] = todos[i];
                    }
                    todos = newTodos;
                }

                string text = parts[1].Trim('"');
                todos[count] = text;
                count++;
                Console.WriteLine($"Задача добавлена: {text}");
            }
            else if (command == "view")
            {
                Console.WriteLine("Задачи:");
                foreach (string todo in todos)
                {
                    if (!string.IsNullOrEmpty(todo))
                    {
                        Console.WriteLine($"  {todo}");
                    }
                }
            }
            else if (command == "exit")
            {
                break;
            }
            else
            {
                Console.WriteLine("Неизвестная команда. Введите help, чтобы увидеть список команд");
            }
        }
    }
}
