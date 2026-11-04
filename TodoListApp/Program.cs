using System;
using System.Text; // Добавляем для работы с кодировками

namespace TodoListApp
{
    class Program
    {
        static void MainOld(string[] args)
        {
            // 1. Настраиваем кодировку, чтобы русские буквы корректно читались и выводились
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            // 2. Приветствие
            Console.WriteLine("Работу выполнили: Осинцева и Малюкова");

            // 3. Запрос имени
            Console.Write("Введите имя: ");
            // Знак "!" говорит компилятору, что мы уверены: здесь не будет null
            string firstName = Console.ReadLine()!;

            // 4. Запрос фамилии
            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine()!;

            // 5. Запрос года рождения
            Console.Write("Введите год рождения: ");
            string birthYearStr = Console.ReadLine()!;

            // 6. Преобразование строки в целое число
            int birthYear = Convert.ToInt32(birthYearStr);

            // 7. Вычисление возраста
            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;

            // 8. Вывод результата
            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

            // Чтобы консоль не закрылась сразу
            Console.ReadLine();
        }
    }
}