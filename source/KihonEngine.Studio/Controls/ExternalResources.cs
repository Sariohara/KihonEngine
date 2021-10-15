
using System.Diagnostics;

namespace KihonEngine.Studio.Controls
{
    public static class ExternalResources
    {
        public const string DocumentationUrl = "https://github.com/nico65535/KihonEngine/blob/main/doc/kihon-engine-studio.md";

        public static void Navigate(string url)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}
