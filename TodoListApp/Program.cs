namespace TodoListApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Работу выполнил Добрынин");

            FileManager.EnsureDataDirectory(FileManager.DataDirectoryPath);

            Profile profile = LoadOrCreateProfile();
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

        private static Profile LoadOrCreateProfile()
        {
            Profile? loadedProfile = FileManager.LoadProfile(FileManager.ProfileFilePath);
            if (loadedProfile != null)
            {
                Console.WriteLine($"Загружен профиль: {loadedProfile.GetInfo()}");
                return loadedProfile;
            }

            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine() ?? "";

            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine() ?? "";

            int birthYear;
            while (true)
            {
                Console.Write("Введите год рождения: ");
                if (int.TryParse(Console.ReadLine(), out birthYear))
                {
                    break;
                }

                Console.WriteLine("Год рождения должен быть числом.");
            }

            int age = DateTime.Now.Year - birthYear;
            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

            Profile newProfile = new Profile(firstName, lastName, birthYear);
            FileManager.SaveProfile(newProfile, FileManager.ProfileFilePath);
            return newProfile;
        }
    }
}