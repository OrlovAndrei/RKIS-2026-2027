Console.WriteLine("Application developed by: Nozdrin VV");
Console.WriteLine("=============================================");


Console.Write("Enter your name: ");
string name = Console.ReadLine() ?? "";

Console.Write("Enter your surname: ");
string surname = Console.ReadLine() ?? "";

Console.Write("Enter your birth year: ");
string birthYearInput = Console.ReadLine() ?? "0";


// converting birth year to int and calculating age

int birthYear = Convert.ToInt32(birthYearInput);
int currentYear = DateTime.Now.Year;
int age = currentYear - birthYear;


Console.WriteLine("---------------------------------------------");
Console.WriteLine($"User added: {name} {surname}, age - {age}");

Console.WriteLine("Welcome to TodoListApp! Type 'help' to see available commands.");
Console.WriteLine("---------------------------------------------");

string[] todos = new string[2]; // count of tasks
int taskCount = 0; // number of added tasks

while (true) // loop for checking command
{
    Console.Write("Enter command: ");
    string command = Console.ReadLine()?.Trim() ?? "";

    if (string.IsNullOrWhiteSpace(command)) continue;


    if (command.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Goodbye!");
        break;
    }
    else if (command.Equals("help", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Available commands: ");
        Console.WriteLine("help - Show all available commands with description");
        Console.WriteLine("profile - Display user profile details");
        Console.WriteLine("add - Add a new task");
        Console.WriteLine("view - View all active tasks");
        Console.WriteLine("exit - Exit the application");
    }
    else if (command.Equals("profile", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine($"User Profile: {name} {surname}, Born: {birthYear} (Age: {age})");
    }
    else if (command.StartsWith("add", StringComparison.OrdinalIgnoreCase))
    {
        // Split input into 2 parts 
        string[] parts = command.Split(new[] { ' ' }, 2);

        if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
        {
            Console.WriteLine("Error: Task description cannot be empty. Use: add \"task\"");
            continue;
        }

        string taskText = parts[1].Trim('"'); // remove quotes from string

        // dynamic array logic

        if (taskCount >= todos.Length)
        {
            Console.WriteLine($"[System] Array full ({todos.Length} items). Resizing x2...");

            string[] newTodos = new string[todos.Length * 2];

            for (int i = 0; i < todos.Length; i++)
            {
                newTodos[i] = todos[i];
            }
            todos = newTodos;
        }
        todos[taskCount] = taskText;
        taskCount++;
        Console.WriteLine($"Task added: {taskText}");
    }
    else if (command.Equals("view", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Active Tasks");
        bool hasTasks = false;

        for (int i = 0; i < todos.Length; i++)
        {
            if (!string.IsNullOrEmpty(todos[i]))
            {
                Console.WriteLine($"{i + 1}. {todos[i]}");
                hasTasks = true;
            }
        }
        if (!hasTasks) Console.WriteLine("No tasks found");
    }
    else
    {
        Console.WriteLine("Command not found or not development. Type another command");
    }


}