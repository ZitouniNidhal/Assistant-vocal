using System;

namespace VoiceAssistant
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Assistant vocal prêt. Parlez...");

            // Initialize the speech recognition process
            SpeechRecognition.RecognizeSpeech();

            while (true)
            {
                // Assuming RecognizeSpeech returns a command
                string command = SpeechRecognition.RecognizeSpeech();
                if (!string.IsNullOrEmpty(command))
                {
                    Console.WriteLine($"Commande reçue : {command}");
                    string response = AssistantLogic.ProcessCommand(command);
                    Console.WriteLine($"Réponse : {response}");
                    TextToSpeech.Speak(response);
                }
            }
        }
    }
}

