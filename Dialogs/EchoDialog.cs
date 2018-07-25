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
        protected int count = 1;
        protected bool helloSaid = false;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            switch (message.Text)
            {
                case "hello":
                    PromptDialog.Confirm(
                        context,
                        AfterHelloAsync,
                        "Hello. How Are you Today?",
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