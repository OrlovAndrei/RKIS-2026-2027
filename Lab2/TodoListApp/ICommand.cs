namespace TodoListApp
{
    // Пункт 2: Интерфейс для реализации паттерна "Команда"
    public interface ICommand
    {
        void Execute();
        // Пункт 3: Метод для отмены действий команды
        void Unexecute();
    }
}