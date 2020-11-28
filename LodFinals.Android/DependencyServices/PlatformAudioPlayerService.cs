using System;
using System.Threading.Tasks;
using Android.Media;
using LodFinals.DependencyServices;
using NoTryCatch.Core.Services;
using Plugin.CurrentActivity;

namespace LodFinals.Droid.DependencyServices
{
    public class PlatformAudioPlayerService : IPlatformAudioPlayerService
    {
        private readonly IDebuggerService _debuggerService;
        private readonly MediaPlayer _mediaPlayer;

        public PlatformAudioPlayerService(IDebuggerService debuggerService)
        {
            _debuggerService = debuggerService;
            _mediaPlayer = new MediaPlayer();
        }

        public async Task Play(string filePath)
        {
            try
            {
                if (_mediaPlayer.IsPlaying)
                {
                    _mediaPlayer.Stop();
                }

                _mediaPlayer.Reset();

                Java.IO.File javaFile = new Java.IO.File(filePath);
                javaFile.SetReadable(true);

                Android.Net.Uri fileUri = Android.Support.V4.Content.FileProvider.GetUriForFile(
                    CrossCurrentActivity.Current.Activity.ApplicationContext,
                    $"{CrossCurrentActivity.Current.Activity.ApplicationContext.PackageName}.fileprovider",
                    javaFile);

                _mediaPlayer.SetAudioAttributes(
                    new AudioAttributes.Builder()
                        .SetContentType(AudioContentType.Music)
                        .SetUsage(AudioUsageKind.Media)
                        .Build());

                await _mediaPlayer.SetDataSourceAsync(CrossCurrentActivity.Current.Activity.ApplicationContext, fileUri);
                _mediaPlayer.Prepare();
                _mediaPlayer.Start();
            }
            catch (Exception ex)
            {
                _debuggerService.Log(ex);
            }
        }
    }
}
