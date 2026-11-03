namespace TodoListApp
{
    internal class ErrorCommand : ICommand
    {
        public string Message { get; set; } = "";

        public void Execute()
        {
            Console.WriteLine(Message);
        }
    }
}