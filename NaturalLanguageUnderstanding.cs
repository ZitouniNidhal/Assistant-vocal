using System;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;

namespace VoiceAssistant
{
    public static class NaturalLanguageUnderstanding
    {
        private static readonly LuisRuntimeClient _luisClient;

        static NaturalLanguageUnderstanding()
        {
            _luisClient = new LuisRuntimeClient(new ApiKeyServiceClientCredentials("YOUR_LUIS_API_KEY"))
            {
                Endpoint = "https://YOUR_LUIS_REGION.api.cognitive.microsoft.com/"
            };
        }

        public static (string Intent, Dictionary<string, string>) AnalyzeCommand(string command)
        {
            var request = new PredictionRequest { Query = command };
            var response = _luisClient.Prediction.GetSlotPredictionAsync("YOUR_LUIS_APP_ID", "YOUR_LUIS_SLOT_NAME", request).Result;

            var intent = response.TopIntent?.IntentName;
            var entities = new Dictionary<string, string>();

            foreach (var entity in response.Entities)
            {
                entities.Add(entity.Type, entity.Entity);
            }

            return (intent, entities);
        }
    }
}