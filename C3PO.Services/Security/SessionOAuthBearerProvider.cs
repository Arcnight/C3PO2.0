using System;
using System.Threading.Tasks;

using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.ServiceLocation;

namespace C3PO.Web.Security
{
    internal class SessionOAuthBearerProvider : OAuthBearerAuthenticationProvider
    {
        IServiceLocator _serviceLocator;
        ISessionManager _sessionManager;

        public SessionOAuthBearerProvider()
        {
            _serviceLocator = ServiceLocator.Current;
            _sessionManager = _serviceLocator.GetInstance<ISessionManager>();
        }

        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            // Retrieve the session id from the request header
            var sessionId = context.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(sessionId))
            {
                var certString = context.Request.Headers["X-ARR-ClientCert"];

                if (string.IsNullOrEmpty(certString)) return Task.FromResult<object>(null);

                var session = _sessionManager.GetSessionByUserName(Methods.GetCertificateFromString(certString).GetEDIPI());

                if (session != null) sessionId = session.SessionId;
            }

            if (!string.IsNullOrEmpty(sessionId))
            {
                sessionId = sessionId.Replace("Bearer ", string.Empty);

                var session = _sessionManager[sessionId];

                if (session != null)
                {
                    var currentTime = DateTime.UtcNow;
                    var accessToken = session.AccessToken;

                    // If the last time the user's session was refreshed was more than a minute ago, refresh their session
                    if (session.ExpiresUtc > currentTime && (currentTime - session.LastTimeRefreshed).TotalMinutes > 1)
                    {
                        var expiresUtc = currentTime.AddMinutes(_sessionManager.SessionLifetime);
                        var jwtFormat = _serviceLocator.GetInstance<JwtFormat>();
                        var ticket = jwtFormat.Unprotect(accessToken);
                        var ticketProps = ticket.Properties;

                        ticketProps.IssuedUtc = currentTime;
                        ticketProps.ExpiresUtc = expiresUtc;

                        accessToken = jwtFormat.Protect(ticket);

                        _sessionManager.RefreshSession(sessionId, accessToken, expiresUtc);
                    }

                    context.Token = accessToken;
                }
            }

            return Task.FromResult<object>(null);
        }
    }
}