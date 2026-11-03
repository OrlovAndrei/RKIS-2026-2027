namespace TodoListApp
{
    internal class ExitCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Завершение работы.");
        }
    }
}