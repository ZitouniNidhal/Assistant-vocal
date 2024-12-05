using System;
using System.Diagnostics;

namespace VoiceAssistant
{
    public static class JuliaIntegration
    {
        public static string RunJuliaScript(string scriptPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "julia",  // Assurez-vous que Julia est installé et accessible dans le PATH
                Arguments = scriptPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();

                // Lire la sortie standard
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                
                process.WaitForExit();

                // Vérifiez s'il y a des erreurs
                if (!string.IsNullOrEmpty(error))
                {
                    throw new InvalidOperationException($"Erreur lors de l'exécution du script Julia : {error}");
                }

                return output;
            }
        }
    }
}