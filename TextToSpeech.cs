using System;
using System.Diagnostics;

namespace VoiceAssistant
{
    public static class TextToSpeech
    {
        // Implémentation simple : sur Windows on utilise System.Speech.Synthesis si disponible.
        // Si l'assembly n'est pas présent, on tombe en fallback qui n'empêche pas l'exécution.
        public static void Speak(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            try
            {
                // Utiliser System.Speech.Synthesis si disponible (Windows .NET Framework)
                Type synthType = Type.GetType("System.Speech.Synthesis.SpeechSynthesizer, System.Speech");
                if (synthType != null)
                {
                    dynamic synth = Activator.CreateInstance(synthType);
                    synth.Speak(text);
                    (synth as IDisposable)?.Dispose();
                    return;
                }
            }
            catch
            {
                // ignore and fallback
            }

            // Fallback : ouvrir une notification son / log (ici on écrit simplement dans le debug)
            Debug.WriteLine("TTS (fallback): " + text);
        }
    }
}
