using GroqApiLibrary;
using System.Text.Json.Nodes;

namespace QueryBuilderApi.Services
{
    public class GroqService
    {
        private readonly GroqApiClient? _groqClient;
        private readonly bool _isConfigured;

        public GroqService(IConfiguration configuration)
        {
            var apiKey = configuration["Groq:ApiKey"];
            
            // No lanzar excepción en el constructor
            if (string.IsNullOrEmpty(apiKey))
            {
                _isConfigured = false;
                _groqClient = null;
            }
            else
            {
                _isConfigured = true;
                _groqClient = new GroqApiClient(apiKey);
            }
        }

        public async Task<string> GenerateSqlFromDescription(string databaseSchema, string userDescription)
        {
            // Lanzar excepción solo cuando se intente usar el servicio
            if (!_isConfigured || _groqClient == null)
                return "ERROR: Groq API Key not configured";

            var messages = new JsonArray
            {
                new JsonObject
                {
                    ["role"] = "system",
                    ["content"] = GroqPrompts.SQL_GENERATION_SYSTEM
                },
                new JsonObject
                {
                    ["role"] = "user",
                    ["content"] = GroqPrompts.GetSqlGenerationUserPrompt(databaseSchema, userDescription)
                }
            };

            var request = new JsonObject
            {
                ["model"] = GroqModels.Llama31_8B, 
                ["temperature"] = 0.2,
                ["max_completion_tokens"] = 500,
                ["messages"] = messages
            };

            try
            {
                var result = await _groqClient.CreateChatCompletionAsync(request);
                var generatedSql = result?["choices"]?[0]?["message"]?["content"]?.ToString() ?? "ERROR: Empty response";
                return generatedSql.Trim();
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }
    }
}