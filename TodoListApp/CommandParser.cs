namespace TodoListApp
{
    internal static class CommandParser
    {
        private const char TaskTextSeparator = '"';
        private const char ArgumentSeparator = ' ';

        private const string CommandHelp = "help";
        private const string CommandProfile = "profile";
        private const string CommandAdd = "add";
        private const string CommandView = "view";
        private const string CommandRead = "read";
        private const string CommandDone = "done";
        private const string CommandDelete = "delete";
        private const string CommandUpdate = "update";
        private const string CommandExit = "exit";
        private const string CommandStatus = "status";

        private const string FlagMultilineLong = "--multiline";
        private const string FlagMultilineShort = "-m";

        private const string FlagIndexLong = "--index";
        private const string FlagStatusLong = "--status";
        private const string FlagDateLong = "--update-date";
        private const string FlagAllLong = "--all";
        private const string FlagPrefix = "-";
        private const string LongFlagPrefix = "--";
        private const char LetterIndex = 'i';
        private const char LetterStatus = 's';
        private const char LetterDate = 'd';
        private const char LetterAll = 'a';

        private const string UnknownCommandMessage = "Неизвестная команда. Введите help для списка команд.";
        private const string AddFormatMessage = "Формат: add \"текст задачи\" или add --multiline";
        private const string ViewFormatMessage = "Неизвестный флаг. Формат: view [--index|-i] [--status|-s] [--update-date|-d] [--all|-a]";
        private const string ReadFormatMessage = "Формат: read <номер задачи>";
        private const string DoneFormatMessage = "Формат: done <номер задачи>";
        private const string DeleteFormatMessage = "Формат: delete <номер задачи>";
        private const string UpdateFormatMessage = "Формат: update <номер задачи> \"новый текст\"";
        private const string StatusFormatMessage = "Формат: status <номер задачи> <статус>. Статусы: notstarted, inprogress, completed, postponed, failed";

        public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
        {
            string input = (inputString ?? "").Trim();
            int separatorIndex = input.IndexOf(ArgumentSeparator);
            string commandName = separatorIndex < 0 ? input : input.Substring(0, separatorIndex);
            string arguments = separatorIndex < 0 ? "" : input.Substring(separatorIndex + 1).Trim();

            switch (commandName)
            {
                case CommandHelp:
                    return new HelpCommand();
                case CommandExit:
                    return new ExitCommand();
                case CommandProfile:
                    return new ProfileCommand { Profile = profile };
                case CommandAdd:
                    return ParseAdd(arguments, todoList);
                case CommandView:
                    return ParseView(arguments, todoList);
                case CommandRead:
                    return ParseRead(arguments, todoList);
                case CommandDone:
                    return ParseDone(arguments, todoList);
                case CommandDelete:
                    return ParseDelete(arguments, todoList);
                case CommandUpdate:
                    return ParseUpdate(arguments, todoList);
                case CommandStatus:
                    return ParseStatus(arguments, todoList);
                default:
                    return CreateError(UnknownCommandMessage);
            }
        }

        private static ICommand CreateError(string message)
        {
            return new ErrorCommand { Message = message };
        }

        private static bool TryParseIndex(string text, out int index)
        {
            index = -1;
            if (!int.TryParse(text, out int number) || number < 1)
            {
                return false;
            }

            index = number - 1;
            return true;
        }

        private static ICommand ParseAdd(string arguments, TodoList todoList)
        {
            if (arguments == FlagMultilineLong || arguments == FlagMultilineShort)
            {
                return new AddCommand { IsMultiline = true, TodoList = todoList };
            }

            string[] parts = arguments.Split(TaskTextSeparator);
            if (parts.Length < 2 || parts[1] == "")
            {
                return CreateError(AddFormatMessage);
            }

            return new AddCommand { Text = parts[1], TodoList = todoList };
        }

        private static string ToShortFlag(string flag)
        {
            switch (flag)
            {
                case FlagIndexLong:
                    return FlagPrefix + LetterIndex;
                case FlagStatusLong:
                    return FlagPrefix + LetterStatus;
                case FlagDateLong:
                    return FlagPrefix + LetterDate;
                case FlagAllLong:
                    return FlagPrefix + LetterAll;
                default:
                    return flag;
            }
        }

        private static ICommand ParseView(string arguments, TodoList todoList)
        {
            ViewCommand command = new ViewCommand { TodoList = todoList };

            string[] flags = arguments.Split(ArgumentSeparator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rawFlag in flags)
            {
                string flag = ToShortFlag(rawFlag);
                if (!flag.StartsWith(FlagPrefix) || flag.StartsWith(LongFlagPrefix) || flag.Length < 2)
                {
                    return CreateError(ViewFormatMessage);
                }

                foreach (char letter in flag.Substring(1))
                {
                    switch (letter)
                    {
                        case LetterIndex:
                            command.ShowIndex = true;
                            break;
                        case LetterStatus:
                            command.ShowStatus = true;
                            break;
                        case LetterDate:
                            command.ShowDate = true;
                            break;
                        case LetterAll:
                            command.ShowIndex = true;
                            command.ShowStatus = true;
                            command.ShowDate = true;
                            break;
                        default:
                            return CreateError(ViewFormatMessage);
                    }
                }
            }

            return command;
        }

        private static ICommand ParseRead(string arguments, TodoList todoList)
        {
            if (!TryParseIndex(arguments, out int index))
            {
                return CreateError(ReadFormatMessage);
            }

            return new ReadCommand { Index = index, TodoList = todoList };
        }

        private static ICommand ParseDone(string arguments, TodoList todoList)
        {
            if (!TryParseIndex(arguments, out int index))
            {
                return CreateError(DoneFormatMessage);
            }

            return new DoneCommand { Index = index, TodoList = todoList };
        }

        private static ICommand ParseDelete(string arguments, TodoList todoList)
        {
            if (!TryParseIndex(arguments, out int index))
            {
                return CreateError(DeleteFormatMessage);
            }

            return new DeleteCommand { Index = index, TodoList = todoList };
        }
        private static ICommand ParseStatus(string arguments, TodoList todoList)
        {
            string[] parts = arguments.Split(ArgumentSeparator, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2
                || !TryParseIndex(parts[0], out int index)
                || !Enum.TryParse(parts[1], true, out TodoStatus status)
                || !Enum.IsDefined(typeof(TodoStatus), status))
            {
                return CreateError(StatusFormatMessage);
            }

            return new StatusCommand { Index = index, Status = status, TodoList = todoList };
        }


        private static ICommand ParseUpdate(string arguments, TodoList todoList)
        {
            string[] parts = arguments.Split(TaskTextSeparator);
            if (parts.Length < 2 || parts[1] == "" || !TryParseIndex(parts[0].Trim(), out int index))
            {
                return CreateError(UpdateFormatMessage);
            }

            return new UpdateCommand { Index = index, Text = parts[1], TodoList = todoList };
        }
    }
}