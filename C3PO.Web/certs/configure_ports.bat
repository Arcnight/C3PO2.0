: USAGE
: Find the certificate hash of the cert to be used for app server <-> web server communications
: Replace the my_certhash variable listed in this file with the 20-bit 
: These certificates are required to be in place before the servcies can communicate with the identity server

: MY_CERTHASH
: Set the certhash variable to the hash of the cert installed on the C3PO services server for communication with the identity server.
: ex. SET my_certhash=cbd73795151d644ce38d3330bc78e886985e704f (cert issued to localhost by C3PO.Services.RootCert)
SET my_certhash=b4e7b2fc7faec52628e22495fe08afebb9817970

: You can customize this variable (randomly generated at www.guidgenerator.com), but there is no need for it to change.
SET my_appid={bf9101b7-f110-42c7-92c3-aa90b5027b58}

netsh http delete urlacl url=https://+:3300/

netsh http delete sslcert ipport=0.0.0.0:3300

netsh http add urlacl url=https://+:3300/ user=BUILTIN\Administrators

netsh http add sslcert ipport=0.0.0.0:3300 certhash=%my_certhash% appid=%my_appid%