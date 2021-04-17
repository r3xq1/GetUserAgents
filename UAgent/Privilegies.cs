namespace UAgent
{
    using System.Security.Principal;

    public static class Privilegies
    {
        public static bool IsAdmin
        {
            get
            {
                try
                {
                    using var identity = WindowsIdentity.GetCurrent();
                    if (null != identity)
                    {
                        var principal = new WindowsPrincipal(identity);
                        return principal.IsInRole(WindowsBuiltInRole.Administrator);
                    }
                }
                catch { }
                return false;
            }
        }
    }
}