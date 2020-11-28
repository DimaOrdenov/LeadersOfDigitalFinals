using System;
using AVFoundation;
using Foundation;
using LodFinals.DependencyServices;
using NoTryCatch.Core.Services;
using Speech;

namespace LodFinals.iOS.DependencyServices
{
    public class PlatformSpeechToTextService : IPlatformSpeechToTextService
    {
        private readonly IDebuggerService _debuggerService;

        private readonly AVAudioEngine _audioEngine;
        private readonly SFSpeechRecognizer _speechRecognizer;
        private SFSpeechAudioBufferRecognitionRequest _recognitionRequest;
        private SFSpeechRecognitionTask _recognitionTask;
        private AVAudioSession _aVAudioSession;
        private string _recognizedString;

        public event EventHandler<string> ErrorOccured;

        public event EventHandler<string> PartialResultsReceived;

        public event EventHandler<string> Finished;

        public PlatformSpeechToTextService(IDebuggerService debuggerService)
        {
            _debuggerService = debuggerService;

            _audioEngine = new AVAudioEngine();
            _speechRecognizer = new SFSpeechRecognizer();
        }

        public void StartSpeechToText()
        {
            if (_audioEngine.Running)
            {
                StopRecordingAndRecognition();
            }

            StartRecordingAndRecognizing();
        }

        public void StopSpeechToText()
        {
            if (_audioEngine.Running)
            {
                StopRecordingAndRecognition();
            }
        }

        private void StartRecordingAndRecognizing()
        {
            _recognitionTask?.Cancel();
            _recognitionTask = null;

            _aVAudioSession = AVAudioSession.SharedInstance();
            NSError nsError;

            nsError = _aVAudioSession.SetCategory(AVAudioSessionCategory.Record, AVAudioSessionCategoryOptions.DuckOthers);

            _aVAudioSession.SetMode(AVAudioSession.ModeSpokenAudio, out nsError);

            nsError = _aVAudioSession.SetActive(true, AVAudioSessionSetActiveOptions.NotifyOthersOnDeactivation);

            _recognitionRequest = new SFSpeechAudioBufferRecognitionRequest();
            _recognitionRequest.ShouldReportPartialResults = true;

            var inputNode = _audioEngine.InputNode;

            if (inputNode == null)
            {
                throw new Exception();
            }

            inputNode.InstallTapOnBus(
                0,
                1024,
                inputNode.GetBusOutputFormat(0),
                (buffer, when) =>
                {
                    _recognitionRequest?.Append(buffer);
                });

            _audioEngine.Prepare();
            _audioEngine.StartAndReturnError(out nsError);

            _recognitionTask = _speechRecognizer.GetRecognitionTask(
                _recognitionRequest,
                (result, error) =>
                {
                    if (result != null)
                    {
                        _recognizedString = result.BestTranscription.FormattedString;

                        PartialResultsReceived?.Invoke(this, _recognizedString);
                    }

                    if (error != null)
                    {
                        _debuggerService.Log(error.LocalizedDescription);

                        ErrorOccured?.Invoke(this, error.LocalizedDescription);

                        return;
                    }
                });
        }

        private void StopRecordingAndRecognition()
        {
            Finished?.Invoke(this, _recognizedString);

            _recognitionRequest.EndAudio();
            _recognitionRequest = null;

            _audioEngine.Stop();
            _audioEngine.InputNode.RemoveTapOnBus(0);

            _recognitionTask?.Cancel();
            _recognitionTask = null;

            _aVAudioSession.SetActive(false);
            _aVAudioSession = null;
        }
    }
}
