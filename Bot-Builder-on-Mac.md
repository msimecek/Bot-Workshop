# Bot Builder on Mac

> Running the Bot Builder on pure ASP.NET Core on the Mac is currently not supported by the library. However the .NET Core support is under development and you can get to it and try it.

Clone the [Microsoft/BotBuilder repo](https://github.com/Microsoft/BotBuilder/).

```
git clone https://github.com/Microsoft/BotBuilder.git
```

Checkout branch `develop`.

```
cd BotBuilder
git checkout develop
```

Open **Microsoft.Bot.Builder.VS2017.sln** in [Visual Studio for Mac](https://www.visualstudio.com/vs/visual-studio-mac/).

Rebuild the whole solution. Ignore errors and manually build AspNetCore projects:

* Framework/Microsoft.Bot.Connector.AspNetCore.Mvc
* Framework/Microsoft.Bot.Connector.AspNetCore
* Framework/Microsoft.Bot.Connector.Common

(To make sure the necessary DLLs were created.)

Create a new ASP.NET Core project in Visual Studio.

```
File > New Solution > .NET Core > ASP.NET Core Web API
```

Reference two projects from Git by right clicking on the solution and selecting **Add > Add Existing Project...**

Select:

* Microsoft.Bot.Connector.AspNetCore.csproj
* Microsoft.Bot.Connector.Common.csproj

Add dependencies to your bot project:

```
Dependencies > Edit References... > All > Microsoft..AspNetCore, Microsoft..Common
```

Go to **Startup.cs**.

Add to the **ConfigureServices()** method:

```c#
services.UseBotConnector();
```

Add to the Configure() method:

```c#
app.UseBotAuthentication(new StaticCredentialProvider(
    Configuration.GetSection(MicrosoftAppCredentials.MicrosoftAppIdKey)?.Value,
	Configuration.GetSection(MicrosoftAppCredentials.MicrosoftAppPasswordKey)?.Value)
);
```

Add using to the top:

```c#
using Microsoft.Bot.Connector;
```

Go to **appsettings.json**.

Add:

```javascript
  "MicrosoftAppId": "",
  "MicrosoftAppPassword": ""
```

So that it looks like this:

```javascript
{
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "MicrosoftAppId": "",
  "MicrosoftAppPassword": ""
}
```

Add new Controller to the project. Call it **MessagesController**.

Replace contents with this:

```c#
using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Bot.Sample.AspNetCore.Echo.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IConfigurationRoot configuration;

        public MessagesController(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        [Authorize(Roles = "Bot")]
        // POST api/values
        [HttpPost]
        public virtual async Task<OkResult> Post([FromBody]Activity activity)
        {
            var client = new ConnectorClient(new Uri(activity.ServiceUrl));
            var reply = activity.CreateReply();
            if (activity.Type == ActivityTypes.Message)
            {
                reply.Text = $"echo: {activity.Text}";
            }
            else
            {
                reply.Text = $"activity type: {activity.Type}";
            }
            await client.Conversations.ReplyToActivityAsync(reply);
            return Ok();
        }
    }
}
```

Rebuild solution and you should be ready to go.