using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Bot_Application1.Services;
using Microsoft.Bot.Connector;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class SpeakersDialog : IDialog<object>
    {
        private string _entity = null;

        public SpeakersDialog() { }

        public SpeakersDialog(string entity)
        {
            _entity = entity;
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var mess = await result;

            if (mess.Text.ToLower().StartsWith("nikdo") || mess.Text.ToLower() == "n" || mess.Text.ToLower() == "back")
            {
                context.Done(true);
                return;
            }

            var qna = new QnaService("<knowledgebase id>", "<key>");

            var answer = await qna.QnAMakerQueryAsync(_entity == null ? mess.Text : _entity);
            if (answer != null)
            {
                await context.PostAsync(answer);
            }
            else
            {
                await context.PostAsync("Toho neznám...");
            }

            _entity = null;

            await context.PostAsync("Kdo dál?");
            context.Wait(MessageReceivedAsync);
        }
    }
}