using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;


namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class Lycopersicon : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            String preparedResult;

            preparedResult = answerTo(message.Text);
            await context.PostAsync(preparedResult);
            context.Wait(MessageReceivedAsync);
        }

        public String answerTo(string question)
        {
            if (String.IsNullOrEmpty(question))
            {
                return "Come on..";
            }

            string answer = $"Lycopersicon.";
            switch (question.ToLower())
            {
                case "what is the answer to life, the universe and everything?":
                    answer = "42, read the book..";
                    break;
            }

            return answer;
        }
    }
}