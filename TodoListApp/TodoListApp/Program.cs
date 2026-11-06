using TodoListApp;
using TodoListApp.Commands;
using TodoListApp.Services;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        AppInfo.Initialize();

        while (!AppInfo.ExitRequested)
        {
            var profile = AccountSession.SelectProfile();
            AppInfo.Activate(profile);
            Console.WriteLine($"\nЗдравствуйте, {profile.FullName}!");
            new HelpCommand().Execute();

            while (AppInfo.CurrentProfileId is not null && !AppInfo.ExitRequested)
            {
                Console.Write("\n> ");
                string? input = Console.ReadLine();
                if (input is null)
                {
                    AppInfo.ExitRequested = true;
                    break;
                }

                ICommand command = CommandParser.Parse(input);
                CommandHistory.Execute(command);
            }
        }
    }
}
