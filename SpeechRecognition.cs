using System;
using Microsoft.CognitiveServices.Speech;

namespace VoiceAssistant
{
    public static class SpeechRecognition
    {
        public static string RecognizeSpeech()
        {
            var config = SpeechConfig.FromSubscription("YOUR_AZURE_SUBSCRIPTION_KEY", "YOUR_REGION");
            using var recognizer = new SpeechRecognizer(config);
            
            var result = recognizer.RecognizeOnceAsync().Result;
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                return result.Text;
            }
            else
            {
                Console.WriteLine("Erreur lors de la reconnaissance vocale.");
                return null;
            }
        }
    }
}
