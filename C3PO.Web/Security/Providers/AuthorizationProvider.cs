using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;

using AspNet.Security.OpenIdConnect.Server;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;

//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
//using Mvc.Server.Models;

namespace C3PO.Web.Security.Providers
{
    public class AuthorizationProvider : OpenIdConnectServerProvider
    {
        IServiceProvider _serviceLocator;

        public AuthorizationProvider()
        {
            
        }

        public override async Task ValidateAuthorizationRequest(ValidateAuthorizationRequestContext context)
        {
            context.Validate();

            //    // Note: the OpenID Connect server middleware supports the authorization code, implicit and hybrid flows
            //    // but this authorization provider only accepts response_type=code authorization/authentication requests.
            //    // You may consider relaxing it to support the implicit or hybrid flows. In this case, consider adding
            //    // checks rejecting implicit/hybrid authorization requests when the client is a confidential application.
            //    if (!context.Request.IsAuthorizationCodeFlow())
            //    {
            //        context.Reject(
            //            error: OpenIdConnectConstants.Errors.UnsupportedResponseType,
            //            description: "Only the authorization code flow is supported by this authorization server");

            //        return;
            //    }

            //    // Note: to support custom response modes, the OpenID Connect server middleware doesn't
            //    // reject unknown modes before the ApplyAuthorizationResponse event is invoked.
            //    // To ensure invalid modes are rejected early enough, a check is made here.
            //    if (!string.IsNullOrEmpty(context.Request.ResponseMode) && !context.Request.IsFormPostResponseMode() &&
            //                                                               !context.Request.IsFragmentResponseMode() &&
            //                                                               !context.Request.IsQueryResponseMode())
            //    {
            //        context.Reject(
            //            error: OpenIdConnectConstants.Errors.InvalidRequest,
            //            description: "The specified response_mode is unsupported.");

            //        return;
            //    }

            //    var database = context.HttpContext.RequestServices.GetRequiredService<ApplicationContext>();

            //    // Retrieve the application details corresponding to the requested client_id.
            //    var application = await (from entity in database.Applications
            //                             where entity.ApplicationID == context.ClientId
            //                             select entity).SingleOrDefaultAsync(context.HttpContext.RequestAborted);

            //    if (application == null)
            //    {
            //        context.Reject(
            //            error: OpenIdConnectConstants.Errors.InvalidClient,
            //            description: "Application not found in the database: ensure that your client_id is correct");

            //        return;
            //    }

            //    if (!string.IsNullOrEmpty(context.RedirectUri) &&
            //        !string.Equals(context.RedirectUri, application.RedirectUri, StringComparison.Ordinal))
            //    {
            //        context.Reject(
            //            error: OpenIdConnectConstants.Errors.InvalidClient,
            //            description: "Invalid redirect_uri");

            //        return;
            //    }

            //    context.Validate(application.RedirectUri);
        }

        public override Task ValidateTokenRequest(ValidateTokenRequestContext context)
        {
            /* ------------------------ Way to do #1 ----------------- */

            //// Note: the OpenID Connect server middleware supports authorization code, refresh token, client credentials
            //// and resource owner password credentials grant types but this authorization provider uses a safer policy
            //// rejecting the last two ones. You may consider relaxing it to support the ROPC or client credentials grant types.
            //if (!context.Request.IsAuthorizationCodeGrantType() && !context.Request.IsRefreshTokenGrantType())
            //{
            //    context.Reject(
            //        error: OpenIdConnectConstants.Errors.UnsupportedGrantType,
            //        description: "Only authorization code and refresh token grant types " +
            //                     "are accepted by this authorization server");

            //    return;
            //}

            //// Note: client authentication is not mandatory for non-confidential client applications like mobile apps
            //// (except when using the client credentials grant type) but this authorization server uses a safer policy
            //// that makes client authentication mandatory and returns an error if client_id or client_secret is missing.
            //// You may consider relaxing it to support the resource owner password credentials grant type
            //// with JavaScript or desktop applications, where client credentials cannot be safely stored.
            //// In this case, call context.Skip() to inform the server middleware the client is not trusted.
            //if (string.IsNullOrEmpty(context.ClientId) || string.IsNullOrEmpty(context.ClientSecret))
            //{
            //    context.Reject(
            //        error: OpenIdConnectConstants.Errors.InvalidRequest,
            //        description: "Missing credentials: ensure that your credentials were correctly " +
            //                     "flowed in the request body or in the authorization header");

            //    return;
            //}

            //var database = context.HttpContext.RequestServices.GetRequiredService<ApplicationContext>();

            //// Retrieve the application details corresponding to the requested client_id.
            //var application = await (from entity in database.Applications
            //                         where entity.ApplicationID == context.ClientId
            //                         select entity).SingleOrDefaultAsync(context.HttpContext.RequestAborted);

            //if (application == null)
            //{
            //    context.Reject(
            //        error: OpenIdConnectConstants.Errors.InvalidClient,
            //        description: "Application not found in the database: ensure that your client_id is correct");

            //    return;
            //}

            //// Note: to mitigate brute force attacks, you SHOULD strongly consider applying
            //// a key derivation function like PBKDF2 to slow down the secret validation process.
            //// You SHOULD also consider using a time-constant comparer to prevent timing attacks.
            //// For that, you can use the CryptoHelper library developed by @henkmollema:
            //// https://github.com/henkmollema/CryptoHelper. If you don't need .NET Core support,
            //// SecurityDriven.NET/inferno is a rock-solid alternative: http://securitydriven.net/inferno/
            //if (!string.Equals(context.ClientSecret, application.Secret, StringComparison.Ordinal))
            //{
            //    context.Reject(
            //        error: OpenIdConnectConstants.Errors.InvalidClient,
            //        description: "Invalid credentials: ensure that you specified a correct client_secret");

            //    return;
            //}

            //context.Validate();

            /* -------------- Way to do #2 ------------------------- */

            // Reject token requests that don't use grant_type=password or grant_type=refresh_token.
            if (!context.Request.IsPasswordGrantType() && !context.Request.IsRefreshTokenGrantType())
            {
                context.Reject(
                    error: OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    description: "Only grant_type=password and refresh_token " +
                                 "requests are accepted by this server.");

                return Task.FromResult(0);
            }

            // Note: you can skip the request validation when the client_id
            // parameter is missing to support unauthenticated token requests.
            // if (string.IsNullOrEmpty(context.ClientId)) {
            //     context.Skip();
            // 
            //     return Task.FromResult(0);
            // }

            // Note: to mitigate brute force attacks, you SHOULD strongly consider applying
            // a key derivation function like PBKDF2 to slow down the secret validation process.
            // You SHOULD also consider using a time-constant comparer to prevent timing attacks.
            //            if (string.Equals(context.ClientId, "client_id", StringComparison.Ordinal) &&
            //                string.Equals(context.ClientSecret, "client_secret", StringComparison.Ordinal))
            //            {
            //                context.Validate();
            //            }
            context.Skip();

            // Note: if Validate() is not explicitly called,
            // the request is automatically rejected.
            return Task.FromResult(0);
        }

        //public override async Task ValidateLogoutRequest(ValidateLogoutRequestContext context)
        //{
        //    var database = context.HttpContext.RequestServices.GetRequiredService<ApplicationContext>();

        //    // Skip validation if the post_logout_redirect_uri parameter was missing.
        //    if (string.IsNullOrEmpty(context.PostLogoutRedirectUri))
        //    {
        //        context.Skip();

        //        return;
        //    }

        //    // When provided, post_logout_redirect_uri must exactly match the address registered by the client application.
        //    if (!await database.Applications.AnyAsync(application => application.LogoutRedirectUri == context.PostLogoutRedirectUri))
        //    {
        //        context.Reject(
        //            error: OpenIdConnectConstants.Errors.InvalidClient,
        //            description: "Invalid post_logout_redirect_uri");

        //        return;
        //    }

        //    context.Validate();
        //}

        public override Task HandleTokenRequest(HandleTokenRequestContext context)
        {
            // Only handle grant_type=password token requests and let the
            // OpenID Connect server middleware handle the other grant types.
            if (context.Request.IsPasswordGrantType())
            {
                // Implement context.Request.Username/context.Request.Password validation here.
                // Note: you can call context Reject() to indicate that authentication failed.
                // Using password derivation and time-constant comparer is STRONGLY recommended.
                if (!string.Equals(context.Request.Username, "Bob", StringComparison.Ordinal) &&
                    !string.Equals(context.Request.Password, "P@ssw0rd", StringComparison.Ordinal))
                {
                    context.Reject(
                        error: OpenIdConnectConstants.Errors.InvalidGrant,
                        description: "Invalid user credentials.");

                    return Task.FromResult(0);
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationScheme)
                    .AddClaim(ClaimTypes.NameIdentifier, "[unique id]")

                    // By default, claims are not serialized in the access/identity tokens.
                    // Use the overload taking a "destinations" parameter to make sure
                    // your claims are correctly inserted in the appropriate tokens.
                    .AddClaim("urn:customclaim", "value", OpenIdConnectConstants.Destinations.AccessToken, OpenIdConnectConstants.Destinations.IdentityToken);

                double sessionLifetime = 5;
                var currentTime = DateTime.UtcNow;

                var ticket = new AuthenticationTicket(
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = true,
                        IssuedUtc = currentTime,
                        ExpiresUtc = currentTime.AddMinutes(sessionLifetime)
                    },
                    context.Options.AuthenticationScheme
                // Call SetScopes with the list of scopes you want to grant
                // (specify offline_access to issue a refresh token).
                ).SetScopes(
                    OpenIdConnectConstants.Scopes.Profile,
                    OpenIdConnectConstants.Scopes.OfflineAccess
                );

                //_sessionManager.CreateSession(userId, userName, _serviceLocator.GetInstance<JwtFormat>().Protect(ticket), ticket.Properties.ExpiresUtc.Value);

                context.Validate(ticket);
            }

            return Task.FromResult(0);
        }
    }
}