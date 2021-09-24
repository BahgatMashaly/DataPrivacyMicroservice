using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace DataPrivacyMicroservice.DataPrivacyManagementLayer.Authorization
{
    public class BasicAuthenticationAttribute: AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,"No user name or password supplied!");
            }
            else
            {
                string authenticationInfo = actionContext.Request.Headers.Authorization.Parameter;
                string decodedAuthenticationToken = Encoding.UTF8.GetString( Convert.FromBase64String(authenticationInfo));
                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                string username = usernamePasswordArray[0];
                string password = usernamePasswordArray[1];

                if (ActorValidate.Login(username, password))
                {
                    ActorDetails actordetails = ActorValidate.ActorDetails(username, password);


                    // var xx = actorData.ActorRoles.Select(x => x.roleID);
                    string[] actorRoles = actordetails.actorRols;
                    Thread.CurrentPrincipal = new GenericPrincipal( new GenericIdentity(actordetails.actor.id.ToString()), actorRoles);

                    IPrincipal principal = new GenericPrincipal(new GenericIdentity(actordetails.actor.id.ToString()), actorRoles);
                    Thread.CurrentPrincipal = principal;
                    if (HttpContext.Current.User != null)
                    {
                        HttpContext.Current.User = principal;
                    }
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "username or password is incorrect.  ");
                }
            }
            base.OnAuthorization(actionContext);
        }
    }
}