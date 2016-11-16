using System;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;

using Microsoft.Owin.Security;
using OWINSecurity = Microsoft.Owin.Security;

using Common.Logging;
using IdentityModel.Tokens;

namespace C3PO.Web.Security
{
    internal class JwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        static string _audiences = "all";
        static string _issuer = "PSIP2.0";

        ILog _logger;
        double _minUntilExpiration;

        public byte[] SigningKey { get; private set; }
        public static string Issuer { get { return _issuer; } }
        public static string Audiences { get { return _audiences; } }

        public JwtFormat(double minUntilExpiration)
        {
            _minUntilExpiration = minUntilExpiration;
            _logger = LogManager.GetCurrentClassLogger();

            SigningKey = new byte[64];

            RNGCryptoServiceProvider.Create().GetBytes(SigningKey);
        }
 
        public string Protect(AuthenticationTicket data)
        {
            _logger.Trace("Entering Protect(AuthenticationTicket ticket)");

            if (data == null) throw new ArgumentNullException("data");

            try
            {
                var currentTime = DateTime.UtcNow;
                var signingCredentials = new SigningCredentials(new InMemorySymmetricSecurityKey(SigningKey), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", "http://www.w3.org/2001/04/xmlenc#sha256");
                var tokenString = new JwtSecurityTokenHandler().WriteToken(
                    new JwtSecurityToken(_issuer, _audiences, data.Identity.Claims, currentTime, currentTime.AddMinutes(_minUntilExpiration), signingCredentials)
                );

                _logger.Trace("Exiting Protect(AuthenticationTicket)");

                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.Error("Error when getting the authentication ticket encrypted string.", ex);
                throw;
            }
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            _logger.Trace("Entering Unprotect(string protectedText)");

            if (protectedText == null) throw new ArgumentNullException("protectedText");

            AuthenticationTicket ticket = null;

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidIssuer = _issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudiences = new[] { _audiences },
                    IssuerSigningKey = new HmacSigningCredentials(SigningKey).SigningKey
                };

                //validate the jwt
                var jwtFormat = new OWINSecurity.Jwt.JwtFormat(validationParameters);

                ticket = jwtFormat.Unprotect(protectedText);

                _logger.Trace("Exiting Unprotect(string protectedText)");
            }
            catch (Exception ex)
            {
                _logger.Error("Error when decrypting the access token.", ex);
                throw;
            }

            return ticket;
        }
    }
}