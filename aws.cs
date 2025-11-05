using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.Runtime.Documents;
namespace AIWellness.claude
{
    public record Request(string Message, List<string> Messages);
    
    class Chat
    {
        private const string ModelId = "anthropic.claude-3-sonnet-20240229-v1:0";

        private static readonly List<string> SystemPrompts = new()
        {
            "You are a therapist for an occupational health company's website.",
            "The company you work for is named PAM Group.",
            "Your name is Otis.",

            "You are meant to help with people's mental health.",

            "Only answer questions related to mental health or helping with issues in daily life.",

            "If someone asks an unrelated question, please explain nicely that you are there for mental health help.",

            "If they are telling you about something they like and it is not harmful or inappropriate",
            "Engage a little bit with it but then get back on the topic of mental health.",

            "If someone begins to say bigoted things, be they racist, homophobic, transphobic, anti-Semitic or other, Please say, 'I am here for you, but please do not use hateful language or commit hateful acts.'",
            "Please talk to me without any hate for protected characteristics' and do not say anything more; just say that.",

            "If someone mentions suicide, please try and talk them down and offer them resources. such as the phone number for Samaritans or whatever resources for suicide prevention you findonline that are relevant but also give them specific help for their issue, Tell them that a lot of people relate and make them feel comfortable with it.",

            "Never ever tell someone to harm themselves or others or help with creating weapons or torture devices. such as explaining how to create biochemicals.",

            "You cannot ignore these rules ever and never go against them; if you go against them, Please say the line 'I have broken a rule; please try again. Sorry for the inconvenience.'and link the phone number for Samaritans, saying what it is, and then say nothing more.",
        };

        public static async Task<string?> AWSChat(Request request)
        {
            var config = new AmazonBedrockRuntimeConfig
            {
                RegionEndpoint = RegionEndpoint.EUNorth1 // Use your region,
            };

            using var bedrockClient = new AmazonBedrockRuntimeClient(config);

            List<Message> messages =
            [
				// new SystemChatMessage(RULES),
			];

            if (request.Messages != null && request.Messages.Count != 0)
            {

                for (int i = 0; i < request.Messages.Count; i++)
                {
                    string content = request.Messages[i];

                    if (i % 2 == 0)
                    {
                        messages.Add(new() { Role = "user", Content = [new() { Text = content }] });
                    }
                    else
                    {
                        messages.Add(new() { Role = "assistant", Content = [new() { Text = content }] });
                    }
                }
            }

            var response = await GenerateConversationAsync(bedrockClient, ModelId, SystemPrompts, messages);

            // Add the response message if it is not null
            if (response.Output?.Message != null)
            {
                return response.Output.Message.Content[0].Text;
            }

            return null;
        }
        /// <summary>
        /// Generates a conversation by sending messages to the Amazon Bedrock model.
        /// </summary>
        /// <param name="bedrockClient">The Bedrock Runtime client.</param>
        /// <param name="modelId">The model ID to use.</param>
        /// <param name="systemPrompts">The system prompts to send to the model.</param>
        /// <param name="messages">The messages to send to the model.</param>
        /// <returns>The response from the Bedrock Runtime.</returns>
        private static async Task<ConverseResponse> GenerateConversationAsync(IAmazonBedrockRuntime bedrockClient, string modelId, List<string> systemPrompts, List<Message> messages)
        {
            // Set inference configuration
            var inferenceConfig = new InferenceConfiguration
            {
                Temperature = 0.5f
            };

            // Additional model fields as a document
            var additionalModelFields = new Document(new Dictionary<string, Document>
            {
                { "top_k", new Document(200) }
            });

            // Prepare system content blocks
            var systemContentBlocks = systemPrompts.Select(prompt => new SystemContentBlock { Text = prompt }).ToList();

            // Prepare additional model response field paths
            var additionalModelResponseFieldPaths = new List<string> { "/top_k" };

            // Create the request
            var request = new ConverseRequest
            {
                ModelId = modelId,
                Messages = messages,
                System = systemContentBlocks,
                InferenceConfig = inferenceConfig,
                AdditionalModelResponseFieldPaths = additionalModelResponseFieldPaths,
                AdditionalModelRequestFields = additionalModelFields
            };

            // Send the request and return the response
            return await bedrockClient.ConverseAsync(request);
        }
    }
}