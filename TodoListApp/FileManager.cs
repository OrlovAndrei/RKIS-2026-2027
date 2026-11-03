namespace TodoListApp
{
    internal static class FileManager
    {
        private const string DataDirectoryName = "data";
        private const string ProfileFileName = "profile.txt";
        private const string TodoFileName = "todo.csv";
        private const int ProfileLinesCount = 3;

        public static string DataDirectoryPath => Path.Combine(AppContext.BaseDirectory, DataDirectoryName);

        public static string ProfileFilePath => Path.Combine(DataDirectoryPath, ProfileFileName);

        public static string TodoFilePath => Path.Combine(DataDirectoryPath, TodoFileName);

        public static void EnsureDataDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        public static void SaveProfile(Profile profile, string filePath)
        {
            if (profile == null)
            {
                return;
            }

            string[] lines = { profile.FirstName, profile.LastName, profile.BirthYear.ToString() };
            File.WriteAllLines(filePath, lines);
        }

        public static Profile? LoadProfile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < ProfileLinesCount || !int.TryParse(lines[2], out int birthYear))
            {
                return null;
            }

            return new Profile(lines[0], lines[1], birthYear);
        }
    }
}