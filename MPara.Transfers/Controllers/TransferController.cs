using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPara.Repositories.Abstract;
using MPara.Repositories.Models;
using MPara.Transfers.Models;

namespace MPara.Transfers.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TransferController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private ClaimModel _claimModel;

        public TransferController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransferCreateRequest model)
        {
            _claimModel = MParaClaims.GetClaims(await HttpContext.GetTokenAsync("access_token"));

            var account = _unitOfWork.AccountRepository.GetAll()
                                        .FirstOrDefault(x => x.AccountId == model.AccountId
                                                    && x.AppUserId == _claimModel.ApiUserId);

            if (account == null) { }
            if (account.Amount <= model.Amount)
                return new JsonResult(new ApiResponse<bool>(ResponseType.Exception, false, "Yetersiz bakiye"));


            var transferToAdd = new Repositories.Entity.Transfer
            {
                AccountId = model.AccountId,
                Amount = model.Amount,
                CreatedBy = _claimModel.ApiUserId,
                CreatedOn = DateTime.Now,
                IsActive = true,
                Receiver = model.Receiver,
                Sender = model.AccountId,
                UpdateOn = DateTime.Now
            };

            account.Amount -= model.Amount;
            _unitOfWork.TransferRepository.Add(transferToAdd);
            _unitOfWork.SaveChanges();
            return new JsonResult(new ApiResponse<bool>(ResponseType.Success, true));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> List([FromBody] TransferListRequest model)
        {
            _claimModel = MParaClaims.GetClaims(await HttpContext.GetTokenAsync("access_token"));

            if (model.From == null)
                model.From = DateTime.Now.AddDays(-7);

            if (model.To == null)
                model.To = DateTime.Now;

            var account = _unitOfWork.AccountRepository.GetAll()
                                                    .FirstOrDefault(x => x.AppUserId == _claimModel.ApiUserId
                                                    && x.AccountId == model.AccountId);

            if (account == null)
            {
                return new JsonResult(new ApiResponse<List<Repositories.Entity.Transfer>>(ResponseType.NotFound,
                                null, "Ilgili hesap bu kullaniciya ait degildir."));
            }

            var transfers = _unitOfWork.TransferRepository.GetAll()
                                            .Where(x => x.AccountId == account.AccountId
                                            && x.CreatedOn >= model.From
                                            && x.CreatedOn <= model.To)
                                            .ToList();

            return new JsonResult(new ApiResponse<List<Repositories.Entity.Transfer>>(ResponseType.Success, transfers));

        }

    }
}
