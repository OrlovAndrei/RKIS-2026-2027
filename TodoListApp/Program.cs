    using System;

    namespace TodoListApp
    {
        internal class Program
        {
            static void Main(string[] args)
            {
                // Вывод измененного текста из шага 7
                Console.WriteLine("Работу выполнили: Кудрявцев и Гапченко");
                Console.WriteLine(); // Пустая строка для красоты

                // Запрос имени
                Console.Write("Введите ваше имя: ");
                string firstName = Console.ReadLine() ?? string.Empty;

                // Запрос фамилии
                Console.Write("Введите вашу фамилию: ");
                string lastName = Console.ReadLine() ?? string.Empty;

                // Запрос года рождения
                Console.Write("Введите год рождения: ");
                string yearInput = Console.ReadLine() ?? string.Empty;

                // Преобразование года рождения в целое число и вычисление возраста
                if (int.TryParse(yearInput, out int birthYear))
                {
                    int currentYear = DateTime.Now.Year; // Текущий год
                    int age = currentYear - birthYear;   // Вычисление возраста

                    // Вывод итогового сообщения
                    Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");
                    string[] todos = new string[2];
                    
                    while (true)
                    {
                        Console.Write("> ");
                        string input = Console.ReadLine() ?? string.Empty;
                        string command = input.Split(' ')[0];

                        if (command == "exit")
                        {
                            break;
                        }
                        else if (command == "help")
                        {
                            Console.WriteLine("help - список команд");
                            Console.WriteLine("profile - данные пользователя");
                            Console.WriteLine("add \"текст задачи\" - добавить задачу");
                            Console.WriteLine("view - показать все задачи");
                            Console.WriteLine("exit - выход");
                        }
                        else if (command == "profile")
                        {
                            Console.WriteLine(firstName + " " + lastName + ", " + birthYear);
                        }
                        else if (command == "add")
                        {
                            string[] parts = input.Split('"');
                            string task = parts[1];

                            for (int i = 0; i < todos.Length; i++)
                            {
                                if (todos[i] == null)
                                {
                                    todos[i] = task;    
                                    break;
                                }
                            }
                        }
                        else if (command == "view")
                        {
                            for (int i = 0; i < todos.Length; i++)
                            {
                                if (todos[i] != null)
                                {
                                    Console.WriteLine(todos[i]);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: год рождения должен быть числом.");
                }

                // Чтобы консоль не закрывалась сразу
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }