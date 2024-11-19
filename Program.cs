using System;

namespace VoiceAssistant
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Assistant vocal prêt. Parlez...");
            while (true)
            {
                string command = SpeechRecognition.RecognizeSpeech();
                if (!string.IsNullOrEmpty(command))
                {
                    Console.WriteLine($"Commande reçue : {command}");
                    string response = AssistantLogic.ProcessCommand(command);
                    TextToSpeech.Speak(response);
                }
            }
        }
    }
}
