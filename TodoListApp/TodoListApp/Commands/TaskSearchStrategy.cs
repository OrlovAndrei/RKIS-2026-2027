using TodoListApp.Models;

namespace TodoListApp.Commands;

internal interface ITaskSearchStrategy
{
    bool Matches(TodoItem item, string query);
}

internal sealed class ContainsTextStrategy : ITaskSearchStrategy
{
    public bool Matches(TodoItem item, string query) =>
        item.Text.Contains(query, StringComparison.CurrentCultureIgnoreCase);
}

internal sealed class StartsWithTextStrategy : ITaskSearchStrategy
{
    public bool Matches(TodoItem item, string query) =>
        item.Text.StartsWith(query, StringComparison.CurrentCultureIgnoreCase);
}

internal static class TaskSearchStrategyFactory
{
    public static ITaskSearchStrategy Create(bool prefixOnly) =>
        prefixOnly ? new StartsWithTextStrategy() : new ContainsTextStrategy();
}
