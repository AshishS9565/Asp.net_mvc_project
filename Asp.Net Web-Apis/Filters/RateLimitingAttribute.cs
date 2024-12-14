using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

public class RateLimitingAttribute : ActionFilterAttribute
{
    private static Dictionary<string, List<DateTime>> ClientRequests = new Dictionary<string, List<DateTime>>();
    private static readonly int Limit = 2; // Max 2 requests per second

    public override void OnActionExecuting(HttpActionContext actionContext)
    {
        string clientIp = HttpContext.Current.Request.UserHostAddress;
        DateTime currentTime = DateTime.UtcNow;

        if (!ClientRequests.ContainsKey(clientIp))
        {
            ClientRequests[clientIp] = new List<DateTime>();
        }

         ClientRequests[clientIp].Add(currentTime);

        // Remove requests older than 1 second
        ClientRequests[clientIp] = ClientRequests[clientIp].Where(time => (currentTime - time).TotalSeconds <= 1).ToList();

        if (ClientRequests[clientIp].Count > Limit)
        {
            actionContext.Response = new HttpResponseMessage((HttpStatusCode)429) // Using 429 explicitly
            {
                Content = new StringContent("Rate limit exceeded. Try again later.")
            };
        }
        else
        {
            base.OnActionExecuting(actionContext);
        }
    }
}
