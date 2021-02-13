using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class RefreshToken
    {
        public string RefreshTokenId { get; set; }
        public string RefreshTokenSubject { get; set; }
        public DateTime RefreshTokenIssuedUtc { get; set; }
        public DateTime RefreshTokenExpiresUtc { get; set; }
        public bool? IsRefreshTokenDeleted { get; set; }

        // Client
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ClientName { get; set; }
        public bool? ClientApplicationType { get; set; }
        public bool? ClientActive { get; set; }
        public int? ClientRefreshTokenLifeTime { get; set; }
        public string ClientAllowedOrigin { get; set; }

        // User
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public bool IsUserBlocked { get; set; }
        public bool IsUserDeleted { get; set; }
        public bool IsUserReported { get; set; }
    }
}
