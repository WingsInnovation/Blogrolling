using System.Text;
using dotenv.net;

namespace Blogrolling.Config;

public class ConfigManager
{
    private IDictionary<string, string> Env { get; }
    
    public ConfigManager()
    {
        DotEnv.Fluent()
            .WithEncoding(Encoding.UTF8)
            .WithEnvFiles(GetConfigPath())
            .WithProbeForEnv()
            .WithOverwriteExistingVars()
            .WithTrimValues()
            .WithoutExceptions()
            .Load();

        Env = DotEnv.Read();
    }
    
    public string GetConnectionString()
    {
        var host = Env["BLOGROLLING_MYSQL_HOST"];
        var port = Env["BLOGROLLING_MYSQL_PORT"];
        var user = Env["BLOGROLLING_MYSQL_USER"];
        var pass = Env["BLOGROLLING_MYSQL_PASS"];
        var name = Env["BLOGROLLING_MYSQL_NAME"];

        return $"Server={host};Port={port};Uid={user};Pwd={pass};Database={name};";
    }

    public bool IsDebug()
    {
        return "true".Equals(Env["BLOGROLLING_DEBUG"]);
    }
    
    public int GetTimeout()
    {
        var timeout = Env["BLOGROLLING_TIMEOUT"];
        return string.IsNullOrWhiteSpace(timeout) ? 5 : int.Parse(timeout);
    }

    private string GetConfigPath()
    {
        var userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        
        var dir = Path.Combine(userHome, ".config");
        if (!Directory.Exists(userHome))
        {
            Directory.CreateDirectory(dir);
        }

        return Path.Combine(dir, "blogrolling.cfg");
    }
}