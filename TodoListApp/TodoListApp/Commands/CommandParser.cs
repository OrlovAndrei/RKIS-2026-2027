namespace TodoListApp.Commands;

internal static class CommandParser
{
    private static readonly Dictionary<string, Func<string, ICommand>> CommandHandlers =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["add"] = arguments => new AddCommand(arguments),
            ["view"] = arguments => new ViewCommand(arguments),
            ["read"] = arguments => new ReadCommand(arguments),
            ["status"] = arguments => new StatusCommand(arguments),
            ["update"] = arguments => new UpdateCommand(arguments),
            ["delete"] = arguments => new DeleteCommand(arguments),
            ["profile"] = arguments => new ProfileCommand(arguments),
            ["search"] = arguments => new SearchCommand(arguments),
            ["stats"] = _ => new StatisticsCommand(),
            ["undo"] = _ => new UndoCommand(),
            ["redo"] = _ => new RedoCommand(),
            ["help"] = _ => new HelpCommand(),
            ["types"] = _ => new TypesDemoCommand(),
            ["collections"] = _ => new CollectionsDemoCommand(),
            ["exit"] = _ => new ExitCommand()
        };

    public static ICommand Parse(string input)
    {
        (string commandName, string arguments) = SplitCommand(input);
        if (CommandHandlers.TryGetValue(commandName, out Func<string, ICommand>? handler))
        {
            return handler(arguments);
        }

        return commandName.Length == 0
            ? new EmptyCommand()
            : new UnknownCommand(commandName);
    }

    private static (string Command, string Arguments) SplitCommand(string input)
    {
        string value = input.Trim();
        int separator = value.IndexOf(' ');
        return separator < 0
            ? (value, string.Empty)
            : (value[..separator], value[(separator + 1)..].Trim());
    }
}
