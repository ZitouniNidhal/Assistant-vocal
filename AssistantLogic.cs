namespace VoiceAssistant
{
    public static class AssistantLogic
    {
        public static string ProcessCommand(string command)
        {
            if (command.ToLower().Contains("météo"))
            {
                return "Je vais vérifier la météo pour vous.";
            }
            else if (command.ToLower().Contains("heure"))
            {
                return $"Il est actuellement {DateTime.Now.ToShortTimeString()}.";
            }
            else
            {
                return "Je n'ai pas compris votre commande.";
            }
        }
    }
}
