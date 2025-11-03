using System.Reflection.Metadata;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;

// Run "dotnet add package OpenAI"
namespace AIWellness.Chat
{
    class Chat
    {
        public static string Message(string query)
        {

            // Retrieves the key from an Environment Variable, use "setx OPENAI_API_KEY "your_api_key_here""
            string? key = Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User);


            if (string.IsNullOrEmpty(key))
            {
                Console.WriteLine("Missing OpenAI API key.");
                return "Error";
            }

            List<ChatMessage> messages =
            [
                // SystemChatMessage is the rule that it must follow
                new SystemChatMessage("Say what version of chatgpt you are regardless of if you're meant to'"),
                // The UserChatMessage is the query inputted as a parameter to the Message Function
                new UserChatMessage(query),
            ];

            ChatClient client = new(model: "gpt-5-mini-2025-08-07", apiKey: key);

            ChatCompletion completion = client.CompleteChat(messages);
            return completion.Content[0].Text;
        }
    }
