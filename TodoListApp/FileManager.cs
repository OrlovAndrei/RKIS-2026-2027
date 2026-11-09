using System; // Базовые типы .NET (DateTime, исключения и др.)
using System.IO; // Работа с файлами и папками (File, Directory, исключения ввода-вывода)
using System.Globalization; // Культуры и стили чисел/дат (CultureInfo, NumberStyles, DateTimeStyles)
using System.Collections.Generic; // Обобщённые коллекции (List)
using System.Text; // Класс StringBuilder для пошаговой сборки строк

// Статический класс для сохранения и загрузки данных приложения.
public static class FileManager // Статический: экземпляры не создаются, методы вызываются как FileManager.Метод()
{
    // Проверяет наличие папки и создаёт её при необходимости.
    public static void EnsureDataDirectory(string dirPath) // Принимает путь к папке, ничего не возвращает
    {
        if (!Directory.Exists(dirPath)) // Если папки по этому пути нет...
        {
            Directory.CreateDirectory(dirPath); // ...создаём её (вместе с недостающими родительскими)
        }
    } // Конец метода EnsureDataDirectory

    // Сохраняет данные пользователя в текстовый файл.
    public static void SaveProfile(Profile profile, string filePath) // Принимает профиль и путь к файлу
    {
        string[] lines = // Массив строк: каждый элемент станет отдельной строкой файла
        {
            profile.FirstName, // 1-я строка файла: имя
            profile.LastName, // 2-я строка файла: фамилия
            profile.BirthYear.ToString(CultureInfo.InvariantCulture) // 3-я строка: год рождения в текст, независимо от региональных настроек
        }; // Конец массива

        File.WriteAllLines(filePath, lines); // Записываем строки в файл (существующий файл перезаписывается)
    } // Конец метода SaveProfile

    // Загружает профиль пользователя из файла.
    public static Profile LoadProfile(string filePath) // Принимает путь к файлу, возвращает объект Profile
    {
        if (!File.Exists(filePath)) // Если файла нет...
        {
            throw new FileNotFoundException( // ...выбрасываем исключение «файл не найден»
                "Файл профиля не найден.", // Текст сообщения об ошибке
                filePath // Путь к файлу, который не найден
            ); // Конец создания исключения
        }

        string[] lines = File.ReadAllLines(filePath); // Читаем все строки файла в массив

        if (lines.Length < 3) // Нужно минимум 3 строки: имя, фамилия, год рождения
        {
            throw new InvalidDataException( // Выбрасываем исключение «некорректные данные»
                "Файл профиля содержит недостаточно данных." // Текст сообщения об ошибке
            ); // Конец создания исключения
        }

        if (!int.TryParse( // Пробуем преобразовать третью строку в число
            lines[2], // Строка с годом рождения
            NumberStyles.Integer, // Допускаем целое число (с возможным знаком и пробелами по краям)
            CultureInfo.InvariantCulture, // Используем неизменяемую культуру, чтобы результат не зависел от региона
            out int birthYear)) // При успехе результат попадает в переменную birthYear
        {
            throw new InvalidDataException( // Если преобразование не удалось — выбрасываем исключение
                "Некорректный год рождения в файле профиля." // Текст сообщения об ошибке
            ); // Конец создания исключения
        }

        return new Profile(lines[0], lines[1], birthYear); // Создаём и возвращаем профиль: имя, фамилия, год
    } // Конец метода LoadProfile

    // Сохраняет список задач в CSV-файл.
    public static void SaveTodos(TodoList todos, string filePath) // Принимает список задач и путь к файлу
    {
        List<string> lines = new List<string>(); // Список строк будущего файла

        // Добавляем заголовок таблицы.
        lines.Add("Index;Text;IsDone;LastUpdate"); // Первая строка CSV — названия столбцов, разделённые «;»

                // Перебираем все задачи списка через итератор.
        int index = 0; // Индекс в файле начинается с 0.

        foreach (TodoItem item in todos)
        {
            // Заменяем реальные переносы строк специальной последовательностью.
            string text = item.Text
                .Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Replace("\n", "\\n");

            // Удваиваем кавычки для корректной записи в CSV.
            text = text.Replace("\"", "\"\"");

            // Формируем строку: индекс, текст, статус, дата изменения.
            string line =
                $"{index};\"{text}\";{item.IsDone.ToString().ToLowerInvariant()};{item.LastUpdate.ToString("O", CultureInfo.InvariantCulture)}";

            lines.Add(line);
            index++;
        }

        // Записываем заголовок и задачи в файл.
        File.WriteAllLines(filePath, lines); // Записываем все строки в файл (перезаписывая его)
    } // Конец метода SaveTodos

    // Загружает задачи из CSV-файла.
    public static TodoList LoadTodos(string filePath) // Принимает путь к файлу, возвращает заполненный список задач
    {
        if (!File.Exists(filePath)) // Если файла нет...
        {
            throw new FileNotFoundException( // ...выбрасываем исключение «файл не найден»
                "Файл задач не найден.", // Текст сообщения об ошибке
                filePath // Путь к файлу, который не найден
            ); // Конец создания исключения
        }

        string[] lines = File.ReadAllLines(filePath); // Читаем все строки файла
        TodoList todos = new TodoList(); // Создаём пустой список, который будем заполнять

        // Если файл пустой, возвращаем пустой список.
        if (lines.Length == 0) // Если в файле нет ни одной строки
        {
            return todos; // Возвращаем пустой список
        }

        // Проверяем заголовок CSV-файла.
        if (lines[0] != "Index;Text;IsDone;LastUpdate") // Первая строка должна точно совпадать с ожидаемым заголовком
        {
            throw new InvalidDataException( // Иначе файл не нашего формата — выбрасываем исключение
                "Некорректный заголовок файла todo.csv." // Текст сообщения об ошибке
            ); // Конец создания исключения
        }

        // Обрабатываем каждую строку с задачей.
        for (int i = 1; i < lines.Length; i++) // Начинаем с индекса 1, пропуская строку заголовка
        {
            if (string.IsNullOrWhiteSpace(lines[i])) // Если строка пустая или состоит из пробелов
            {
                continue; // Пропускаем её и переходим к следующей
            }

            List<string> fields = SplitCsvLine(lines[i]); // Разбиваем строку на поля с учётом кавычек

            if (fields.Count != 4) // Должно быть ровно 4 поля: индекс, текст, статус, дата
            {
                throw new InvalidDataException( // Иначе формат строки нарушен
                    $"Некорректный формат строки {i + 1}." // Сообщение с номером строки файла (i + 1, т.к. счёт с 0)
                ); // Конец создания исключения
            }

            if (!int.TryParse( // Проверяем, что индекс — целое число
                fields[0], // Поле с индексом
                NumberStyles.Integer, // Допускаем целое число
                CultureInfo.InvariantCulture, // Неизменяемая культура
                out _)) // Само значение не нужно, поэтому отбрасываем его (_)
            {
                throw new InvalidDataException( // Если не число — выбрасываем исключение
                    $"Некорректный индекс в строке {i + 1}." // Сообщение с номером строки
                ); // Конец создания исключения
            }

            // Восстанавливаем переносы строк в тексте задачи.
            string text = fields[1].Replace("\\n", "\n"); // Заменяем текст «\n» обратно на настоящий перенос строки

            // Проверяем статус выполнения задачи.
            if (!bool.TryParse(fields[2], out bool isDone)) // Пробуем превратить «true»/«false» в bool
            {
                throw new InvalidDataException( // Если не получилось — выбрасываем исключение
                    $"Некорректный статус в строке {i + 1}." // Сообщение с номером строки
                ); // Конец создания исключения
            }

            // Восстанавливаем дату последнего изменения.
            if (!DateTime.TryParse( // Пробуем превратить текст в дату и время
                fields[3], // Поле с датой
                CultureInfo.InvariantCulture, // Неизменяемая культура для разбора
                DateTimeStyles.RoundtripKind, // Сохраняем вид времени (UTC/локальное), как при записи в формате "O"
                out DateTime lastUpdate)) // При успехе результат попадает в lastUpdate
            {
                throw new InvalidDataException( // Если дата некорректна — выбрасываем исключение
                    $"Некорректная дата в строке {i + 1}." // Сообщение с номером строки
                ); // Конец создания исключения
            }

            // Создаём задачу со всеми сохранёнными данными.
            TodoItem item = new TodoItem(text, isDone, lastUpdate); // Конструктор с текстом, статусом и датой (не текущей)

            // Добавляем задачу в список.
            todos.Add(item); // Кладём задачу в список
        } // Конец цикла по строкам файла

        return todos; // Возвращаем заполненный список задач
    } // Конец метода LoadTodos

    // Разбирает одну строку CSV с учётом кавычек и разделителей внутри текста.
    private static List<string> SplitCsvLine(string line) // Приватный вспомогательный метод: строка → список полей
    {
        List<string> fields = new List<string>(); // Список готовых полей
        StringBuilder field = new StringBuilder(); // Накопитель символов текущего поля
        bool insideQuotes = false; // Флаг: сейчас мы внутри кавычек или нет

        for (int i = 0; i < line.Length; i++) // Проходим по строке посимвольно
        {
            char current = line[i]; // Текущий символ

            if (current == '"') // Если встретилась кавычка
            {
                // Две кавычки внутри поля обозначают одну кавычку в тексте.
                if (insideQuotes && // Мы внутри кавычек
                    i + 1 < line.Length && // И следующий символ существует
                    line[i + 1] == '"') // И следующий символ тоже кавычка
                {
                    field.Append('"'); // Добавляем в поле одну настоящую кавычку
                    i++; // Пропускаем вторую кавычку из пары
                }
                else // Одиночная кавычка
                {
                    insideQuotes = !insideQuotes; // Переключаем режим: вошли в кавычки или вышли из них
                }
            }
            else if (current == ';' && !insideQuotes) // Если это «;» вне кавычек
            {
                // Разделитель завершает поле только за пределами кавычек.
                fields.Add(field.ToString()); // Сохраняем накопленное поле в список
                field.Clear(); // Очищаем накопитель для следующего поля
            }
            else // Любой другой символ (или «;» внутри кавычек)
            {
                field.Append(current); // Просто добавляем символ в текущее поле
            }
        } // Конец цикла по символам

        if (insideQuotes) // Если после прохода по строке кавычки остались открытыми
        {
            throw new InvalidDataException( // Строка некорректна — выбрасываем исключение
                "В CSV-файле обнаружены незакрытые кавычки." // Текст сообщения об ошибке
            ); // Конец создания исключения
        }

        // Добавляем последнее поле строки.
        fields.Add(field.ToString()); // После последнего поля нет «;», поэтому добавляем его вручную

        return fields; // Возвращаем список полей
    } // Конец метода SplitCsvLine
} // Конец класса FileManager