using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using LodFinals.DependencyServices;
using NoTryCatch.Core.Services;
using Plugin.CurrentActivity;

namespace LodFinals.Droid.DependencyServices
{
    public class PlatformSpeechToTextService : Java.Lang.Object, IPlatformSpeechToTextService, IRecognitionListener
    {
        private readonly IDebuggerService _debuggerService;

        private SpeechRecognizer _speechRecognizer;

        public PlatformSpeechToTextService(IDebuggerService debuggerService)
        {
            _debuggerService = debuggerService;
        }

        public event EventHandler<string> ErrorOccured;

        public event EventHandler<string> PartialResultsReceived;

        public event EventHandler<string> Finished;

        public void StartSpeechToText()
        {
            if (!CrossCurrentActivity.Current.AppContext.PackageManager.HasSystemFeature(Android.Content.PM.PackageManager.FeatureMicrophone))
            {
                throw new PlatformNotSupportedException("No mic found");
            }

            _speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(CrossCurrentActivity.Current.Activity);
            _speechRecognizer.SetRecognitionListener(this);

            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);

            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraPartialResults, true);
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now");
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, "en-US");

            _speechRecognizer.StartListening(voiceIntent);
        }

        public void StopSpeechToText()
        {
            _speechRecognizer?.Cancel();
            _speechRecognizer?.StopListening();
            _speechRecognizer?.Destroy();
        }

        public void OnBeginningOfSpeech()
        {
            //throw new NotImplementedException();
        }

        public void OnBufferReceived(byte[] buffer)
        {
            //throw new NotImplementedException();
        }

        public void OnEndOfSpeech()
        {
            //throw new NotImplementedException();
        }

        public void OnError([GeneratedEnum] SpeechRecognizerError error)
        {
            _debuggerService.Log($"Android native error: {error}");

            StopSpeechToText();

            ErrorOccured?.Invoke(this, error.ToString());
        }

        public void OnEvent(int eventType, Bundle @params)
        {
            //throw new NotImplementedException();
        }

        public void OnPartialResults(Bundle partialResults)
        {
            var matches = partialResults?.GetStringArrayList(SpeechRecognizer.ResultsRecognition);

            if (matches?.Count > 0)
            {
                string textInput = matches[0];

                PartialResultsReceived?.Invoke(this, textInput);
            }
        }

        public void OnReadyForSpeech(Bundle @params)
        {
            //throw new NotImplementedException();
        }

        public void OnResults(Bundle results)
        {
            var matches = results?.GetStringArrayList(SpeechRecognizer.ResultsRecognition);

            if (matches?.Count > 0)
            {
                string textInput = matches[0];

                Finished?.Invoke(this, textInput);
            }

            StopSpeechToText();
        }

        public void OnRmsChanged(float rmsdB)
        {
            //throw new NotImplementedException();
        }
    }
}
