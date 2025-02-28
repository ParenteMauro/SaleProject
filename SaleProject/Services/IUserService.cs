

using SaleProject.Models.Request;
using SaleProject.Models.Response;

namespace SaleProject.Services
{
    public interface IUserService
    {
        Task<UserResponse> Auth(AuthRequest model);
    }
}
