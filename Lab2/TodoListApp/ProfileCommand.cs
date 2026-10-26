using System;

namespace TodoListApp
{
    // Пункт 3: Класс конкретной команды для вывода профиля
    public class ProfileCommand : ICommand
    {
        private readonly Profile _profile;
        private readonly string _input;
            public ProfileCommand(Profile profile, string input)
    {
        _profile = profile;
        _input = input;
    }


        public void Execute()
        {
            Console.WriteLine($"\nПрофиль: {_profile.GetInfo()}\n");
        }
        // Пункт 5: Неотменяемая команда — метод оката оставляем пустым
        public void Unexecute()
        {
        }
    }
}
