using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        protected uint _lastRoll = UInt32.MaxValue;
        protected uint count = 1;
        protected bool helloSaid;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            Contract.Ensures(Contract.Result<Task>() != null);
            var message = await argument;
            var messageLower = message.Text.ToLower();

            String preparedResult = null;
            switch (messageLower)
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
                    // Use of weak ranom numbers provider
                    Random random = new Random();
                    _lastRoll = (uint)random.Next(1, 20);
                    preparedResult = "";
                    if (_lastRoll == 1)
                    {
                        preparedResult = $"You rolled {_lastRoll}. Critical Failure!";
                    }
                    else if (_lastRoll == 20)
                    {
                        preparedResult = $"You rolled {_lastRoll}. Critical Success!";
                    }
                    else
                    {
                        preparedResult = $"You rolled {_lastRoll}.";
                    }

                    await context.PostAsync(preparedResult);
                    context.Wait(MessageReceivedAsync);
                    break;

                case "roll d100":
                    // Use some secure random numbers provider
                    RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
                    var byteArray = new byte[4];
                    _lastRoll = BitConverter.ToUInt32(byteArray, 0);
                    preparedResult = "";
                    if (_lastRoll == 1)
                    {
                        preparedResult = $"You rolled {_lastRoll}. Critical Failure!";
                    }
                    else if (_lastRoll == 20)
                    {
                        preparedResult = $"You rolled {_lastRoll}. Critical Success!";
                    }
                    else
                    {
                        preparedResult = $"You rolled {_lastRoll}.";
                    }

                    await context.PostAsync(preparedResult);
                    context.Wait(MessageReceivedAsync);
                    break;

                case "what was my last roll?":
                    preparedResult = _lastRoll != UInt32.MaxValue ? $"You rolled {_lastRoll} in last roll." : $"You didn't made a single roll yet.";
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
                count = 1;
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
            helloSaid = false;
            var confirm = await argument;
            if (confirm)
            {
                helloSaid = true;
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