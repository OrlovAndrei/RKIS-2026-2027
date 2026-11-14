using System;

namespace TodoListApp
{
   public class Program
    {
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



        }
    } 
}