using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;

namespace VoiceAssistant
{
    public static class AssistantLogic
    {
        // Traite la commande textuelle et retourne une réponse.
        // Le booléen out indique si l'assistant doit s'arrêter.
        public static string ProcessCommand(string command, out bool shouldStop)
        {
            shouldStop = false;
            if (string.IsNullOrWhiteSpace(command))
                return "Je n'ai rien entendu.";

            string c = command.Trim().ToLowerInvariant();

            // Commandes simples
            if (c == "quelle heure" || c.Contains("heure"))
            {
                return $"Il est {DateTime.Now:HH:mm}.";
            }

            if (c.StartsWith("ouvrir ") || c.StartsWith("ouvre "))
            {
                // essayer d'extraire une url ou un nom de site
                // ex: "ouvrir github.com" ou "ouvrir https://google.com"
                string target = command.Substring(command.IndexOf(' ') + 1).Trim();
                string url = ToUrl(target);
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                    return $"Ouverture de {url}";
                }
                catch (Exception ex)
                {
                    return $"Impossible d'ouvrir {url} : {ex.Message}";
                }
            }

            if (c == "aide" || c == "help")
            {
                return "Commandes disponibles : 'quelle heure', 'ouvrir <site>', 'aide', 'arrêter', 'répéter <texte>'";
            }

            if (c.StartsWith("répéter ") || c.StartsWith("repeter "))
            {
                string toRepeat = command.Substring(command.IndexOf(' ') + 1);
                return toRepeat;
            }

            if (c == "arrêter" || c == "arreter" || c == "stop" || c == "au revoir")
            {
                shouldStop = true;
                return "D'accord, j'arrête. À bientôt !";
            }

            // Par défaut, si on reconnaît un mot clef
            if (c.Contains("météo"))
            {
                return "Je ne suis pas encore connecté à un service météo, mais je peux l'ajouter.";
            }

            // commande inconnue
            return "Commande inconnue. Dites 'aide' pour la liste des commandes.";
        }

        private static string ToUrl(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "https://www.google.com";

            input = input.Trim();
            // si c'est déjà une URL
            if (Regex.IsMatch(input, @"^https?://", RegexOptions.IgnoreCase))
                return input;

            // si ressemble à domain.com
            if (Regex.IsMatch(input, @"^[\w\-]+(\.[\w\-]+)+", RegexOptions.IgnoreCase))
            {
                if (!input.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    return "https://" + input;
                return input;
            }

            // sinon recherche Google
            string query = HttpUtility.UrlEncode(input);
            return $"https://www.google.com/search?q={query}";
        }
    }
}
