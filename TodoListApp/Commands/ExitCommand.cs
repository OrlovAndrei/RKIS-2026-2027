public class ExitCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("До свидания!");
        Environment.Exit(0);
    }
}
