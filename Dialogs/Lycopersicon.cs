using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Diagnostics.Contracts;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    /// <summary>
    /// Simple bot instance, with whom speaker can play famous game of Lycopersicon.
    /// </summary>
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

            preparedResult = AnswerTo(message.Text);
            await context.PostAsync(preparedResult);
            context.Wait(MessageReceivedAsync);
        }

        /// <summary>
        /// Processes input and returns proper response. Hides here whole Q'n'A logic and references to knowledge.
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public String AnswerTo(string question)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            if (String.IsNullOrEmpty(question))
            {
                return "Come on.. Write me something..";
            }

            var questionLower = question.ToLower();

            string answer = $"Hmm..";
            switch (questionLower)
            {
                case "what is the answer to life, the universe and everything?":
                    answer = "42, read the book..";
                    break;

                default:
                    answer = $"Lycopersicon.";
                    break;
            }

            return answer;
        }
    }
}