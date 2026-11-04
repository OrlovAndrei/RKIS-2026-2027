using System;
using System.Data;

Console.WriteLine("Работу выполнили Деркач и Остапенко");
Console.WriteLine("Введите вашу фамилию");
String firstName = Console.ReadLine();
Console.WriteLine("Введите ваше имя");
String secondName = Console.ReadLine();
Console.WriteLine("Введите год рождения");
int year = int.Parse(Console.ReadLine());
int number = (int)2026;
int age = number - year;
Console.WriteLine($"{firstName} {secondName} возраст: {age}");

string[] todos = new string[2];
int taskCount = 0;
bool[] statuses = new bool[todos.Length];
DateTime[] dates = new DateTime[todos.Length];
static void ExtendArrays(
    string[] todos,
    bool[] statuses,
    DateTime[] dates,
    out string[] newTodos,
    out bool[] newStatuses,
    out DateTime[] newDates)
{
    int newSize = todos.Length * 2;

    newTodos = new string[newSize];
    newStatuses = new bool[newSize];
    newDates = new DateTime[newSize];

    Array.Copy(todos, newTodos, todos.Length);
    Array.Copy(statuses, newStatuses, statuses.Length);
    Array.Copy(dates, newDates, dates.Length);
}

while (true)
{
    Console.Write("Введите команду: ");
    string input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) continue;

    string[] parts = input.Split(' ', 3);
    string command = parts[0].ToLower();
    
    
    switch (command)
    {
        case "help":

            Console.WriteLine("     help — список команд");
            Console.WriteLine("     profile — данные пользователя");
            Console.WriteLine("     add \"задача\" — добавить задачу");
            Console.WriteLine("     view — показать задачи");
            Console.WriteLine("     exit — выход");
            Console.WriteLine("     done < idx > — отмечает задачу выполненной");
            Console.WriteLine("     update < idx > \"new_text\" обновляет текст задачи");
            break;

        case "add":
            if (parts.Length < 2)
                {
                    Console.WriteLine("Ошибка: укажите текст задачи в кавычках. Формат: add \"текст задачи\"");
                    break;
                }
                string task = input.Substring(4).Trim('"');

            if (taskCount >= todos.Length)
                {

                ExtendArrays(todos, statuses, dates, out todos, out statuses, out dates);
            }

                todos[taskCount] = task;
                statuses[taskCount] = false;
                dates[taskCount] = DateTime.Now;
                taskCount++;
                Console.WriteLine("Задача добавлена.");
                break;

        case "profile":
            Console.WriteLine($"{firstName} {secondName} возраст: {age}");
            break;
        case "done":
            if (parts.Length < 2)
            {
                Console.WriteLine("Индекс не введен");
                 break;
            }
            if (!int.TryParse(parts[1], out int idx))
            {
                Console.WriteLine("Ошибка: номер задачи должен быть числом.");
                break;
            }
            int index = idx - 1;
            statuses[index] = true;
            dates[index] = DateTime.Now;
            break;

        case "view":
            if (taskCount == 0)
            {
                Console.WriteLine("Список задач пуст.");
            }
            else
            {
                for (int i = 0; i < taskCount; i++)
                {
                    Console.WriteLine($"{i + 1}. {todos[i]} {statuses[i]} {dates[i]}");
                }
            }
            break;
        
        case "exit":
            Console.WriteLine("Завершение программы...");
            return;
        case "delete":
            if (parts.Length < 2)
            {
                Console.WriteLine("Индекс не введен");
                break;
            }
            if (!int.TryParse(parts[1], out  int id))
            {
                Console.WriteLine("Ошибка: номер задачи должен быть числом.");
                break;
            }
            
           
            if (!int.TryParse(parts[1], out  id))
            {
                Console.WriteLine("Ошибка: номер задачи должен быть числом.");
                break;
            }
            int indexDelete = id - 1;
            for (int i = indexDelete; i < taskCount - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
               
            } 
            taskCount--;
                break;
        case "update":
            if (parts.Length < 3)
            {
                Console.WriteLine("Неверный ввод , чего то не хватает");
                break;
            }
            if (!int.TryParse(parts[1], out id))
            {
                Console.WriteLine("Ошибка: номер задачи должен быть числом.");
                break;
            }

            if (string.IsNullOrWhiteSpace(parts[2]))
            {
                Console.WriteLine("Текст задачи не может быть пустым.");
                break;
            }
            int indexUpdate = id - 1;
            todos[indexUpdate] = parts[2];
            dates[indexUpdate] = DateTime.Now;
            break;
        default:
            Console.WriteLine("Неизвестная комманда. Введите help, для отображения списка команд");
            break;
    }
}