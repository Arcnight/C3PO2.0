using System;
using System.Threading.Tasks;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Owin.Security.Infrastructure;

using C3PO.Web.Security.Interfaces;

namespace C3PO.Web.Security
{
    internal class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        ISessionManager _sessionManager;

        public RefreshTokenProvider()
        {
            _sessionManager = ServiceLocator.Current.GetInstance<ISessionManager>();
        }

        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var authenticationResponse = context.OwinContext.Get<AuthenticationResponse>("authenticationResponse");

            if (authenticationResponse != null) context.SetToken(authenticationResponse.Token);

            return Task.FromResult<object>(null);
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            string header = context.OwinContext.Request.Headers["Authorization"];

            var session = _sessionManager[context.Token];

            if (session != null)
            {
                context.DeserializeTicket(session.AccessToken);
            }

            return Task.FromResult<object>(null);
        }
    }
}