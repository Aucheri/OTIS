using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;

namespace OTIS.OTIS
{
	class AWS : OTIS
	{
		private const string ModelId = "amazon.nova-micro-v1:0";

		public override async Task<string?> Chat(Request request)
		{
			var config = new AmazonBedrockRuntimeConfig
			{
				RegionEndpoint = RegionEndpoint.EUWest2 // Use your region,
			};

			using var bedrockClient = new AmazonBedrockRuntimeClient(config);

			List<Message> messages = [];

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

			messages.Add(new Message
			{
				Role = "user",
				Content = [new ContentBlock { Text = request.Message }]
			});


			var response = await GenerateConversationAsync(bedrockClient, ModelId, messages);

			// Add the response message if it is not null
			if (response.Output?.Message != null)
			{
				return response.Output.Message.Content[0].Text;
			}

			return "NO RESPONSE";
		}
		
		/// <summary>
		/// Generates a conversation by sending messages to the Amazon Bedrock model.
		/// </summary>
		/// <param name="bedrockClient">The Bedrock Runtime client.</param>
		/// <param name="modelId">The model ID to use.</param>
		/// <param name="messages">The messages to send to the model.</param>
		/// <returns>The response from the Bedrock Runtime.</returns>
		private async Task<ConverseResponse> GenerateConversationAsync(AmazonBedrockRuntimeClient bedrockClient, string modelId, List<Message> messages)
		{
			// Set inference configuration
			var inferenceConfig = new InferenceConfiguration
			{
				Temperature = 0.5f
			};

			// Prepare system content blocks
			var systemContentBlocks = new List<SystemContentBlock> { new() { Text = SystemPrompt } };

			// Create the request
			var request = new ConverseRequest
			{
				ModelId = modelId,
				Messages = messages,
				System = systemContentBlocks,
				InferenceConfig = inferenceConfig,
			};

			// Send the request and return the response
			return await bedrockClient.ConverseAsync(request);
		}
	}
}