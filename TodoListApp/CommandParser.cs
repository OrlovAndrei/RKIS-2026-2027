using System;

namespace TodoListApp
{
    public static class CommandParser
    {
        public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
        {
            if (string.IsNullOrWhiteSpace(inputString)) return null;

            string[] parts = inputString.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToLower();
            string arguments = parts.Length > 1 ? parts[1] : "";

            if (command == "help")
            {
                return new HelpCommand();
            }
            else if (command == "profile")
            {
                return new ProfileCommand { Profile = profile };
            }
            else if (command == "add")
            {
                AddCommand cmd = new AddCommand { TodoList = todoList };
                if (arguments.Contains("-m") || arguments.Contains("--multiline"))
                {
                    cmd.IsMultiline = true;
                }
                else
                {
                    cmd.TaskText = arguments.Trim('"');
                }
                return cmd;
            }
            else if (command == "view")
            {
                ViewCommand cmd = new ViewCommand { TodoList = todoList };
                cmd.ShowIndex = arguments.Contains("-i") || arguments.Contains("--index");
                cmd.ShowStatus = arguments.Contains("-s") || arguments.Contains("--status");
                cmd.ShowDate = arguments.Contains("-d") || arguments.Contains("--update-date");
                bool showAll = arguments.Contains("-a") || arguments.Contains("--all");
                if (showAll)
                {
                    cmd.ShowIndex = true;
                    cmd.ShowStatus = true;
                    cmd.ShowDate = true;
                }
                return cmd;
            }
            else if (command == "done")
            {
                if (int.TryParse(arguments, out int idx))
                {
                    return new DoneCommand { Index = idx, TodoList = todoList };
                }
            }
            else if (command == "delete")
            {
                if (int.TryParse(arguments, out int idx))
                {
                    return new DeleteCommand { Index = idx, TodoList = todoList };
                }
            }
            else if (command == "update")
            {
                string[] updateParts = arguments.Split(new char[] { ' ' }, 2);
                if (updateParts.Length > 1 && int.TryParse(updateParts[0], out int idx))
                {
                    return new UpdateCommand { Index = idx, NewText = updateParts[1].Trim('"'), TodoList = todoList };
                }
            }
            else if (command == "read")
            {
                if (int.TryParse(arguments, out int idx))
                {
                    return new ReadCommand { Index = idx, TodoList = todoList };
                }
            }
            else if (command == "exit")
            {
                return new ExitCommand();
            }

            return null;
        }
    }
}