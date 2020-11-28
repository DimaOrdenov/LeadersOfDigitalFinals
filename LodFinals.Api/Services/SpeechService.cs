using Google.Cloud.TextToSpeech.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LodFinals.Api.Services
{
    public class SpeechService
    {
        public async Task<string> TextToSpeech(string text, string language)
        {
            var builder = new TextToSpeechClientBuilder()
            {
                JsonCredentials = System.IO.File.ReadAllText("google.json")
            };

            var client = builder.Build();

            // Set the text input to be synthesized.
            SynthesisInput input = new SynthesisInput
            {
                Text = text
            };

            // Build the voice request, select the language code ("en-US"),
            // and the SSML voice gender ("neutral").
            VoiceSelectionParams voice = new VoiceSelectionParams
            {
                LanguageCode = language,
                SsmlGender = SsmlVoiceGender.Unspecified,
            };

            // Select the type of audio file you want returned.
            AudioConfig config = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3
            };

            // Perform the Text-to-Speech request, passing the text input
            // with the selected voice parameters and audio file type
            var response = await client.SynthesizeSpeechAsync(new SynthesizeSpeechRequest
            {
                Input = input,
                Voice = voice,
                AudioConfig = config
            });

            return response.AudioContent.ToBase64();
        }
    }
}
