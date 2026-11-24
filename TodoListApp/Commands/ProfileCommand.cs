public class ProfileCommand : ICommand
{
    // Флаг --out / -o: выйти из профиля
    public bool Out { get; set; }

    public void Execute()
    {
        if (Out)
        {
            // Главный цикл увидит, что никто не вошёл, и предложит выбрать профиль заново
            AppInfo.CurrentProfileId = null;
            Console.WriteLine("Вы вышли из профиля.");
            return;
        }

        Profile? profile = AppInfo.CurrentProfile;
        if (profile is not null)
            Console.WriteLine(profile.GetInfo());
    }
}
