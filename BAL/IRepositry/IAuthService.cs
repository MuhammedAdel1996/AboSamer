using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.IRepositry
{
    public interface IAuthService
    {
        bool AddToken(RefreshToken token);
        bool ExpireToken(RefreshToken token);
        RefreshToken GetToken(string refresh_token);
        User GetUserByEmailAndPassword(string email, string password);
        User GetUserByEmail(string email);
        RefreshToken GenerateRefreshTokenModel(string clientId, string clientSecret, string userName, string refreshToken, long userId);
        bool UpdateLastAccess(long userId);
    }
}
