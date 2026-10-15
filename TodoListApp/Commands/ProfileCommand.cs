public class ProfileCommand : ICommand
{
    public required Profile Profile { get; set; }

    public void Execute()
    {
        Console.WriteLine(Profile.GetInfo());
    }
}
