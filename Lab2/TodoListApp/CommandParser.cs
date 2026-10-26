using System;

namespace TodoListApp
{
    // Пункт 4: Статический класс для разбора строк ввода
    public static class CommandParser
    {
        // Часть 1, Пункт 1: Словарь делегатов-обработчиков команд
        private static readonly Dictionary<string, Func<string, TodoList, Profile, ICommand>> _commandHandlers = new();

        // Часть 1, Пункт 2: Статический конструктор для регистрации команд
        static CommandParser()
        {
            _commandHandlers["profile"] = (args, list, prof) => new ProfileCommand(prof, string.IsNullOrWhiteSpace(args) ? "profile" : "profile " + args);
            _commandHandlers["add"] = (args, list, prof) => new AddCommand(list, "add " + args);
            _commandHandlers["view"] = (args, list, prof) => new ViewCommand(list, string.IsNullOrWhiteSpace(args) ? "view" : "view " + args);
            _commandHandlers["status"] = (args, list, prof) => new StatusCommand(list, "status " + args);
            _commandHandlers["undo"] = (args, list, prof) => new UndoCommand();
            _commandHandlers["redo"] = (args, list, prof) => new RedoCommand();
        }

        // Часть 1, Пункт 3: Метод Parse на основе словаря делегатов
        public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
        {
            var parts = inputString.Split(new[] { ' ' }, 2);
            var cmdName = parts[0].ToLower();
            var args = parts.Length > 1 ? parts[1] : "";

            if (_commandHandlers.TryGetValue(cmdName, out var handler))
            {
                return handler(args, todoList, profile);
            }

            return new UnknownCommand(inputString);
        }
    }
     // Вспомогательный класс для обработки неизвестных команд в паттерне
    public class UnknownCommand : ICommand
    {
        private readonly string _input;
        public UnknownCommand(string input) { _input = input; }

        public void Execute() 
        {
            if (_input.ToLower() != "help" && _input.ToLower() != "exit")
            {
                Console.WriteLine("Неизвестная команда. Введите 'help'.\n");
            }
        }
        // Пункт 5: Неотменяемая команда — метод отката оставляем пустым
        public void Unexecute()
        {
        }

    }
    // Класс-заглушка для команды отмены (Undo) по паттерну Команда
    public class UndoCommand : ICommand
    {
        public void Execute() { }
        public void Unexecute() { }
    }

    // Класс-заглушка для команды повтора (Redo) по паттерну Команда
    public class RedoCommand : ICommand
    {
        public void Execute() { }
        public void Unexecute() { }
    }

}