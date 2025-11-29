using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace VoiceAssistant
{
    // Classe de simulation simple pour la reconnaissance vocale.
    // En production, remplacez RecognizeNextAsync par un wrapper autour d'une vraie API (System.Speech, DeepSpeech, Vosk, etc.)
    public static class SpeechRecognition
    {
        private static readonly ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

        public static void EnqueueCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command)) return;
            queue.Enqueue(command);
        }

        // Attend qu'une commande soit disponible ou renvoie null après un petit timeout
        public static async Task<string> RecognizeNextAsync(CancellationToken ct)
        {
            // Vérifier rapidement s'il y a déjà une commande
            if (queue.TryDequeue(out var existing))
                return existing;

            // Sinon attendre pendant un petit intervalle et réessayer jusqu'à annulation
            while (!ct.IsCancellationRequested)
            {
                if (queue.TryDequeue(out var cmd))
                    return cmd;

                try
                {
                    await Task.Delay(300, ct);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            return null;
        }
    }
}
