using System;
using System.Linq;
using System.Reflection;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Net;

using System.Security.Claims;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

using Microsoft.Practices.ServiceLocation;

using Common.Logging;

using PSIP.Data.Models;
using PSIP.Data.EntityFramework;
using PSIP.Data.Web.SimplifiedModels;

using PSIP.Security;
using PSIP.Security.Web;
using PSIP.Security.Helpers;
using PSIP.Security.Interfaces;

using PSIP.Services.Web.Helpers;
using PSIP.Services.Web.Security;

using PSIP.Common.Helpers;
using PSIP.Common.Logging;

namespace C3PO.Web.Security
{
    internal class AuthenticationProvider : OAuthAuthorizationServerProvider
    {
        const string IS_INTERNAL_KEY = "userIsInternal";

        IServiceLocator _serviceLocator;
        ISessionManager _sessionManager;

        public AuthenticationProvider()
        {
            _serviceLocator = ServiceLocator.Current;
            _sessionManager = _serviceLocator.GetInstance<ISessionManager>();
        }

        /// <summary>
        /// Used to verify that the client is a valid client.
        /// </summary>
        /// <param name="context"></param>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // We're saying that the context is validated because there is only one client that will be accessing the service
            context.Validated();

            return Task.FromResult<object>(null);
        }

        //public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        //{
        //    var authenticationResponse = context.OwinContext.Get<AuthenticationResponse>("authenticationResponse");

        //    if (authenticationResponse != null)
        //    {
        //        var token = context.AccessToken;
        //        authenticationResponse.token = token;
        //    }

        //    return base.TokenEndpointResponse(context);
        //}

        //public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        //{
        //    if (context.OwinContext.Request.Method == "OPTIONS" && context.IsTokenEndpoint)
        //    {
        //        context.Response.Headers.Add(AccessControlAllowOrigin, new[] { "*" });
        //        context.Response.Headers.Add(AccessControlAllowMethods, new[] { "POST" });
        //        context.Response.Headers.Add(AccessControlAllowHeaders, new[] { "accept", "authorization", "content-type", "isinternal" });
        //        context.Response.StatusCode = 200;
        //        context.RequestCompleted();

        //        return Task.FromResult<object>(null);
        //    }

        //    return base.MatchEndpoint(context);
        //}

        //public override async Task ValidateAuthorizeRequest(OAuthValidateAuthorizeRequestContext context)
        //{
        //    DateTime? sessionExpirationDate = null;
            //var accessTicket = _sessionManager.GetSessionTicket("");

        //    if (accessTicket != null)
        //    {
        //        // Get a new refresh and access token if the session is going to end in 5 minutes or less
        //        if (sessionExpirationDate.HasValue && (sessionExpirationDate.Value - DateTime.UtcNow).TotalMinutes <= 5)
        //        {
        //            //var client = new TokenClient(authSvcConfig.AuthenticationServerUrl + "/connect/token", authSvcConfig.AuthenticationClientId, authSvcConfig.AuthenticationServerSecret);
        //            //var response = client.RequestRefreshTokenAsync(ctx.Token).Result;

        //            //if (response.IsError || response.IsHttpError)
        //            //{
        //            //    Logger.Error("Error returned when accessing token service to refresh tokens: " + (response.Error ?? response.HttpErrorReason));
        //            //    Logger.Debug("Exiting OnRequestToken()");
        //            //}
        //            //else
        //            //{
        //            //    accessToken = response.AccessToken;

        //            //    // TODO: For right now just use the same refresh token even though it will expire.
        //            //    //       Need to figure out a way to tell the front end about the new refresh token
        //            //    //       at some point.
        //            //    SessionManager.RefreshSession(ctx.Token, accessToken, ctx.Token);
        //            //    //SessionManager.RefreshSession(ctx.Token, accessToken, response.RefreshToken);
        //            //}
        //        }

        //        context. = accessToken;
        //    }
        //}

        //public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        //{
        //    var owinContext = context.OwinContext;
        //    var authenticationResponse = owinContext.Get<AuthenticationResponse>("authenticationResponse");
        //    //var user = owinContext.Get<UserProfileSimple>(USER_KEY);
        //    //var operationNames = owinContext.Get<string[]>(OPERATION_NAMES_KEY);

        //    context.AdditionalResponseParameters.Add("User", JsonConvert.SerializeObject(authenticationResponse.User));
        //    context.AdditionalResponseParameters.Add("Operations", JsonConvert.SerializeObject(authenticationResponse.OperationNames));

        //    return Task.FromResult<object>(null);
        //}

        //public override Task AuthorizeEndpoint(OAuthAuthorizeEndpointContext context)
        //{
        //    return base.AuthorizeEndpoint(context);
        //}

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            // Change authentication ticket for refresh token requests
            var newTicket = new AuthenticationTicket(new ClaimsIdentity(context.Ticket.Identity), context.Ticket.Properties);

            var sessionId = context.Request.Headers["Bearer"];

            //_sessionManager.RefreshSession(sessionId, newTicket);

            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var logger = LogManager.GetCurrentClassLogger();
            var roleService = ServiceLocator.Current.GetInstance<IRoleService>();
            var auditLogger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            logger.Debug("Entering GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)");

            var loginValid = true;
            var userIp = "unknown";
            var isInternalUser = false;
            var errMessage = string.Empty;

            X509Certificate2 clientCert = null;

            var certKvp = context.OwinContext.Request.Headers.SingleOrDefault(x => x.Key == "X-ARR-ClientCert");

            if (certKvp.Value == null || !certKvp.Value.Any())
            {
                errMessage = "No certificate found in header of request";
                loginValid = false;
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                clientCert = Methods.GetCertificateFromString(certKvp.Value.First());
            }

            if (loginValid)
            {
                using (var dbContext = PsipDbContextFactory.Current)
                {
                    try
                    {
                        var subjectCN = clientCert.GetSubjectCN();

                        //var user = Users.SingleOrDefault(x => x.Username == context.UserName);
                        var user = dbContext.Users.Include(u => u.UserProfileInternal).Include(u => u.UserProfileExternal).FirstOrDefault(u => u.UserName == subjectCN);

                        if (user == null)
                        {
                            log4net.LogicalThreadContext.Properties["UserId"] = "user with username " + context.UserName;

                            errMessage = "Could not locate user with username " + context.UserName;
                            logger.Error(errMessage);
                            logger.Debug("Exiting GrantResourceOwnerCredentials");

                            loginValid = false;
                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        }
                        else
                        {
                            log4net.LogicalThreadContext.Properties["UserId"] = StringHelper.GetPersonFullname(user.FirstName, user.MiddleName, user.LastName);

                            isInternalUser = (user.InternalProfileId ?? 0) > 0;

                            var authUser = new AuthenticatedUser(subjectCN, user, isInternalUser);

                            logger.Debug(string.Format("{0}, {1}, {2}, {3}, {4}", authUser.CacId, authUser.CacFirstName, authUser.CacLastName, user.UserName, user.UserId));

                            if (user.AccountLocked)
                            {
                                errMessage = string.Format("User account is locked out for userId: {0}", user.UserId);
                                logger.Error(errMessage);
                                logger.Debug("Exiting GrantResourceOwnerCredentials");

                                auditLogger.AuditFormat("Attempt to access locked account from IP <{0}>", userIp);

                                loginValid = false;

                                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                                //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
                            }

                            //if (loginValid && _verifyPassword && !string.IsNullOrEmpty(context.Password))
                            //{
                            //    if (!Methods.DoPasswordsMatch(user.PasswordString, context.Password))
                            //    {
                            //        TimeSpan oneHr = new TimeSpan(0, 1, 0, 0);
                            //        if (user.FirstAttemptDate != null && DateTime.UtcNow - user.FirstAttemptDate < oneHr &&
                            //            user.SecondAttemptDate != null && DateTime.UtcNow - user.SecondAttemptDate < oneHr &&
                            //            user.SecondAttemptDate > user.FirstAttemptDate)
                            //        {
                            //            user.AccountLocked = true;
                            //        }
                            //        else if ((user.FirstAttemptDate == null || DateTime.UtcNow - user.FirstAttemptDate > oneHr) && (user.SecondAttemptDate == null || DateTime.UtcNow - user.SecondAttemptDate > oneHr))
                            //        {
                            //            user.FirstAttemptDate = DateTime.UtcNow;

                            //            auditLogger.AuditFormat("User from IP <{0}> had first invalid login attempt", userIp);
                            //        }
                            //        else if (DateTime.UtcNow - user.FirstAttemptDate > oneHr && DateTime.UtcNow - user.SecondAttemptDate < oneHr)
                            //        {
                            //            user.FirstAttemptDate = user.SecondAttemptDate;
                            //            user.SecondAttemptDate = DateTime.UtcNow;

                            //            auditLogger.AuditFormat("User from IP <{0}> had second invalid login attempt", userIp);
                            //        }
                            //        else if (DateTime.UtcNow - user.FirstAttemptDate < oneHr)
                            //        {
                            //            user.SecondAttemptDate = DateTime.UtcNow;

                            //            auditLogger.AuditFormat("User from IP <{0}> had second invalid login attempt", userIp);
                            //        }

                            //        dbContext.SaveChanges(); //need to return forbbiden here too if tries = 3

                            //        if (user.AccountLocked)
                            //        {
                            //            errMessage = string.Format("User account is now locked out for userId: {0}", user.UserId);

                            //            logger.Info(errMessage);
                            //            logger.Debug("Exiting AuthenticateLocalAsync");

                            //            auditLogger.AuditFormat("User from IP <{0}> went over login attempts in one hour and account has been locked", userIp);

                            //            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            //            //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
                            //        }
                            //        else
                            //        {
                            //            errMessage = string.Format("Incorrect Password for userId: {0}", user.UserId);

                            //            logger.Info(errMessage);
                            //            logger.Debug("Exiting GrantResourceOwnerCredentials");

                            //            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            //            //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                            //        }

                            //        loginValid = false;
                            //    }
                            //}

                            var owinContext = context.OwinContext;

                            string[] operationNames = null;
                            List<ClearanceAttribute> attributes = null;
                            List<SecurableActivitySimple> roles = null;
                            List<SecurableActivitySimple> operations = null;
                            List<SecurableActivitySimple> roleOperations = null;

                            if (loginValid)
                            {
                                try
                                {
                                    operationNames = roleService.GetOperationsFor(user).Select(o => o.ToString()).ToArray();
                                }
                                catch (Exception ex)
                                {
                                    loginValid = false;
                                    errMessage = string.Format("Unable to retrieve user's security operations for userId: {0}", user.UserId);

                                    logger.Error("Unable to retrieve user's security operations", ex);
                                    logger.Debug("Exiting GrantResourceOwnerCredentials");

                                    auditLogger.AuditFormat("Could not find security operations for user from IP <{0}>", userIp);

                                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                                }

                                try
                                {
                                    roles = roleService.GetRoleActivitiesFor(user).Select(r => SecurableActivityHelper.ConvertSecurableActivityToSimple(r)).ToList();
                                }
                                catch (Exception ex)
                                {
                                    loginValid = false;
                                    errMessage = string.Format("Could not find roles for userId: {0}", user.UserId);

                                    logger.Error("Unable to retrieve user's roles", ex);
                                    logger.Debug("Exiting GrantResourceOwnerCredentials");

                                    auditLogger.AuditFormat("Could not find roles for user from IP <{0}>", userIp);

                                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                                }

                                try
                                {
                                    operations = roleService.GetOperationActivitiesFor(user).Select(o => SecurableActivityHelper.ConvertSecurableActivityToSimple(o)).ToList();
                                    roleOperations = roleService.GetRoleOperationActivitiesFor(user).Select(o => SecurableActivityHelper.ConvertSecurableActivityToSimple(o)).ToList();
                                }
                                catch (Exception ex)
                                {
                                    loginValid = false;
                                    errMessage = string.Format("Could not find operations for userId: {0}", user.UserId);

                                    logger.Error("Unable to retrieve user's operations", ex);
                                    logger.Debug("Exiting GrantResourceOwnerCredentials");

                                    auditLogger.AuditFormat("Could not find operations for user from IP <{0}>", userIp);

                                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                                }

                                try
                                {
                                    attributes = dbContext.ClearanceAttributes.OrderBy(a => a.ClearanceAttributeId).ToList();
                                }
                                catch (Exception ex)
                                {
                                    loginValid = false;
                                    errMessage = string.Format("Could not create response object for userId: {0}", user.UserId);

                                    logger.Error("Unable to create response", ex);
                                    logger.Debug("Exiting GrantResourceOwnerCredentials");

                                    auditLogger.AuditFormat("Could not create response object for user from IP <{0}>", userIp);

                                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                                }
                            }

                            if (loginValid)
                            {
                                //if success
                                user.NumberTries = 0;
                                user.FirstAttemptDate = null;
                                user.SecondAttemptDate = null;
                                user.CurrentLoginDate = DateTime.UtcNow;
                                user.LastLoginDate = user.CurrentLoginDate;

                                dbContext.SaveChanges();

                                auditLogger.AuditFormat("User logged in from IP <{0}>", userIp);

                                logger.TraceFormat("GrantResourceOwnerCredentials: User {0} authenticated.", user.UserName);

                                var userIdentity = new ClaimsIdentity(
                                    new GenericIdentity(user.UserName),
                                    new List<Claim>()
                                    {
                                        new Claim("sub", user.UserId.ToString()),
                                        new Claim(ClaimTypes.Name, user.UserName),
                                        new Claim(IS_INTERNAL_KEY, isInternalUser.ToString().ToLower())
                                    },
                                    context.Options.AuthenticationType,
                                    string.Empty,
                                    string.Empty
                                );

                                var currentTime = DateTime.UtcNow;

                                var ticket = new AuthenticationTicket(userIdentity, new AuthenticationProperties()
                                {
                                    AllowRefresh = true,
                                    IsPersistent = true,
                                    IssuedUtc = currentTime,
                                    ExpiresUtc = currentTime.AddMinutes(_sessionManager.SessionLifetime),
                                });

                                context.Validated(ticket);

                                owinContext.Set("authenticationResponse", new AuthenticationResponse()
                                {
                                    OperationNames = operationNames,
                                    User = new UserProfileSimple(user, isInternalUser, roles, operations, roleOperations, attributes),
                                    token = _sessionManager.CreateSession(user.UserId, user.UserName, _serviceLocator.GetInstance<JwtFormat>().Protect(ticket), ticket.Properties.ExpiresUtc.Value)
                                });

                                context.Request.User = new ClaimsPrincipal(userIdentity);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errMessage = "Authentication error: " + ex.InnerException;
                        logger.Error("GrantResourceOwnerCredentials: unknown error in locating user", ex);

                        loginValid = false;
                    }
                }
            }

            if (!loginValid)
            {
                context.SetError("invalid_grant", errMessage ?? string.Format("Could not login user from IP <{0}>", userIp));
                context.Rejected();
            }

            return Task.FromResult<object>(null);
        }
    }
}