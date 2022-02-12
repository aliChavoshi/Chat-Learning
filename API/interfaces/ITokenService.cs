using System.Threading.Tasks;
using API.Entities;

namespace API.interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(Users user);
    }
}