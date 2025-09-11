namespace FCG_MS_Game_Library.Application.Helpers;
public class EnviromentHelper
{
    public static bool IsProductionOrDevelopment()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (string.IsNullOrEmpty(env))
            return false;

        return env.Equals("Development", StringComparison.OrdinalIgnoreCase) || env.Equals("Production", StringComparison.OrdinalIgnoreCase);
    }
}
