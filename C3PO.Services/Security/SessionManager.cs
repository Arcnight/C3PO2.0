using System;
using System.Linq;
using System.Timers;
using System.Collections.Concurrent;

namespace C3PO.Web.Security
{
    class OnSessionTimeoutEventArgs : EventArgs
    {
        public string SessionId { get; set; }
    }

    class SessionRefreshLock
    {
        readonly object _refreshLock = new object();

        public object Lock { get { return _refreshLock; } }
    }

    public class Session : IDisposable
    {
        Timer SessionTimer { get; set; }

        internal long UserId { get; private set; }
        internal string UserName { get; private set; }
        public string SessionId { get; private set; }
        public string AccessToken { get; private set; }

        public DateTimeOffset ExpiresUtc { get; private set; }
        public DateTimeOffset LastTimeRefreshed { get; private set; }

        internal event EventHandler<OnSessionTimeoutEventArgs> OnTimeout;

        Session() { }

        /// <summary>Constructor for the session object.</summary>
        /// <param name="sessionId">The unique id of the session.</param>
        /// <param name="userId">The user id of the user that the session belongs to.</param>
        /// <param name="userName">The username of the user that the session belongs to.</param>
        /// <param name="accessToken">The access token provided by the authorization service.</param>
        /// <param name="sessionLifetime">The length of the session in minutes.</param>
        /// <param name="expiresUtc">The UTC datetime that the access token will expire.</param>
        internal Session(string sessionId, long userId, string userName, string accessToken, double sessionLifetime, DateTimeOffset expiresUtc)
        {
            SessionId = sessionId;
            UserId = userId;
            UserName = userName;
            ExpiresUtc = expiresUtc;
            AccessToken = accessToken;
            LastTimeRefreshed = DateTime.UtcNow;

            SessionTimer = new Timer()
            {
                Enabled = true,
                AutoReset = false,
                Interval = sessionLifetime * 60000
            };

            SessionTimer.Elapsed += (sender, e) => SessionTimedOut(new OnSessionTimeoutEventArgs() { SessionId = SessionId });
        }

        internal void Start()
        {
            if (SessionTimer != null)
            {
                SessionTimer.Enabled = true;
                SessionTimer.Start();
            }
        }

        internal void Stop()
        {
            if (SessionTimer != null)
            {
                SessionTimer.Stop();
                SessionTimer.Enabled = false;
            }
        }

        internal void Refresh(string newAccessToken, DateTimeOffset newExpiresUtc)
        {
            if (SessionTimer != null)
            {
                SessionTimer.Stop();

                ExpiresUtc = newExpiresUtc;
                AccessToken = newAccessToken;

                LastTimeRefreshed = DateTime.UtcNow;

                SessionTimer.Start();
            }
        }

        public void Dispose()
        {
            SessionTimer.Dispose();
        }

        void SessionTimedOut(OnSessionTimeoutEventArgs e)
        {
            OnTimeout?.Invoke(null, e);
        }
    }

    public class SessionManager : ISessionManager
    {
        ILog _logger;
        static readonly object _globalSessionRefreshLock = new object();
        ConcurrentDictionary<string, Session> _sessions = new ConcurrentDictionary<string, Session>();
        ConcurrentDictionary<string, SessionRefreshLock> _sessionRefreshLocks = new ConcurrentDictionary<string, SessionRefreshLock>();

        public Session this[string sessionId]
        {
            get
            {
                Session session = null;

                _sessions.TryGetValue(sessionId, out session);

                return session;
            }
        }

        public double SessionLifetime { get; set; }

        SessionManager() { }

        public SessionManager(double sessionLifetime)
        {
            SessionLifetime = sessionLifetime;

            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>Creates a session for a user.</summary>
        /// <param name="userId">The user id of the user the session is being created for.</param>
        /// <param name="userName">The username of the user the session is being created for.</param>
        /// <param name="accessToken">The access token provided by the authorization service.</param>
        /// <param name="expiresUtc">The UTC datetime that the access token will expire.</param>
        public string CreateSession(long userId, string userName, string accessToken, DateTimeOffset expiresUtc)
        {
            if (string.IsNullOrEmpty(accessToken)) return string.Empty;

            var session = AddNewSession(userId, userName, accessToken, SessionLifetime, expiresUtc);

            _logger.InfoFormat("Session created for {0} at {1:MM/dd/yyyy HH:mm:ss} UTC", userName, DateTime.UtcNow);

            return session.SessionId;
        }

        public string GetAccessToken(string sessionId)
        {
            Session session = null;

            if (_sessions.TryGetValue(sessionId ?? string.Empty, out session)) return session.AccessToken;

            return string.Empty;
        }

        public void TerminateSession(string sessionId)
        {
            Session sessionToTerminate = null;

            if (_sessions.TryRemove(sessionId ?? string.Empty, out sessionToTerminate))
            {
                using (sessionToTerminate)
                {
                    sessionToTerminate.Stop();

                    _logger.InfoFormat("Session terminated for {0} at {1:MM/dd/yyyy HH:mm:ss} UTC", sessionToTerminate.UserName, DateTime.UtcNow);

                    SessionRefreshLock sessionRefreshLock;

                    _sessionRefreshLocks.TryRemove(sessionToTerminate.SessionId, out sessionRefreshLock);
                }
            }
        }

        public void RefreshSession(string sessionId, string newAccessToken, DateTimeOffset newExpiresUtc)
        {
            Session session = null;

            if (_sessions.TryGetValue(sessionId ?? string.Empty, out session))
            {
                var userName = session.UserName;
                SessionRefreshLock sessionRefreshLock;

                // Lock this so that the access token for the session can be refreshed
                // before it is used again.
                if (_sessionRefreshLocks.TryGetValue(sessionId ?? string.Empty, out sessionRefreshLock))
                {
                    // If the session refresh lock is found for this particular session use it to do a
                    // lock for only this session...
                    lock (sessionRefreshLock.Lock)
                    {
                        session.Refresh(newAccessToken, newExpiresUtc);
                    }
                }
                else
                {
                    // ... otherwise do a general lock before refreshing the session
                    lock (_globalSessionRefreshLock)
                    {
                        session.Refresh(newAccessToken, newExpiresUtc);
                    }
                }

                _logger.InfoFormat("Session refreshed for {0} at {1:MM/dd/yyyy HH:mm:ss} UTC", userName, DateTime.UtcNow);
            }
        }

        public DateTimeOffset? GetSessionExpirationDateUTC(string sessionId)
        {
            Session session = null;

            if (_sessions.TryGetValue(sessionId ?? string.Empty, out session)) return session.ExpiresUtc;

            return null;
        }

        public Session GetSessionByUserName(string userName)
        {
            return _sessions.Values.Where(s => s.UserName == (userName ?? string.Empty)).FirstOrDefault();
        }

        public void Dispose()
        {
            foreach (var session in _sessions.Values)
            {
                using (session)
                {
                    SessionRefreshLock sessionRefreshLock;

                    _sessionRefreshLocks.TryRemove(session.SessionId, out sessionRefreshLock);
                }
            }
        }

        Session AddNewSession(long userId, string userName, string accessToken, double sessionLifetime, DateTimeOffset expiresUtc)
        {
            // Check to see if the user already has an active session.  If so then just use that one.
            Session session = GetSessionByUserName(userName);

            if (session == null)
            {
                session = new Session(Guid.NewGuid().ToString(), userId, userName, accessToken, sessionLifetime, expiresUtc);

                if (_sessions.TryAdd(session.SessionId, session))
                {
                    _sessionRefreshLocks.TryAdd(session.SessionId, new SessionRefreshLock());

                    // Subscribe to the session timeout event and start the timer
                    session.OnTimeout += (sender, e) => SessionTimedOut(e.SessionId);
                    session.Start();
                }
            }

            return session;
        }

        void SessionTimedOut(string refreshToken)
        {
            Session expiredSession = null;

            if (_sessions.TryRemove(refreshToken ?? string.Empty, out expiredSession))
            {
                using (expiredSession)
                {
                    expiredSession.Stop();

                    _logger.InfoFormat("Session timed out for {0} at {1:MM/dd/yyyy HH:mm:ss} UTC", expiredSession.UserName, DateTime.UtcNow);

                    SessionRefreshLock sessionRefreshLock;

                    _sessionRefreshLocks.TryRemove(expiredSession.SessionId, out sessionRefreshLock);
                }
            }
        }
    }
}