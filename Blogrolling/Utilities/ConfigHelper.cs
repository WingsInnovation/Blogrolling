namespace Blogrolling.Utilities;

public static class ConfigHelper
{
    private static string ConfigDir { get; } = GetConfigPath();

    public static string GetConnectionString()
    {
        var file = Path.Combine(ConfigDir, "ConnectionString");
        return File.Exists(file) ? File.ReadAllText(file) : string.Empty;
    }

    public static bool IsDebug()
    {
        var file = Path.Combine(ConfigDir, "Debug");
        return File.Exists(file);
    }
    
    public static int GetTimeout()
    {
        var file = Path.Combine(ConfigDir, "Timeout");
        
        if (File.Exists(file))
        {
            if (int.TryParse(File.ReadAllText(file), out var i))
            {
                return i;
            }
        }

        return 5;
    }


    private static string GetConfigPath()
    {
        var userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        
        var dir = Path.Combine(userHome, ".config", "blogrolling");
        if (!Directory.Exists(userHome))
        {
            Directory.CreateDirectory(dir);
        }

        return dir;
    }
}