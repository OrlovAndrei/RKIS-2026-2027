using System;

namespace TodoListApp
{
    internal class Profile
    {
        private string _firstName;
        private string _lastName;
        private int _birthYear;

        public string FirstName
        {
            get { return _firstName; }
            private set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            private set { _lastName = value; }
        }

        public int BirthYear
        {
            get { return _birthYear; }
            private set { _birthYear = value; }
        }

        public Profile(string firstName, string lastName, int birthYear)
        {
            this._firstName = firstName;
            this._lastName = lastName;
            this._birthYear = birthYear;
        }

        public string GetInfo()
        {
            int currentYear = DateTime.Now.Year;
            int age = currentYear - _birthYear;
            return $"{_firstName} {_lastName}, возраст - {age}";
        }
    }
}
