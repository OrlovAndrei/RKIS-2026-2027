// Общий контракт для всех команд: парсер создаёт команду, а главный цикл просто вызывает Execute()
public interface ICommand
{
    void Execute();
}
