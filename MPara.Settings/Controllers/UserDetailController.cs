using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPara.Repositories.Abstract;
using MPara.Repositories.Models;
using MPara.Settings.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MPara.Settings.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class UserDetailController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private ClaimModel _claimModel;

        public UserDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {            
            var userDetailsFromRepo = await GetUserDetail();

            if (userDetailsFromRepo == null)
                return new JsonResult(new ApiResponse<Repositories.Entity.UserDetail>(ResponseType.NotFound, null));

            return new JsonResult(new ApiResponse<Repositories.Entity.UserDetail>(ResponseType.Success, userDetailsFromRepo));

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDetailRequest model)
        {
            var userDetailsFromRepo = await GetUserDetail();

            if (userDetailsFromRepo != null)
                return new JsonResult(new ApiResponse<bool>(ResponseType.Undone, false,
                                "Oturum icin yaratilmis olan bir detay bilgisi vardir."));


            var userDetailsToAdd = new Repositories.Entity.UserDetail
            {
                AppUserId = _claimModel.ApiUserId,
                Birthdate = model.Birthdate,
                CreatedBy = _claimModel.ApiUserId,
                CreatedOn = DateTime.Now,
                Gender = model.Gender,
                IsActive = true,
                Name = model.Name,
                Surname = model.Surname,
                UpdateOn = DateTime.Now,
            };

            _unitOfWork.UserDetailRepository.Add(userDetailsToAdd);
            _unitOfWork.SaveChanges();
            return new JsonResult(new ApiResponse<bool>(ResponseType.Success, true));

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserDetailRequest model)
        {

            var userDetailsFromRepo = await GetUserDetail();

            if (userDetailsFromRepo == null)
                return new JsonResult(new ApiResponse<Repositories.Entity.UserDetail>(ResponseType.NotFound, null));

            userDetailsFromRepo.Birthdate = model.Birthdate;
            userDetailsFromRepo.Gender = model.Gender;
            userDetailsFromRepo.Name = model.Name;
            userDetailsFromRepo.Surname = model.Surname;
            userDetailsFromRepo.UpdateOn = DateTime.Now;
            _unitOfWork.SaveChanges();

            return new JsonResult(new ApiResponse<bool>(ResponseType.Success, true));

        }


        private async Task<Repositories.Entity.UserDetail> GetUserDetail()
        {
            _claimModel = MParaClaims.GetClaims(await HttpContext.GetTokenAsync("access_token"));

            return _unitOfWork.UserDetailRepository
                                            .GetAll()
                                            .FirstOrDefault(x => x.AppUserId == _claimModel.ApiUserId);

        }


    }
}
