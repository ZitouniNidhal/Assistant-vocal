using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoiceAssistant
{
    public class MainForm : Form
    {
        private TextBox outputBox;
        private Button startButton;
        private Button stopButton;
        private Button simulateCommandButton;
        private ListBox historyList;
        private Label statusLabel;

        private CancellationTokenSource cts;
        private Task recognitionTask;

        public MainForm()
        {
            // Configure la fenêtre
            this.Text = "Assistant Vocal";
            this.Width = 600;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Zone de texte pour afficher les logs
            outputBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Top,
                Height = 160,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Liste d'historique des commandes / réponses
            historyList = new ListBox
            {
                Dock = DockStyle.Fill
            };

            // Panel pour boutons
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40
            };

            startButton = new Button
            {
                Text = "Démarrer",
                Left = 10,
                Width = 100,
                Top = 6
            };
            startButton.Click += StartButton_Click;

            stopButton = new Button
            {
                Text = "Arrêter",
                Left = 120,
                Width = 100,
                Top = 6,
                Enabled = false
            };
            stopButton.Click += StopButton_Click;

            simulateCommandButton = new Button
            {
                Text = "Simuler commande",
                Left = 230,
                Width = 140,
                Top = 6
            };
            simulateCommandButton.Click += SimulateCommandButton_Click;

            statusLabel = new Label
            {
                Text = "Statut : Inactif",
                Left = 380,
                Top = 10,
                AutoSize = true
            };

            bottomPanel.Controls.Add(startButton);
            bottomPanel.Controls.Add(stopButton);
            bottomPanel.Controls.Add(simulateCommandButton);
            bottomPanel.Controls.Add(statusLabel);

            // Ajout des contrôles à la fenêtre
            this.Controls.Add(historyList);
            this.Controls.Add(outputBox);
            this.Controls.Add(bottomPanel);
        }

        private void Log(string text)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => Log(text)));
                return;
            }

            outputBox.AppendText($"{DateTime.Now:HH:mm:ss} - {text}\r\n");
            historyList.Items.Insert(0, $"{DateTime.Now:HH:mm:ss} - {text}");
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            if (cts != null)
                return;

            cts = new CancellationTokenSource();
            startButton.Enabled = false;
            stopButton.Enabled = true;
            statusLabel.Text = "Statut : En écoute";
            Log("Assistant vocal prêt. En écoute...");

            // Start recognition loop in background
            recognitionTask = Task.Run(() => RecognitionLoopAsync(cts.Token));
            await Task.CompletedTask;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (cts == null)
                return;

            cts.Cancel();
            stopButton.Enabled = false;
            startButton.Enabled = true;
            statusLabel.Text = "Statut : Arrêté";
            Log("Arrêt de l'assistant demandé.");
            cts = null;
        }

        private void SimulateCommandButton_Click(object sender, EventArgs e)
        {
            // Affiche une petite boîte pour taper une commande manuelle (utile pour tests sans reconnaissance vocale)
            string command = Prompt.ShowDialog("Tapez une commande à simuler :", "Simuler commande");
            if (!string.IsNullOrWhiteSpace(command))
            {
                SpeechRecognition.EnqueueCommand(command.Trim());
                Log($"Commande simulée injectée : {command}");
            }
        }

        private async Task RecognitionLoopAsync(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    // Attendre la prochaine commande (ou null si timeout)
                    string command = await SpeechRecognition.RecognizeNextAsync(ct);
                    if (ct.IsCancellationRequested) break;

                    if (!string.IsNullOrEmpty(command))
                    {
                        Log($"Commande reçue : {command}");

                        bool shouldStop;
                        string response = AssistantLogic.ProcessCommand(command, out shouldStop);
                        Log($"Réponse : {response}");

                        // Essayer de parler la réponse (implémentation simple, peut être remplacée)
                        try
                        {
                            TextToSpeech.Speak(response);
                        }
                        catch (Exception ex)
                        {
                            Log($"Erreur TTS : {ex.Message}");
                        }

                        if (shouldStop)
                        {
                            // si la commande demande d'arrêter, on annule proprement
                            this.BeginInvoke(new Action(() =>
                            {
                                StopButton_Click(this, EventArgs.Empty);
                            }));
                            break;
                        }
                    }
                    else
                    {
                        // aucune commande — petite pause
                        await Task.Delay(250, ct);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // normal lors de l'annulation
                Log("Boucle de reconnaissance annulée.");
            }
            catch (Exception ex)
            {
                Log("Erreur dans la boucle de reconnaissance : " + ex.Message);
            }
            finally
            {
                // s'assurer que l'UI est dans un état cohérent
                this.BeginInvoke(new Action(() =>
                {
                    statusLabel.Text = "Statut : Inactif";
                    startButton.Enabled = true;
                    stopButton.Enabled = false;
                }));
            }
        }
    }
}
