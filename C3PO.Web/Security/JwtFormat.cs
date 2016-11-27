using System;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.ServiceModel.Security.Tokens;

using Serilog;
using Microsoft.Owin.Security;
using OWINSecurity = Microsoft.Owin.Security;

using C3PO.Utilities;
//using IdentityModel.Tokens;

namespace C3PO.Web.Security
{
    internal class JwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        static string _audiences = "all";
        static string _issuer = "PSIP2.0";

        ILogger _logger;
        double _minUntilExpiration;

        public byte[] SigningKey { get; private set; }
        public static string Issuer { get { return _issuer; } }
        public static string Audiences { get { return _audiences; } }

        public JwtFormat(double minUntilExpiration)
        {
            _minUntilExpiration = minUntilExpiration;
            _logger = Logging.CreateLogger();

            SigningKey = new byte[64];

            RNGCryptoServiceProvider.Create().GetBytes(SigningKey);
        }
 
        public string Protect(AuthenticationTicket data)
        {
            _logger.Debug("Entering Protect(AuthenticationTicket ticket)");

            if (data == null) throw new ArgumentNullException("data");

            try
            {
                var currentTime = DateTime.UtcNow;
                var signingCredentials = new SigningCredentials(new InMemorySymmetricSecurityKey(SigningKey), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", "http://www.w3.org/2001/04/xmlenc#sha256");
                var tokenString = new JwtSecurityTokenHandler().WriteToken(
                    new JwtSecurityToken(_issuer, _audiences, data.Identity.Claims, currentTime, currentTime.AddMinutes(_minUntilExpiration), signingCredentials)
                );

                _logger.Debug("Exiting Protect(AuthenticationTicket)");

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
            _logger.Debug("Entering Unprotect(string protectedText)");

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
                    IssuerSigningToken = new BinarySecretSecurityToken(SigningKey)
                    //IssuerSigningKey = new HmacSigningCredentials(SigningKey).SigningKey
                };

                //validate the jwt
                var jwtFormat = new OWINSecurity.Jwt.JwtFormat(validationParameters);

                ticket = jwtFormat.Unprotect(protectedText);

                _logger.Debug("Exiting Unprotect(string protectedText)");
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