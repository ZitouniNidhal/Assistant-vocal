using System;
using System.Diagnostics;
using System.IO;

public class PythonIntegration
{
    public string RunPythonScript(string scriptPath, string arguments)
    {
        // Vérifie si le fichier de script existe
        if (!File.Exists(scriptPath))
        {
            throw new FileNotFoundException("Le fichier de script Python n'a pas été trouvé.", scriptPath);
        }

        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
            FileName = "python",  // Assurez-vous que Python est bien installé et accessible dans le PATH
            Arguments = $"{scriptPath} {arguments}",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true, // Redirige la sortie d'erreur
            CreateNoWindow = true
        };

        using (Process process = new Process())
        {
            process.StartInfo = startInfo;

            try
            {
                process.Start();

                // Capture la sortie standard et la sortie d'erreur
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                
                process.WaitForExit();

                // Vérifie s'il y a des erreurs
                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception($"Erreur lors de l'exécution du script Python : {error}");
                }

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de l'exécution du script Python.", ex);
            }
        }
    }
}