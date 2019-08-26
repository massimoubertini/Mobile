using Urho;
using Urho.Desktop;

namespace SamplyGame.Desktop
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DesktopUrhoInitializer.AssetsDirectory = @"../../Assets";
            new SamplyGame().Run();
        }
    }
}