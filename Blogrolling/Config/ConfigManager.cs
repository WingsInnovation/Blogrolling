using System.Text;
using dotenv.net;

namespace Blogrolling.Config;

public class ConfigManager
{
    public static readonly string ConfigName = "blogrolling.cfg"; 
    
    private IDictionary<string, string> Env { get; }
    
    public ConfigManager()
    {
        Env = DotEnv.Fluent()
            .WithEncoding(Encoding.UTF8)
            .WithProbeForEnv()
            .WithEnvFiles(GetConfigPath())
            .WithOverwriteExistingVars()
            .WithTrimValues()
            .WithoutExceptions()
            .Read();
    }
    
    public string GetConnectionString()
    {
        return Env["BLOGROLLING_DATABASE_CONNECTION_STRING"];
    }

    public bool IsDebug()
    {
        return "true".Equals(Env["BLOGROLLING_DEBUG"]);
    }
    
    public int GetTimeout()
    {
        var timeout = Env["FETCH_TIMEOUT"];
        return string.IsNullOrWhiteSpace(timeout) ? 5 : int.Parse(timeout);
    }

    private string[] GetConfigPath()
    {
        var userHome = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", ConfigName);
        var runDir = Path.Combine(Environment.CurrentDirectory, ConfigName);
        return [userHome, runDir];
    }
}