namespace Blogrolling.Utilities;

public static class ConfigHelper
{
    public static string GetConnectionString()
    {
        var userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (Directory.Exists(userHome))
        {
            var file = Path.Combine(userHome, ".config", "blogrolling", "connectionString");
            return File.ReadAllText(file);
        }

        return string.Empty;
    }
}