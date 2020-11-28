using System;
using LodFinals.DependencyServices;

namespace LodFinals.iOS.DependencyServices
{
    public class PlatformFileManagerService : IPlatformFileManagerService
    {
        public PlatformFileManagerService()
        {
        }

        public string DownloadDirectory => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }
}
