using OpenAI.Chat;

namespace OTIS.OTIS;

class OpenAI : OTIS
{
	public override async Task<string?> Chat(Request request)
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
			new SystemChatMessage(SystemPrompt),
		];

		if (request.Messages != null && request.Messages.Count != 0)
		{
			
			for (int i = 0; i < request.Messages.Count; i++)
			{
				string content = request.Messages[i];
				
				if (i % 2 == 0)
				{
					messages.Add(new UserChatMessage(content));
				}
				else
				{
					messages.Add(new AssistantChatMessage(content));
				}
			}
		}

		messages.Add(new UserChatMessage(request.Message));

		ChatClient client = new(model: "gpt-4o-mini", apiKey: key);

		ChatCompletion completion = client.CompleteChat(messages);
		return completion.Content[0].Text;
	}
}
