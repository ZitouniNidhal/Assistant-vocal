using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;

namespace VoiceAssistant
{
    public static class NaturalLanguageUnderstanding
    {
        private static  LUISRuntimeClient _luisClient;

        static NaturalLanguageUnderstanding()
        {
            _luisClient = new LUISRuntimeClient(new ApiKeyServiceClientCredentials("YOUR_LUIS_API_KEY"))
            {
                Endpoint = "https://YOUR_LUIS_REGION.api.cognitive.microsoft.com/"
            };
        }

        public static (string Intent, Dictionary<string, string> Entities) AnalyzeCommand(string command)
        {
            var request = new PredictionRequest { Query = command };
            var response = _luisClient.Prediction.GetSlotPredictionAsync("1455887587887", Guid.Parse("7488954666"), request).Result;

            string intent = response.Prediction.TopIntent;
            var entities = new Dictionary<string, string>();

            foreach (var entity in response.Prediction.Entities)
            {
                var entityValue = (Newtonsoft.Json.Linq.JObject)entity.Value;
                entities[entity.Key] = entityValue["entity"].ToString(); // Use indexer to add entities
            }

            return (intent, entities);
        }

    }
}