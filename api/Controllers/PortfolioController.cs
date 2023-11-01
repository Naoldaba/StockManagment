using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly PortfolioRepository _portfolioRepo;


        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, PortfolioRepository portfolioRepo)
        {
            _userManager=userManager;
            _stockRepo = stockRepo;
            _portfolioRepo =  portfolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            
            return Ok(userPortfolio);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username= User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if (stock == null) 
                return BadRequest("Stock not found");

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            
            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol)) 
                return BadRequest("Cannot add some stock to portfolio");

            var portfolioModel = new Portfolio{
                stockId = stock.Id,
                appUserId = appUser.Id
            };

            await _portfolioRepo.CreateAsync(portfolioModel);
            if (portfolioModel==null) {
                return StatusCode(500, "Could not create");
            }
            else{
                return Created();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            var filteredData = userPortfolio.Where(s=>s.Symbol.ToLower()==symbol.ToLower());

            if (filteredData.Count()==1){
                await _portfolioRepo.DeletePortfolio(appUser, symbol);
            }else{
                return BadRequest("Stock not in your portfolio");
            }
            
            return Ok();
        }
        
    }

}