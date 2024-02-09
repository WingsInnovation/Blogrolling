using Blogrolling.Config;

namespace Blogrolling;

public class Blogrolling
{
    private static Blogrolling? Instance { get; set; } = null;
    
    public static Blogrolling GetInstance()
    {
        return Instance ??= new Blogrolling();
    }

    public ConfigManager Config { get; }
    
    public Blogrolling()
    {
        Config = new ConfigManager();
    }
}