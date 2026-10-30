// Подключаем функции консольного ввода и вывода.
using System;

// Создаём команду просмотра профиля пользователя.
public class ProfileCommand : ICommand
{
    // Храним профиль пользователя.
    public Profile Profile { get; set; } = null!;

    // Выполняем команду просмотра профиля.
    public void Execute()
    {
        // Получаем информацию о пользователе.
        string info = Profile.GetInfo();

        // Выводим информацию в консоль.
        Console.WriteLine(info);
    }
}