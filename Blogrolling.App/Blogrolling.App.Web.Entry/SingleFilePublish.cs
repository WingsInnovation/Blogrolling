using Furion;
using System.Reflection;

namespace Blogrolling.App.Web.Entry;

public class SingleFilePublish : ISingleFilePublish
{
    public Assembly[] IncludeAssemblies()
    {
        return Array.Empty<Assembly>();
    }

    public string[] IncludeAssemblyNames()
    {
        return new[]
        {
            "Blogrolling.App.Application",
            "Blogrolling.App.Core",
            "Blogrolling.App.EntityFramework.Core",
            "Blogrolling.App.Web.Core"
        };
    }
}