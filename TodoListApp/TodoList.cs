namespace TodoListApp
{
    internal class TodoList : IEnumerable<TodoItem>
    {
        private const string ColumnSeparator = " | ";
        private const char HeaderLineChar = '-';
        private const string HeaderIndex = "№";
        private const string HeaderText = "Задача";
        private const string HeaderStatus = "Статус";
        private const string HeaderDate = "Дата изменения";

        private readonly List<TodoItem> items = new List<TodoItem>();

        public int Count => items.Count;

        public TodoItem? this[int index]
        {
            get
            {
                if (index < 0 || index >= items.Count)
                {
                    return null;
                }

                return items[index];
            }
        }

        public IEnumerator<TodoItem> GetEnumerator()
        {
            for (int i = 0; i < items.Count; i++)
            {
                yield return items[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TodoItem item)
        {
            if (item == null)
            {
                return;
            }

            items.Add(item);
        }

        public bool Delete(int index)
        {
            if (index < 0 || index >= items.Count)
            {
                return false;
            }

            items.RemoveAt(index);
            return true;
        }

        public bool SetStatus(int index, TodoStatus status)
        {
            TodoItem? item = this[index];
            if (item == null)
            {
                return false;
            }

            item.SetStatus(status);
            return true;
        }

        public TodoItem? GetItem(int index)
        {
            return this[index];
        }

        public void View(bool showIndex, bool showDone, bool showDate)
        {
            if (items.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            List<string> headers = new List<string>();
            if (showIndex)
            {
                headers.Add(HeaderIndex);
            }

            headers.Add(HeaderText);

            if (showDone)
            {
                headers.Add(HeaderStatus);
            }

            if (showDate)
            {
                headers.Add(HeaderDate);
            }

            List<List<string>> rows = new List<List<string>>();
            int number = 1;
            foreach (TodoItem item in this)
            {
                List<string> cells = new List<string>();

                if (showIndex)
                {
                    cells.Add(number.ToString());
                }

                cells.Add(item.GetShortText());

                if (showDone)
                {
                    cells.Add(item.GetStatusText());
                }

                if (showDate)
                {
                    cells.Add(item.GetDateText());
                }

                rows.Add(cells);
                number++;
            }

            PrintTable(headers, rows);
        }

        private static void PrintRow(List<string> cells, int[] widths)
        {
            List<string> paddedCells = new List<string>();
            for (int column = 0; column < cells.Count; column++)
            {
                paddedCells.Add(cells[column].PadRight(widths[column]));
            }

            Console.WriteLine(string.Join(ColumnSeparator, paddedCells));
        }

        private static void PrintTable(List<string> headers, List<List<string>> rows)
        {
            int[] widths = new int[headers.Count];
            for (int column = 0; column < headers.Count; column++)
            {
                widths[column] = headers[column].Length;
                foreach (List<string> row in rows)
                {
                    if (row[column].Length > widths[column])
                    {
                        widths[column] = row[column].Length;
                    }
                }
            }

            PrintRow(headers, widths);

            int totalWidth = widths.Sum() + ColumnSeparator.Length * (widths.Length - 1);
            Console.WriteLine(new string(HeaderLineChar, totalWidth));

            foreach (List<string> row in rows)
            {
                PrintRow(row, widths);
            }
        }
    }
}