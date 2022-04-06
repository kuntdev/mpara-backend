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
    [Authorize]
    public class FastTransferController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private ClaimModel _claimModel;

        public FastTransferController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FastTransferCreateRequest model)
        {
            _claimModel = MParaClaims.GetClaims(await HttpContext.GetTokenAsync("access_token"));

            var fastTransferFromRepo = _unitOfWork.FastTransferRepository.GetAll()
                                            .FirstOrDefault(x => x.AppUserId == _claimModel.ApiUserId
                                                        && x.NickName == model.NickName
                                                        && x.Receiver == model.Receiver
                                                        && x.IsActive);

            if (fastTransferFromRepo != null)
                return new JsonResult(new ApiResponse<bool>(ResponseType.Conflict, false));

            var fastTransfer = new Repositories.Entity.FastTransfer
            {
                Amount = model.Amount,
                AppUserId = _claimModel.ApiUserId,
                CreatedBy = _claimModel.ApiUserId,
                CreatedOn = DateTime.Now,
                IsActive = true,
                NickName = model.NickName,
                Receiver = model.Receiver,
                UpdateOn = DateTime.Now
            };

            _unitOfWork.FastTransferRepository.Add(fastTransfer);
            _unitOfWork.SaveChanges();
            return new JsonResult(new ApiResponse<bool>(ResponseType.Success, true));
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            _claimModel = MParaClaims.GetClaims(await HttpContext.GetTokenAsync("access_token"));

            var fastTransfersFromRepo = _unitOfWork.FastTransferRepository.GetAll()
                                            .Where(x => x.AppUserId == _claimModel.ApiUserId
                                                    && x.IsActive)
                                            .ToList();

            if (fastTransfersFromRepo == null && fastTransfersFromRepo.Count < 1)
                return new JsonResult(new ApiResponse<List<Repositories.Entity.FastTransfer>>(ResponseType.NotFound, null));
            return new JsonResult(new ApiResponse<List<Repositories.Entity.FastTransfer>>(ResponseType.Success, fastTransfersFromRepo));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] FastTransferCreateRequest model)
        {
            _claimModel = MParaClaims.GetClaims(await HttpContext.GetTokenAsync("access_token"));

            var fastTransferFromRepo = _unitOfWork.FastTransferRepository.GetAll()
                                            .FirstOrDefault(x => x.AppUserId == _claimModel.ApiUserId
                                                        && x.NickName == model.NickName
                                                        && x.Receiver == model.Receiver);

            if (fastTransferFromRepo == null)
                return new JsonResult(new ApiResponse<bool>(ResponseType.Conflict, false));

            fastTransferFromRepo.IsActive = false;
            fastTransferFromRepo.UpdateOn = DateTime.Now;
            
            _unitOfWork.SaveChanges();
            return new JsonResult(new ApiResponse<bool>(ResponseType.Success, true));
        }



    }
}
