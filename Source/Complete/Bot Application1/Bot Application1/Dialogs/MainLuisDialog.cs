using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    [LuisModel("<appid>", "<key>")]
    public class MainLuisDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Sorry, I did not understand '{result.Query}'.");

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("whois")]
        public async Task WhoIs(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;

            var name = result.Entities.FirstOrDefault()?.Entity;
            await context.Forward(new SpeakersDialog(name), AfterTaskDone, message);
        }

        [LuisIntent("info")]
        public async Task Info(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;

        }

        [LuisIntent("program")]
        public async Task Program(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;

        }

        private async Task AfterTaskDone(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }
    }
}