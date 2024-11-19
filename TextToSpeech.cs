using System;
using Microsoft.CognitiveServices.Speech;

namespace VoiceAssistant
{
    public static class TextToSpeech
    {
        public static void Speak(string text)
        {
            var config = SpeechConfig.FromSubscription("YOUR_AZURE_SUBSCRIPTION_KEY", "YOUR_REGION");
            using var synthesizer = new SpeechSynthesizer(config);
            
            var result = synthesizer.SpeakTextAsync(text).Result;
            if (result.Reason != ResultReason.SynthesizingAudioCompleted)
            {
                Console.WriteLine("Erreur lors de la synthèse vocale.");
            }
        }
    }
}
