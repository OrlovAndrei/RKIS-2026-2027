using System;

string name;
string lastName;
int yearOfBirth;
int year = DateTime.Now.Year;

Console.Write("Работу выполнили Береснев Роман и Кузнецов Александр \n");
Console.Write("Введите свое имя: ");
name = Console.ReadLine();

Console.Write("Введите свою фамилию: ");
lastName = Console.ReadLine();

Console.Write("Введите свой год рождения: ");
yearOfBirth = int.Parse(Console.ReadLine());
int age = year - yearOfBirth;

Console.Write($"Добавлен пользователь с именем: {name} {lastName}\nВ возрасте: {age} \n");


// Работа 3
string[] todos = new string[2];
int todoCount = 0;
bool running = true; //нужно для того чтобы exit останавливал работу, еще можно было сделать метод и через return

while (running){
    Console.Write("Введите команду: ");
    string userInput = Console.ReadLine();

    // есть 2 стула, if else и switch, выполняют одно и то же, но switch выглядит похайповее (удобнее)
    switch (userInput){
        case "exit":
            Console.WriteLine("Завершение работы программы");
            running = false;
            break;

        case "help":
            Console.WriteLine("help — выводит это сообщение.");
            Console.WriteLine("profile — выводит данные пользователя в формате: <Имя> <Фамилия>, <Год рождения> .");
            Console.WriteLine("add — добавляет новую задачу. Формат ввода: add  \"текст задачи\".");
            Console.WriteLine("view — выводит все задачи из массива.");
            Console.WriteLine("exit — завершает цикл и останавливает выполнение программы.");
            break;

        case "profile":
            Console.WriteLine($"{name} {lastName}, {age}");
            break;

        case string s when s.StartsWith("add "):
            string raw = s.Substring(4).Trim();
            if (raw.Length >= 2 && raw.StartsWith("\"") && raw.EndsWith("\"")){
                string task = raw.Substring(1, raw.Length - 2);

            if (todoCount >= todos.Length){
                string[] newTodos = new string[todos.Length * 2];
                for (int i = 0; i < todos.Length; i++){
                    newTodos[i] = todos[i];
                    todos = newTodos;
                }
                todos[todoCount] = task;
                todoCount++;
                Console.WriteLine($"Добавлено: {task}");
            }else{
                Console.WriteLine("Ошибка: текст задачи должен быть в кавычках. Пример: add \"купить хлеб\"");
            }
            break;

        case "view":
            if (todos.Count == 0){
                Console.WriteLine("Список задач пуст.");
                break;
            }
            for (int i = 0; i < todos.Count; i++){
                Console.WriteLine($"[{i + 1}] {todos[i]}");
            }
            break;
            
        default:
            Console.WriteLine("Неизвестная команда, введите help для вывода списка команд");
            break;
    }
}
