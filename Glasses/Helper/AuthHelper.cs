using BAL.IRepositry;
using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Technical.Helper
{
    public class AuthHelper
    {
        public class UserInfo
        {
            public long Id { get; set; }
            public string UserName { get; set; }
        }
        public static long GetCurrentUserId(ClaimsPrincipal user)
        {
            string userId = user.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            long UserId = 0;
            if (long.TryParse(userId, out UserId))
            {
                return UserId;
            }
            else
                throw new UnauthorizedAccessException();
        }
        public static string GetCurrentUserSearchId(ClaimsPrincipal user)
        {
            string searchId = user.Claims.FirstOrDefault(c => c.Type == "SearchId").Value;
            if (!string.IsNullOrEmpty(searchId))
            {
                return searchId;
            }
            else
                throw new Exception();
        }
        public static UserInfo GetCurrentUser(ClaimsPrincipal user, IUserRepositry _userService)
        {
            var userObj = (User)_userService.GetById(GetCurrentUserId(user));
            return new UserInfo() { Id = userObj.UserId, UserName = userObj.UserName };
        }
      
        public static User GetCurrentUserById(long id, IUserRepositry _userService)
        {
            return (User)_userService.GetById(id);
        }
     
    }
}
