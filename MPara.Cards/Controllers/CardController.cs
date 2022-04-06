using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MPara.Cards.Models;
using MPara.Repositories.Abstract;
using MPara.Repositories.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MPara.Cards.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class CardController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IConfiguration _configuration;
        private ClaimModel _claimModel;

        public CardController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCardRequest model)
        {
            _claimModel = MParaClaims.GetClaims(await HttpContext.GetTokenAsync("access_token"));

            var cardFromRepo = _unitOfWork.CardRepository.GetAll()
                                        .FirstOrDefault(x => x.AppUserId == _claimModel.ApiUserId
                                                        && x.Type == model.Type);

            if (cardFromRepo != null)
                return new JsonResult(new ApiResponse<bool>(ResponseType.Undone,
                                                false, "Secilen kart tipinde bir karta sahipsiniz."));

            var rand = new Random();
            var cvv = rand.Next(100, 999);
            var expireDate = rand.Next(1, 12).ToString() + "/" + rand.Next(22, 27).ToString();
            var number = rand.NextInt64(1000023041012345, 9999999999999999).ToString();
            var limit = Int32.Parse(_configuration["DefaultCardInfo:Limit"]);

            var cardToAdd = new Repositories.Entity.Card
            {
                AppUserId = _claimModel.ApiUserId,
                AvaliableLimit = limit,
                CreatedBy = _claimModel.ApiUserId,
                CreatedOn = DateTime.Now,
                Cvv = cvv,
                ExpireDate = expireDate,
                IsActive = true,
                Limit = limit,
                Number = number,
                Type = model.Type,
                UpdateOn = DateTime.Now
            };

            _unitOfWork.CardRepository.Add(cardToAdd);
            _unitOfWork.SaveChanges();
            return new JsonResult(new ApiResponse<bool>(ResponseType.Success, true));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int cardId)
        {
            var cardFromRepo = await GetCard(cardId);

            if (cardFromRepo == null)
                return new JsonResult(new ApiResponse<bool>(ResponseType.NotFound,
                                                false, "Iletilen kart bulunamadi."));

            cardFromRepo.IsActive = false;
            cardFromRepo.UpdateOn = DateTime.Now;
            _unitOfWork.SaveChanges();
            return new JsonResult(new ApiResponse<bool>(ResponseType.Success, true));
        }

        [HttpPut]
        public async Task<IActionResult> Activate(int cardId)
        {
            var cardFromRepo = await GetCard(cardId);

            if (cardFromRepo == null)
                return new JsonResult(new ApiResponse<bool>(ResponseType.NotFound,
                                                false, "Iletilen kart bulunamadi."));

            cardFromRepo.IsActive = true;
            cardFromRepo.UpdateOn = DateTime.Now;
            _unitOfWork.SaveChanges();
            return new JsonResult(new ApiResponse<bool>(ResponseType.Success, true));
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            _claimModel = MParaClaims.GetClaims(await HttpContext.GetTokenAsync("access_token"));

            var cardsFromRepo = _unitOfWork.CardRepository.GetAll()
                                        .Where(x => x.AppUserId == _claimModel.ApiUserId)
                                        .ToList();

            if (cardsFromRepo.Count < 1)
                return new JsonResult(new ApiResponse<List<Repositories.Entity.Card>>(ResponseType.NotFound,
                                                null, "Tanimli bir kart bulunamadi"));

            return new JsonResult(new ApiResponse<List<Repositories.Entity.Card>>(ResponseType.Success, cardsFromRepo));
        }

        private async Task<Repositories.Entity.Card> GetCard(int cardId)
        {
            _claimModel = MParaClaims.GetClaims(await HttpContext.GetTokenAsync("access_token"));

            return _unitOfWork.CardRepository.GetAll()
                                        .FirstOrDefault(x => x.AppUserId == _claimModel.ApiUserId
                                                        && x.CardId == cardId);
        }

    }
}
