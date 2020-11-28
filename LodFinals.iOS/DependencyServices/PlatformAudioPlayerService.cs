using System;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using LodFinals.DependencyServices;

namespace LodFinals.iOS.DependencyServices
{
    public class PlatformAudioPlayerService : IPlatformAudioPlayerService
    {
        private AVAudioPlayer _player;

        public PlatformAudioPlayerService()
        {
        }

        public event EventHandler Finished;

        public Task PlayAsync(string filePath)
        {
            if (_player != null)
            {
                _player.Stop();
                _player.Dispose();
                _player = null;
            }

            _player = AVAudioPlayer.FromData(NSData.FromFile(filePath));

            _player.PrepareToPlay();
            _player.Play();

            _player.FinishedPlaying += (sender, e) => Finished?.Invoke(this, EventArgs.Empty);

            return Task.FromResult(true);
        }

        public void Stop()
        {
            if (_player != null)
            {
                _player.Stop();
                _player.Dispose();
                _player = null;
            }
        }
    }
}
