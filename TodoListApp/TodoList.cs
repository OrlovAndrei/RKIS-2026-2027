namespace TodoListApp
{
    internal class TodoList
    {
        private const int InitialCapacity = 2;
        private const int CapacityGrowthFactor = 2;

        private const string ColumnSeparator = " | ";
        private const char HeaderLineChar = '-';
        private const string HeaderIndex = "№";
        private const string HeaderText = "Задача";
        private const string HeaderStatus = "Статус";
        private const string HeaderDate = "Дата изменения";

        private TodoItem?[] items = new TodoItem?[InitialCapacity];
        private int count = 0;

        public int Count => count;

        public void Add(TodoItem item)
        {
            if (item == null)
            {
                return;
            }

            if (count == items.Length)
            {
                IncreaseArray(items, item);
            }
            else
            {
                items[count] = item;
            }

            count++;
        }

        public bool Delete(int index)
        {
            if (index < 0 || index >= count)
            {
                return false;
            }

            for (int i = index; i < count - 1; i++)
            {
                items[i] = items[i + 1];
            }

            items[count - 1] = null;
            count--;
            return true;
        }

        public TodoItem? GetItem(int index)
        {
            if (index < 0 || index >= count)
            {
                return null;
            }

            return items[index];
        }

        public void View(bool showIndex, bool showDone, bool showDate)
        {
            if (count == 0)
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
            for (int i = 0; i < count; i++)
            {
                TodoItem item = items[i]!;
                List<string> cells = new List<string>();

                if (showIndex)
                {
                    cells.Add((i + 1).ToString());
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
            }

            PrintTable(headers, rows);
        }

        private void IncreaseArray(TodoItem?[] oldItems, TodoItem item)
        {
            TodoItem?[] newItems = new TodoItem?[oldItems.Length * CapacityGrowthFactor];
            for (int i = 0; i < oldItems.Length; i++)
            {
                newItems[i] = oldItems[i];
            }

            newItems[count] = item;
            items = newItems;
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