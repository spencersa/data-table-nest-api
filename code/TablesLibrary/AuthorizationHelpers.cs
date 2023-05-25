using System.IdentityModel.Tokens.Jwt;

namespace TablesLibrary
{
    public static class AuthorizationHelpers
    {
        public static JwtSecurityToken DecodeJwtToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }
    }
}
