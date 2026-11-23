using System;

namespace TodoListApp;

public class HelpCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine(@"Доступные команды:
  help                        - список команд
  profile                     - данные пользователя
  add <текст>                 - добавить задачу в одну строку
  add --multiline (-m)        - добавить многострочную задачу (до !end)
  view [-i] [-s] [-d] [-a]    - показать таблицу задач (флаги можно объединять: -is)
  read <индекс>               - показать полный текст, статус и дату задачи
  done <индекс>               - отметить задачу выполненной
  delete <индекс>             - удалить задачу по индексу
  update <индекс> <текст>     - обновить текст задачи
  exit                        - выход");
    }
}