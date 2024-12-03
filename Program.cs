using System;

namespace VoiceAssistant
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Assistant vocal prêt. Parlez...");

            // Initialize the speech recognition process
            SpeechRecognition.RecognizeSpeech();
            bool exit = false;
            while (!exit)
            while (true)
            {
                // Assuming RecognizeSpeech returns a command
                string command = SpeechRecognition.RecognizeSpeech();
                if (!string.IsNullOrEmpty(command))
                {
                    Console.WriteLine($"Commande reçue : {command}");
                    string response = AssistantLogic.ProcessCommand(command);
                    if (command.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        exit = true;
                    }
                }
            }
                }
            }
        }

