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

        public Task Play(string filePath)
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

            return Task.FromResult(true);
        }
    }
}
