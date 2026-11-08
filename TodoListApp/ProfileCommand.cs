using System;

namespace TodoListApp
{
    public class ProfileCommand : ICommand
    {
        public Profile Profile { get; set; }

        public void Execute()
        {
            if (Profile != null)
            {
                Console.WriteLine(Profile.GetInfo());
            }
        }
    }
}