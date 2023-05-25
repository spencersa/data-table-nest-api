using System.IdentityModel.Tokens.Jwt;

namespace TablesLibrary
{
    public static class AuthorizationHelpers
    {
        public static JwtSecurityToken DecodeJwtToken(string token)
        {
            if (token.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase)) {
                token = token.Substring("Bearer ".Length);
            }
            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }
    }
}
