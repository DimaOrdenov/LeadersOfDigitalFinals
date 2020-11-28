using System;
using LodFinals.DependencyServices;
using Plugin.CurrentActivity;

namespace LodFinals.Droid.DependencyServices
{
    public class PlatformFileManagerService : IPlatformFileManagerService
    {
        public PlatformFileManagerService()
        {
        }

        public string DownloadDirectory => CrossCurrentActivity.Current.Activity.ApplicationContext.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
    }
}
