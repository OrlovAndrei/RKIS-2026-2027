using System;

namespace TodoListApp
{
    public class Profile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthYear { get; set; }

        public Profile(string firstName, string lastName, string birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public string GetInfo()
        {
            if (int.TryParse(BirthYear, out int year))
            {
                int age = DateTime.Now.Year - year;
                return $"{FirstName} {LastName}, возраст {age}";
            }
            return $"{FirstName} {LastName}, {BirthYear}";
        }
    }
}