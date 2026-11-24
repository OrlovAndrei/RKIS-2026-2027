public class ProfileCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine(AppInfo.CurrentProfile.GetInfo());
    }
}
