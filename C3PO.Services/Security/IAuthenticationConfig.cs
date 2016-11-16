using System;
using System.Collections.Generic;

namespace C3PO.Web.Security
{
    public interface IAuthenticationConfig
    {
        bool VerifyWithPassword { get; set; }
        string AuthenticationServerUrl { get; }
        int AuthenticationServerPort { get; set; }
        string AuthenticationClientId { get; set; }
        string AuthenticationServerGrantType { get; }
        string AuthenticationServerSecret { get; set; }
        string AuthenticationServerRootUrl { get; set; }
        string AuthenticationServerHostname { get; set; }

        List<string> AuthenticationScopes { get; set; }
    }
}