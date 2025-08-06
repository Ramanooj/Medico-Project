using System;
using AgoraIO.Media; 

namespace MedicoAPI.Controllers 
{
    public class AgoraTokenService
    {
        private readonly string _appId = Environment.GetEnvironmentVariable("agoraAppId");
        private readonly string _appCertificate = Environment.GetEnvironmentVariable("agoraCert");

        public string GenerateToken(string channelName, uint uid, int expirationTimeInSeconds)
        {
            var privilegeExpiredTs = (uint)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + expirationTimeInSeconds);

            AccessToken token = new AccessToken(_appId, _appCertificate, channelName, uid.ToString());

            token.addPrivilege(Privileges.kJoinChannel, privilegeExpiredTs);

            return token.build();
        }
    }
}
