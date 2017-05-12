using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class InfoDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var card = new ThumbnailCard()
            {
                Title = "Info",
                Images = new List<CardImage>()
                {
                    new CardImage("http://devdays.sk/images/location.png")
                },
                Text = "Konference se koná 13. 5. na Fakultě informatiky a informačních technologií STU v Bratislavě."
            };

            var reply = context.MakeMessage();
            reply.Attachments.Add(card.ToAttachment());

            await context.PostAsync(reply);

            context.Done(true);
        }
    }
}