using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoiceAssistant
{
    public class MainForm : Form
    {
        private TextBox   outputBox;

        private Button startButton;

        public MainForm()
        {
            // Configure la fenêtre
            this.Text = "Assistant Vocal";
            this.Width = 400;
            this.Height = 300;

            // Zone de texte pour afficher les commandes et réponses
            outputBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Top,
                Height = 200,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Bouton pour démarrer la reconnaissance vocale
            startButton = new Button
            {
                Text = "Démarrer l'assistant",
                Dock = DockStyle.Bottom
            };
            startButton.Click += StartButton_Click;

            // Ajout des contrôles à la fenêtre
            this.Controls.Add(outputBox);
            this.Controls.Add(startButton);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            outputBox.AppendText("Assistant vocal prêt. Parlez...\r\n");

            // Simule une boucle d'écoute (à remplacer par un vrai modèle vocal)
            while (true)
            {
                string command = SpeechRecognition.RecognizeSpeech(); // Appel à ta méthode de reconnaissance vocale
                if (!string.IsNullOrEmpty(command))
                {
                    outputBox.AppendText($"Commande reçue : {command}\r\n");
                    string response = AssistantLogic.ProcessCommand(command);
                    outputBox.AppendText($"Réponse : {response}\r\n");

                    TextToSpeech.Speak(response); // Parle la réponse
                }

                // Simule une pause pour ne pas bloquer complètement l'interface
                Application.DoEvents();
            }
        }
    }
}
