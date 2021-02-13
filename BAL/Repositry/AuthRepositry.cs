using BAL.IRepositry;
using DataAccessLayer.Model;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL.Repositry
{
    public class AuthRepositry : IAuthService
    {
        private readonly TechnicalContext db;
        public AuthRepositry(TechnicalContext _db)
        {
            db = _db;
        }
        public bool AddToken(RefreshToken token)
        {
            if (token == null)
                return false;

            db.RefreshTokens.Add((token));
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ExpireToken(RefreshToken token)
        {
            if (token == null)
                return false;

            db.RefreshTokens.Update((token));
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public RefreshToken GetToken(string refresh_token)
        {
            var token = db.RefreshTokens.FirstOrDefault(x => x.RefreshTokenId == refresh_token);
            if (token == null)
                return null;
            else
                return (token);
        }



        public User GetUserByEmailAndPassword(string userName, string password)
        {
            var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            if (user != null && user.Password == (password))
            {
                return (user);
            }
            else
                return null;
        }

        public RefreshToken GenerateRefreshTokenModel(string clientId, string clientSecret, string userName, string refreshToken, long userId)
        {
            return new RefreshToken
            {
                ClientId = clientId,
                RefreshTokenSubject = userName,
                RefreshTokenId = refreshToken,
                RefreshTokenIssuedUtc = DateTime.UtcNow,
                RefreshTokenExpiresUtc = DateTime.UtcNow.AddDays(1),
                UserId = userId,
                IsRefreshTokenDeleted = false
            };
        }

        public User GetUserByEmail(string email)
        {
            var user = db.Users.FirstOrDefault(x => x.UserName == email);
            if (user != null)
            {
                return (user);
            }
            else
                return null;
        }

        public bool UpdateLastAccess(long userId)
        {
            var user = db.Users.Find(userId);

            user.LastAccess = DateTime.Now;

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }
    }
}
