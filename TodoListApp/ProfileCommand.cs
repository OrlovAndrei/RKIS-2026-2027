namespace TodoListApp
{
    internal class ProfileCommand : ICommand
    {
        public Profile? Profile { get; set; }

        public void Execute()
        {
            if (Profile == null)
            {
                return;
            }

            Console.WriteLine(Profile.GetInfo());
        }
    }
}