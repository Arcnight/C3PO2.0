using System;

namespace C3PO.Web.Security.Interfaces
{
    public interface ISessionManager : IDisposable
    {
        double SessionLifetime { get; set; }
        Session this[string sessionId] { get; }

        void TerminateSession(string sessionId);
        void RefreshSession(string sessionId, string newAccessToken, DateTimeOffset newExpiresUtc);

        /// <summary>Creates a session for a user.</summary>
        /// <param name="userId">The user id of the user the session is being created for.</param>
        /// <param name="userName">The username of the user the session is being created for.</param>
        /// <param name="accessToken">The access token provided by the authorization service.</param>
        /// <param name="expiresUtc">The UTC datetime that the access token will expire.</param>
        string CreateSession(long userId, string userName, string accessToken, DateTimeOffset expiresUtc);

        DateTimeOffset? GetSessionExpirationDateUTC(string sessionId);

        Session GetSessionByUserName(string userName);
    }
}
