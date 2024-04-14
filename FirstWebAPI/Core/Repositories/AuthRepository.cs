using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using FirstWebAPI.Core.Context;
using FirstWebAPI.Core.Contracts;
using FirstWebAPI.Core.Entities;
using static System.Collections.Specialized.BitVector32;

namespace FirstWebAPI.Core.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(ApplicationDbContext context ,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var responce = new ServiceResponse<string>();
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));
            if (user is null) 
            {
                responce.Success = false;
                responce.Message = "IUsername or Password is Wrong";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash , user.PasswordSalt))
            {
                responce.Success = false;
                responce.Message = "Username or IPassword is Wrong";
            }
            else
            {
                responce.Data = CreateToken(user);
            }
            return responce;
        }

        public async Task<bool> UserExists (string username)
        {
            if (await _context.Users.AnyAsync(u => u.Username.ToLower() == username)) 
            {
                return true;
            }
            else
                return false;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var resonse = new ServiceResponse<int>();
            if (await UserExists(user.Username))
            {
                resonse.Success = false;
                resonse.Message = "User already exists.";
                return resonse;

            }
            CreatePasswordHash(password , out byte[] passwordHash , out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            resonse.Data = user.Id;
            return resonse;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash (string password, byte[] passwordHash , byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) 
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user)
        {

           // Defining a list of Claim items: In this section, two Claim items are defined for the token.
           // A Claim consists of a Type and a Value.The two Claim items added to the claims list
           // represent the NameIdentifier and the Username of the user, respectively.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            // Reading token settings from the configuration
            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
            if(appSettingsToken is null)
            {
                throw new Exception("AppSetings Token is null");
            }

            //Creating a security key for the token: In this section,
            //the token settings value(appSettingsToken) is transformed into a security key.
            //This security key is used to sign the token and is based on the HmacSha512Signature algorithm.
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(appSettingsToken));

            //this part defines the settings for the security key and user information in a SecurityTokenDescriptor object.The settings include:
            //Subject: A ClaimsIdentity object containing the claims list.
            //Expires: The token's expiration date. In this case, the token expires one day after its creation.
            //SigningCredentials: Uses the security key(key) and signing algorithm(SecurityAlgorithms.HmacSha512Signature).
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            //Creating and writing the token: This section creates the token and returns it as a string.
            //First, an instance of JwtSecurityTokenHandler is created.
            //Then, using this instance, the token is created using the token settings(tokenDescriptor).
            //Finally, the generated token is returned as a string using tokenHandler.WriteToken(token).
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
