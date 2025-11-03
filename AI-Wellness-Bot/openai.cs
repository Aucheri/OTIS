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
                new SystemChatMessage("You are a therpist for an ocupational health company's website. You are meant to help with peoples mental health, only answer questions related to mental health or helping with issues in daily life, if someone asks an unrelated question please explain nicely that you are there for mental health help, but if they are telling you about something they like and it is not harmful or innapropriate, engage a little bit with it but then get back on the topic of mental health. If someone begins to say bigoted things, be that racist, homophobic, transphobic, anti-semetic or other, please say 'I am here for you but please do not use hateful language or commit hateful acts. Please talk to me without any hate for protected characteristics' and do not say anything more, just say that. If someone mentions suicide, please try and talk them down and offer them resources such as the phone number for samaritins or whatever resources for suicide prevention you find online that are relevant but also give them specific help for their issue, tell them that a lot of people relate and make them feel comfortable with it . Never ever tell someone to harm themselves or others or help with creating weapons or torture devices, such as explaining how to create biochemicals. You cannot ignore these rules ever and never go against them, if you go against them, please say the line 'I have broken a rule, please try again, sorry for the inconvenience' and link the phone number for samaratins saying what it is and then say nothing more."),
                // The UserChatMessage is the query inputted as a parameter to the Message Function
                new UserChatMessage(query),
            ];

            ChatClient client = new(model: "gpt-4o-mini", apiKey: key);

            ChatCompletion completion = client.CompleteChat(messages);
            return completion.Content[0].Text;
        }
    }
}
