// Profile — данные пользователя TodoList (Лекция 6 «Введение в ООП»).

class Profile
{
    // Поля

    private string _firstName; 
    private string _lastName;   
    private int _birthYear;     

    // Свойства 

    public string FirstName { get { return _firstName; } }
    public string LastName { get { return _lastName; } }
    public int BirthYear { get { return _birthYear; } }

    // Конструктор: вызывается при new Profile(...) и сразу задаёт все данные 

    public Profile(string firstName, string lastName, int birthYear)
    {
        _firstName = firstName;
        _lastName = lastName;
        _birthYear = birthYear;
    }
        
    public string GetInfo()
    {
        int age = DateTime.Now.Year - _birthYear;
        return $"{_firstName} {_lastName}, возраст {age}";
    }
}
