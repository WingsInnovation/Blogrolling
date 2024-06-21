namespace Blogrolling.Updater;

public enum EnumExitCode
{
    Success = 0,
    Error = 1,
    
    InvalidUrl = -1,
    CantReach = -2,
    AlreadyExists = -3,
    SourceNotFound = -4,
}