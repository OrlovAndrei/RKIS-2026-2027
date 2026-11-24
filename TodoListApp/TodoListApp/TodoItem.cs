using System;

// TodoItem — одна задача списка дел (Лекция 6 «Введение в ООП»).
// Теперь данные и методы собраны в одном классе.
class TodoItem
{
    private const int MaxTextLength = 30; // копия константы из Program

    // Поля 

    private string _text;         
    private bool _isDone;         
    private DateTime _lastUpdate; 

    // Свойства

    public string Text { get { return _text; } }
    public bool IsDone { get { return _isDone; } }
    public DateTime LastUpdate { get { return _lastUpdate; } }

    // Конструктор: вызывается при new TodoItem("текст")

    public TodoItem(string text)
    {
        _text = text;
        _isDone = false;          
        _lastUpdate = DateTime.Now;
    }

    // Методы

    // Отмечает задачу выполненной и обновляет дату
    public void MarkDone()
    {
        _isDone = true;
        _lastUpdate = DateTime.Now;
    }

    // Меняет текст задачи и обновляет дату
    public void UpdateText(string newText)
    {
        _text = newText;
        _lastUpdate = DateTime.Now;
    }

    // Краткий текст для колонки «Текст» в таблице view
    public string GetShortText()
    {
        string flat = _text.Replace('\n', ' ');

        if (flat.Length <= MaxTextLength)
            return flat;

        return flat.Substring(0, MaxTextLength) + "...";
    }

    // Краткая информация: «текст30 [статус] (дата)»
    public string GetShortInfo()
    {
        string status = _isDone ? "выполнена" : "не выполнена";
        return $"{GetShortText()} [{status}] ({_lastUpdate.ToString("dd.MM.yyyy")})";
    }

    // Полное описание
    public string GetFullInfo()
    {
        string status = _isDone ? "выполнена" : "не выполнена";
        return $"Текст задачи:\n{_text}\nСтатус: {status}\nДата обновления: {_lastUpdate.ToString("dd.MM.yyyy")}";
    }
}