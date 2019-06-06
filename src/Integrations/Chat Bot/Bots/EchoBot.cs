// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace Microsoft.Bot.Builder.EchoBot
{
    public class EchoBot : ActivityHandler
    {
        private const string _unhandledMessage = "I'm sorry, I'm not sure I understand what you're saying. Please try again. I'm best at helping reschedule an appointment or canceling an appointment.";
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(turnContext.Activity.Text))
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(_unhandledMessage));
            }

            var timeMatches = Regex.Matches(turnContext.Activity.Text, "[0-9]{1,2}\\s*:\\s*[0-9]{2}(\\s*(AM)|(PM))?", RegexOptions.IgnoreCase);
            if (timeMatches.Count > 0)
            {
                var timeSpecified = DateTime.Parse(timeMatches[0].Value).ToString("hh:mmtt");
                await turnContext.SendActivityAsync(MessageFactory.Text($"OK, we'll let you your technician know that you'll be arriving at {timeSpecified}."), cancellationToken);
            }
            else if (turnContext.Activity.Text.Contains("cancel", StringComparison.InvariantCultureIgnoreCase))
            {
                await turnContext.SendActivityAsync(MessageFactory.Text($"OK, we've canceled your appointment and let you your technician know."), cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(_unhandledMessage));
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hi {member.Name}, sorry to hear you are running late. What time do you estimate you'll arrive now?"), cancellationToken);
                }
            }
        }
    }
}
