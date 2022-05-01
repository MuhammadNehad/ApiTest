
using ApiTest.Models;

namespace ApiTest.Interfaces
{
    public interface JwtInterface
    {
        string generateJwt(UserModel user);
    }

    public interface JwtValidationInterface
    {
        bool validate(string username, string token);
    }
}
