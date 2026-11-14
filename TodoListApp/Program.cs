using System;

namespace TodoListApp
{
   public class Program
    {
        static string name = "";
        static string last_name = "";
        static string BirthYear = "";

        public static void Main()
        {
            Console.WriteLine("Работу выполнили Мищенко и Крымова\n");
            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            string last_name = Console.ReadLine();
            Console.Write("Введите ваш год рождения: ");
            string BirthYear = Console.ReadLine();


            if (int.TryParse(BirthYear, out int birthYear))
            {
                int currentYear = DateTime.Now.Year;
                int age = currentYear - birthYear;

                Console.WriteLine($"Добавлен пользователь {name} {last_name}, возраст - {age} " );
            }

            else
            {
                Console.WriteLine("Ошибка: год рождения должен быть числом!");
            }
            
            Console.WriteLine("\nДобро пожаловать в менеджер задач! Введите 'help' для списка команд.\n");

            string[] todos = new string[2];

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                string[] parts = input.Split(new char[] { ' ' }, 2);
                string command = parts[0].ToLower();

                if (command == "help")
                {
                    Console.WriteLine("Доступные команды:");
                    Console.WriteLine("  help    - Список команд");
                    Console.WriteLine("  profile - Данные пользователя");
                    Console.WriteLine("  add     - Добавить задачу (add \"текст\")");
                    Console.WriteLine("  view    - Показать задачи");
                    Console.WriteLine("  exit    - Выход");
                }
                else if (command == "exit")
                {
                    break;
                }
                else if (command == "profile")
                {
                    Console.WriteLine($"{name} {last_name}, {BirthYear}");
                }
                else if (command == "view")
                {
                    Console.WriteLine("Ваши задачи:");
                    bool isEmpty = true;
                    
                    for (int i = 0; i < todos.Length; i++)
                    {
                        if (todos[i] != null && todos[i] != "")
                        {
                            Console.WriteLine($"  {i + 1}. {todos[i]}");
                            isEmpty = false;
                        }
                    }
                    if (isEmpty) Console.WriteLine("  (Список пуст)");
                }
                else if (command == "add")
                {
                    if (parts.Length > 1)
                    {
                        string taskText = parts[1].Trim('"'); 

                        int freeIndex = -1;
                        for (int i = 0; i < todos.Length; i++)
                        {
                            if (todos[i] == null || todos[i] == "")
                            {
                                freeIndex = i;
                                break;
                            }
                        }

                        if (freeIndex == -1)
                        {
                            Console.WriteLine(" Массив заполнен, расширяем...");
                            
                            string[] newTodos = new string[todos.Length * 2];
                        
                            for (int i = 0; i < todos.Length; i++)
                            {
                                newTodos[i] = todos[i];
                            }
                            
                            todos = newTodos; 
                            
                            freeIndex = todos.Length / 2; 
                        }

                        todos[freeIndex] = taskText;
                        Console.WriteLine($" Задача \"{taskText}\" добавлена.");
                    }
                    else
                    {
                        Console.WriteLine("Введите текст. Пример: add \"Купить хлеб\"");
                    }
                }
                
            }
        }
    } 
}