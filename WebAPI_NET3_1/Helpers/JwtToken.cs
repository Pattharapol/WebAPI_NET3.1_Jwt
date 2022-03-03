using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI_NET3_1.Helpers
{
    public class JwtToken
    {

        private const string SECRET_KEY = "TQvgjeABMPOwCycOqah5EQu5yyVjpmVGTQvgjeABMPOwCycOqah5EQu5yyVjpmVGTQvgjeABMPOwCycOqah5EQu5yyVjpmVG";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new
            SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));
        public static string GenerateJwtToken()
        {
            // Also note that securityKey length should be >256b
            // so you have to make sure that your private key has a proper length            
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials
                (SIGNING_KEY, SecurityAlgorithms.HmacSha256);

            //  Finally create a Token
            var header = new JwtHeader(credentials);

            // Token will be good for 1 minutes
            DateTime Expiry = DateTime.UtcNow.AddMinutes(1);
            int ts = (int)(Expiry - new DateTime(1970, 1, 1)).TotalSeconds;

            //Some PayLoad that contain information about the  customer
            var payload = new JwtPayload
            {
                { "sub", "testSubject"},
                { "Name", "Scott"},
                { "email", "smithtest@tesst.com"},               
                //, { "nbf", new DateTimeOffset(DateTime.Now).DateTime},
                { "exp", ts},
                { "iss", "https://localhost:44346"}, //Issuer - the party generating the JWT
                { "aud", "https://localhost:44346"} //Audience - The address of the resource
            };

            var secToken = new JwtSecurityToken(header, payload);

            var handler = new JwtSecurityTokenHandler();

            var tokenString = handler.WriteToken(secToken);//securityToken

            Console.WriteLine(tokenString);
            Console.WriteLine("Consume Token");

            return tokenString;
        }


    }
}
