using System.Diagnostics;

public class PythonIntegration
{
    public string RunPythonScript(string scriptPath, string arguments)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
            FileName = "python",  // Assure-toi que Python est bien installé et accessible dans le PATH
            Arguments = $"{scriptPath} {arguments}",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        Process process = new Process();
        process.StartInfo = startInfo;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        return output;
    }
}
