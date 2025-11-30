using System.Drawing;
using System.Windows.Forms;

namespace VoiceAssistant
{
    // Petite boîte de dialogue pour récupérer une chaîne (simule InputBox)
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent
            };
            Label textLabel = new Label() { Left = 10, Top = 10, Text = text, AutoSize = true };
            TextBox inputBox = new TextBox() { Left = 10, Top = 30, Width = 460 };
            Button confirmation = new Button() { Text = "OK", Left = 300, Width = 80, Top = 60, DialogResult = DialogResult.OK };
            Button cancel = new Button() { Text = "Annuler", Left = 390, Width = 80, Top = 60, DialogResult = DialogResult.Cancel };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;

            return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : null;
        }
    }
}
