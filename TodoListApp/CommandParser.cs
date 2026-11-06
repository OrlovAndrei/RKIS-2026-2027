using System;

static class CommandParser
{
    public static ICommand Parse(string inputString, TodoList todoList, Profile? profile)
    {
        string input = inputString.Trim();

        if (input == "help")
        {
            return new HelpCommand();
        }
        else if (input == "profile")
        {
            return new ProfileCommand { Profile = profile };
        }
        else if (input == "exit")
        {
            return new ExitCommand();
        }
        else if (input.StartsWith("add"))
        {
            return ParseAdd(input, todoList);
        }
        else if (input == "view" || input.StartsWith("view "))
        {
            return ParseView(input, todoList);
        }
        else if (input.StartsWith("done"))
        {
            return ParseDone(input, todoList);
        }
        else if (input.StartsWith("delete"))
        {
            return ParseDelete(input, todoList);
        }
        else if (input.StartsWith("update"))
        {
            return ParseUpdate(input, todoList);
        }
        else if (input.StartsWith("read"))
        {
            return ParseRead(input, todoList);
        }

        return new UnknownCommand("Неизвестная команда. Введите help для списка команд.");
    }

    private static ICommand ParseAdd(string input, TodoList todoList)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 2 && (parts[1] == "--multiline" || parts[1] == "-m"))
        {
            return new AddCommand
            {
                TodoList = todoList,
                IsMultiline = true
            };
        }

        string? task = ExtractQuotedText(input);
        if (!string.IsNullOrWhiteSpace(task))
        {
            return new AddCommand
            {
                TodoList = todoList,
                Text = task,
                IsMultiline = false
            };
        }

        return new UnknownCommand("Неверный формат. Используйте: add \"текст задачи\" или add --multiline");
    }

    private static ICommand ParseView(string input, TodoList todoList)
    {
        bool showIndex = false;
        bool showStatus = false;
        bool showDate = false;

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < parts.Length; i++)
        {
            string flag = parts[i];

            if (flag == "--all")
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }
            else if (flag == "--index")
            {
                showIndex = true;
            }
            else if (flag == "--status")
            {
                showStatus = true;
            }
            else if (flag == "--update-date")
            {
                showDate = true;
            }
            else if (flag.StartsWith("-") && !flag.StartsWith("--") && flag.Length > 1)
            {
                for (int j = 1; j < flag.Length; j++)
                {
                    char c = flag[j];
                    if (c == 'i')
                    {
                        showIndex = true;
                    }
                    else if (c == 's')
                    {
                        showStatus = true;
                    }
                    else if (c == 'd')
                    {
                        showDate = true;
                    }
                    else if (c == 'a')
                    {
                        showIndex = true;
                        showStatus = true;
                        showDate = true;
                    }
                    else
                    {
                        return new UnknownCommand($"Неизвестный флаг: -{c}");
                    }
                }
            }
            else
            {
                return new UnknownCommand($"Неизвестный флаг: {flag}");
            }
        }

        return new ViewCommand
        {
            TodoList = todoList,
            ShowIndex = showIndex,
            ShowStatus = showStatus,
            ShowDate = showDate
        };
    }

    private static ICommand ParseDone(string input, TodoList todoList)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2 && int.TryParse(parts[1], out int number))
        {
            return new DoneCommand
            {
                TodoList = todoList,
                Index = number - 1
            };
        }

        return new UnknownCommand("Неверный номер задачи. Используйте: done <номер>");
    }

    private static ICommand ParseDelete(string input, TodoList todoList)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2 && int.TryParse(parts[1], out int number))
        {
            return new DeleteCommand
            {
                TodoList = todoList,
                Index = number - 1
            };
        }

        return new UnknownCommand("Неверный номер задачи. Используйте: delete <номер>");
    }

    private static ICommand ParseUpdate(string input, TodoList todoList)
    {
        string[] quoteParts = input.Split('"');
        if (quoteParts.Length >= 2 && !string.IsNullOrWhiteSpace(quoteParts[1]))
        {
            string[] firstParts = quoteParts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (firstParts.Length >= 2 && int.TryParse(firstParts[1], out int number))
            {
                return new UpdateCommand
                {
                    TodoList = todoList,
                    Index = number - 1,
                    Text = quoteParts[1]
                };
            }
        }

        return new UnknownCommand("Неверный формат или номер задачи. Используйте: update <номер> \"новый текст\"");
    }

    private static ICommand ParseRead(string input, TodoList todoList)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2 && int.TryParse(parts[1], out int number))
        {
            return new ReadCommand
            {
                TodoList = todoList,
                Index = number - 1
            };
        }

        return new UnknownCommand("Неверный номер задачи. Используйте: read <номер>");
    }

    private static string? ExtractQuotedText(string input)
    {
        string[] parts = input.Split('"');
        if (parts.Length >= 2 && !string.IsNullOrWhiteSpace(parts[1]))
        {
            return parts[1];
        }
        return null;
    }
}

