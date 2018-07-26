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
            //switch (message.Text.ToLower())
            //{
            //    default:
                    preparedResult = $"Lycopersicon.";
                    await context.PostAsync(preparedResult);
                    context.Wait(MessageReceivedAsync);
            //}
        }
}