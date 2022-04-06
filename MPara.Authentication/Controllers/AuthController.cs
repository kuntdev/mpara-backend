using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MPara.Authentication.Helper;
using MPara.Authentication.Models;
using MPara.Repositories.Abstract;

namespace MPara.Authentication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IUnitOfWork _unitOfWork;
        private TokenHelper _tokenHelper;

        public AuthController(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            Init();
        }

        private void Init()
        {
            _tokenHelper = new TokenHelper(_configuration, _unitOfWork);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Token(AppUserModel model)
        {
            IActionResult response = Unauthorized();
            var user = _tokenHelper.AuthenticateUser(model);

            if (user != null)
            {
                var tokenString = _tokenHelper.CreateAccessToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }


        
    }
}
