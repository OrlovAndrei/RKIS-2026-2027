// Возвращается парсером, если команду не удалось разобрать: неизвестное имя, неверный флаг или номер.
// Так главный цикл всегда получает объект ICommand и не проверяет результат на null
public class ErrorCommand : ICommand
{
    public string Message { get; set; } = "";

    public void Execute()
    {
        Console.WriteLine(Message);
    }
}
