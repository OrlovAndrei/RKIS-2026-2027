namespace TodoListApp
{
    // Пункт 3: Класс конкретной команды для отображения списка задач
    public class ViewCommand : ICommand
    {
        private readonly TodoList _todoList;
        private readonly string _input;

        public ViewCommand(TodoList todoList, string input)
        {
            _todoList = todoList;
            _input = input;
        }

        public void Execute()
        {
            // Разбираем флаги форматирования таблицы задач
            bool showIndex = _input.Contains("--index") || _input.Contains(" -i") || _input.Contains("--all") || _input.Contains(" -a");
            bool showStatus = _input.Contains("--status") || _input.Contains(" -s") || _input.Contains("--all") || _input.Contains(" -a");
            bool showDate = _input.Contains("--update-date") || _input.Contains(" -d") || _input.Contains("--all") || _input.Contains(" -a");

            // Передаем флаги в метод View нашего менеджера TodoList
            _todoList.View(showIndex, showStatus, showDate, _input);
        }
         // Пункт 5: Неотменяемая команда просмотра — метод оката оставляем пустым
        public void Unexecute()
        {
        }

    }
}