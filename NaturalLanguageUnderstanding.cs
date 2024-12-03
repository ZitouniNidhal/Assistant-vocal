using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Rest;

namespace VoiceAssistant
{
        public static class NaturalLanguageUnderstanding
    {
        private static LUISRuntimeClient? _luisClient;
        private const string LuisAppId = "YOUR_LUIS_APP_ID"; // Store your LUIS App ID here
        private const string LuisApiKey = "YOUR_LUIS_API_KEY";
        private const string LuisEndpoint = "https://YOUR_LUIS_REGION.api.cognitive.microsoft.com/";
    
        static NaturalLanguageUnderstanding()
        {
            InitializeLuisClient();
        }
    
        private static void InitializeLuisClient()
        {
            try
            {
                _luisClient = new LUISRuntimeClient(new ApiKeyServiceClientCredentials(LuisApiKey))
                {
                    Endpoint = LuisEndpoint
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize LUIS client: {ex.Message}");
            }
        }
    
        public static (string Intent, Dictionary<string, string> Entities) AnalyzeCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                Console.WriteLine("Command cannot be null or empty.");
                return (string.Empty, new Dictionary<string, string>());
            }
    
            try
            {
                var request = new PredictionRequest { Query = command };
                if (_luisClient == null)
                {
                    throw new InvalidOperationException("LUIS client is not initialized.");
                }
                var response = _luisClient.Prediction.GetSlotPredictionAsync(Guid.Parse(LuisAppId), "YOUR_SLOT_NAME", request).Result;
    
                string intent = response.Prediction.TopIntent;
                var entities = new Dictionary<string, string>();
    
                foreach (var entity in response.Prediction.Entities)
                {
                    var entityValue = (Newtonsoft.Json.Linq.JObject)entity.Value;
                    entities[entity.Key] = entityValue["entity"].ToString();
                }
    
                return (intent, entities);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error analyzing command: {ex.Message}");
                return (string.Empty, new Dictionary<string, string>());
            }
        }
    
        public static void LogPredictionResult(string command, string intent, Dictionary<string, string> entities)
        {
            Console.WriteLine($"Command: {command}");
            Console.WriteLine($"Predicted Intent: {intent}");
            Console.WriteLine("Entities:");
            foreach (var entity in entities)
            {
                Console.WriteLine($" - {entity.Key}: {entity.Value}");
            }
        }
    }
}