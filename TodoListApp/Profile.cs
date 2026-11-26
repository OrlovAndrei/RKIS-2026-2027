using System;

namespace TodoListApp
{
    class Profile
    {
        public string FirstName { get; }
        public string LastName { get; }
        public int BirthYear { get; }

        public Profile(string firstName, string lastName, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public string GetInfo()
        {
            int age = DateTime.Now.Year - BirthYear;

            return $"{FirstName} {LastName}, возраст {age}";
        }
    }
}