using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BAL.Helper;
using BAL.IRepositry;
using DataAccessLayer;
using DataAccessLayer.Model;
using Technical.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Technical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private IOptions<Jwt> _settings;
        private IAuthService _authService;
        private IUserRepositry _userService;
        public AuthController(IOptions<Jwt> settings, IAuthService authService, IUserRepositry userService)
        {
            this._settings = settings;
            this._authService = authService;
            this._userService = userService;
        }
        [HttpPost("authUser")]
        public ResponseData Auth([FromBody]AuthParameters parameters)
        {
            if (parameters == null)
            {
                return (new ResponseData
                {
                    Code = "901",
                    Message = "null of parameters",
                    Data = null
                });
            }

            if (parameters.grant_type == "password")
            {
                return (DoPassword(parameters));
            }
            else if (parameters.grant_type == "refresh_token")
            {
                return (DoRefreshToken(parameters));
            }
            else
            {
                return (new ResponseData
                {
                    Code = "904",
                    Message = "bad request",
                    Data = null
                });
            }
        }

        //scenario 1 ï¼š get the access-token by username and password  
        private ResponseData DoPassword(AuthParameters parameters)
        {

           
            var user = _authService.GetUserByEmailAndPassword(parameters.email, parameters.password);

            if ( user == null)
            {
                return new ResponseData
                {
                    Code = "902",
                    Message = "invalid user infomation",
                    Data = null
                };
            }

            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");

            var rToken = _authService.GenerateRefreshTokenModel(
                parameters.client_id,
                parameters.client_secret,
                user.UserName,
                refresh_token,
                user.UserId);


            //store the refresh_token   
            if (_authService.AddToken(rToken))
            {
                if (_authService.UpdateLastAccess(user.UserId))
                    return new ResponseData
                    {
                        Code = "999",
                        Message = "OK",
                        Data = GetJwt(parameters.client_id, refresh_token, user)
                    };
                else
                    return new ResponseData
                    {
                        Code = "909",
                        Message = "can not update last access in database",
                        Data = null
                    };
            }
            else
            {
                return new ResponseData
                {
                    Code = "909",
                    Message = "can not add token to database",
                    Data = null
                };
            }
        }

        //scenario 2 ï¼š get the access_token by refresh_token  
        private ResponseData DoRefreshToken(AuthParameters parameters)
        {
            var token = _authService.GetToken(parameters.refresh_token);

            if (token == null)
            {
                return new ResponseData
                {
                    Code = "905",
                    Message = "can not refresh token",
                    Data = null
                };
            }

            if (token.RefreshTokenExpiresUtc <= DateTime.Now || token.IsRefreshTokenDeleted == true)
            {
                return new ResponseData
                {
                    Code = "906",
                    Message = "refresh token has expired",
                    Data = null
                };
            }

            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");

            token.IsRefreshTokenDeleted = true;
            //expire the old refresh_token and add a new refresh_token  
            var updateFlag = _authService.ExpireToken(token);

            var newRToken = _authService.GenerateRefreshTokenModel(
                parameters.client_id,
                parameters.client_secret,
                token.UserName,
                refresh_token,
                token.UserId);

            var addFlag = _authService.AddToken(newRToken);

            if (updateFlag && addFlag)
            {
                return new ResponseData
                {
                    Code = "999",
                    Message = "OK",
                    Data = GetJwt(parameters.client_id, refresh_token, AuthHelper.GetCurrentUserById(token.UserId, _userService))
                };
            }
            else
            {
                return new ResponseData
                {
                    Code = "910",
                    Message = "can not expire token or a new token",
                    Data = null
                };
            }
        }

        //get the jwt token   
        private Object GetJwt(string client_id, string refresh_token, User User)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, client_id),
                new Claim("UserId", User.UserId.ToString()),
                new Claim("UserName", User.UserName.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
            };

            var symmetricKeyAsBase64 = "Y2F0Y2hlciUyMmCxWERTPjBsb3ZlJkjdLm5ldA==";
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var jwt = new JwtSecurityToken(
                issuer: _settings.Value.Iss,
                audience: _settings.Value.Aud,
                claims: claims,
                notBefore: now,
                expires: now.AddDays(30),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = now.AddDays(30),
                refresh_token = refresh_token,
                User = User,
            };

            //JObject JResponse = new JObject();
            //JResponse.Add("access_token", encodedJwt);
            //JResponse.Add("expires_in", (int)TimeSpan.FromMinutes(2).TotalSeconds);
            //JResponse.Add("refresh_token", refresh_token);

            return response;
        }

       
    }
}