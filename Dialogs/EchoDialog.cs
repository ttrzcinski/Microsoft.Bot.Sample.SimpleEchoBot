using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;


namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        protected int lastRoll = -1;
        protected int count = 1;
        protected bool helloSaid = false;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            switch (message.Text.ToLower())
            {
                case "hello":
                    PromptDialog.Confirm(
                        context,
                        AfterHelloAsync,
                        "Hello. Are you well Today?",
                        "What?!",
                        promptStyle: PromptStyle.Auto);
                    break;

                case "reset":
                    PromptDialog.Confirm(
                        context,
                        AfterResetAsync,
                        "Are you sure you want to reset the count?",
                        "Didn't get that!",
                        promptStyle: PromptStyle.Auto);
                    break;

                case "roll d20":
                    Random random = new Random();
                    this.lastRoll = random.Next(1, 20);
                    //this.count++;
                    String preparedResult = "";
                    if (this.lastRoll == 1)
                    {
                        preparedResult = $"You rolled {this.lastRoll}. Critical Failure!";
                    }
                    else if (this.lastRoll == 20)
                    {
                        preparedResult = $"You rolled {this.lastRoll}. Critical Success!";
                    }
                    else
                    {
                        preparedResult = $"You rolled {this.lastRoll}.";
                    }

                    await context.PostAsync(preparedResult);
                    context.Wait(MessageReceivedAsync);
                    break;

                case "what was my last roll?":
                    preparedResult = $"You rolled {this.lastRoll} in last roll.";
                    await context.PostAsync(preparedResult);
                    context.Wait(MessageReceivedAsync);
                    break;

                default:
                    await context.PostAsync($"{this.count++}: You said {message.Text}");
                    context.Wait(MessageReceivedAsync);
                    break;
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(MessageReceivedAsync);
        }

        public async Task AfterHelloAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            this.helloSaid = false;
            var confirm = await argument;
            if (confirm)
            {
                this.helloSaid = true;
                await context.PostAsync("That's nice..");
            }
            else
            {
                await context.PostAsync("Hello? Are You there?");
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}