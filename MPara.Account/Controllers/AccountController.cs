using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MPara.Account.Models;
using MPara.Repositories.Abstract;
using MPara.Repositories.Models;

namespace MPara.Account.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private ClaimModel _claimModel;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(AccountCreateRequest model)
        {
            _claimModel = MParaClaims.GetClaims(await HttpContext.GetTokenAsync("access_token"));

            var accountFromRepo = _unitOfWork.AccountRepository
                                                .GetAll()
                                                .FirstOrDefault(x => x.AppUserId == _claimModel.ApiUserId
                                                    && x.Type == model.Type.ToString()
                                                    && x.Currency == model.Currency.ToString());


            if (accountFromRepo != null)
                return new JsonResult(new ApiResponse<Repositories.Entity.Account>(ResponseType.Undone,
                                                accountFromRepo));

            var rand = new Random();
            int branchCode = rand.Next(1000, 5000);
            int number = rand.Next(100000, 500000);

            _unitOfWork.AccountRepository.Add(new Repositories.Entity.Account
            {
                Amount = 0,
                AppUserId = _claimModel.ApiUserId,
                BranchCode = branchCode,
                CreatedBy = _claimModel.ApiUserId,
                CreatedOn = DateTime.Now,
                Currency = model.Currency.ToString(),
                Number = number,
                Type = model.Type.ToString(),
                UpdateOn = DateTime.Now
            });

            _unitOfWork.SaveChanges();

            accountFromRepo = _unitOfWork.AccountRepository
                                                .GetAll()
                                                .FirstOrDefault(x => x.AppUserId == _claimModel.ApiUserId
                                                    && x.Type == model.Type.ToString()
                                                    && x.Currency == model.Currency.ToString());

            return new JsonResult(new ApiResponse<Repositories.Entity.Account>(ResponseType.Success,
                                        accountFromRepo));

        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int accountId)
        {

            var accountFromRepo = await GetAccount(accountId);

            if (accountFromRepo == null)
                return new JsonResult(new ApiResponse<bool>(ResponseType.NotFound,
                                false));

            accountFromRepo.IsActive = false;
            accountFromRepo.UpdateOn = DateTime.Now;
            _unitOfWork.AccountRepository.Update(accountFromRepo);
            _unitOfWork.SaveChanges();

            return new JsonResult(new ApiResponse<bool>(ResponseType.Success,
                               true));

        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Activate(int accountId)
        {
            var accountFromRepo = await GetAccount(accountId);

            if (accountFromRepo == null)
                return new JsonResult(new ApiResponse<bool>(ResponseType.NotFound,
                                false));

            accountFromRepo.IsActive = true;
            accountFromRepo.UpdateOn = DateTime.Now;
            _unitOfWork.AccountRepository.Update(accountFromRepo);
            _unitOfWork.SaveChanges();

            return new JsonResult(new ApiResponse<bool>(ResponseType.Success,
                               true));

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            _claimModel = MParaClaims.GetClaims(await HttpContext.GetTokenAsync("access_token"));

            var accountsFromRepo = _unitOfWork.AccountRepository
                                                .GetAll()
                                                .Where(x => x.AppUserId == _claimModel.ApiUserId)
                                                .ToList();

            if (accountsFromRepo.Count < 1)
                return new JsonResult(new ApiResponse<List<Repositories.Entity.Account>>(ResponseType.NotFound,
                                null));

            return new JsonResult(new ApiResponse<List<Repositories.Entity.Account>>(ResponseType.Success,
                               accountsFromRepo));

        }

        private async Task<Repositories.Entity.Account> GetAccount(int accountId)
        {

            _claimModel = MParaClaims.GetClaims(await HttpContext.GetTokenAsync("access_token"));

            return _unitOfWork.AccountRepository
                                    .GetAll()
                                    .FirstOrDefault(x => x.AppUserId == _claimModel.ApiUserId
                                    && x.AccountId == accountId);
        }

    }
}
