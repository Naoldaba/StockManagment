using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;

        public PortfolioRepository(ApplicationDBContext context)
        {
            _context=context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            
            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(x=>x.appUserId == appUser.Id && x.stock.Symbol.ToLower()==symbol.ToLower());
            if (portfolioModel==null)
                return null;
            
            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(u => u.appUserId == user.Id)
                    .Select(stock => new Stock{
                        Id=stock.stockId,
                        Symbol = stock.stock.Symbol,
                        CompanyName = stock.stock.CompanyName,
                        Purchase = stock.stock.Purchase,
                        LastDiv = stock.stock.LastDiv,
                        Industry = stock.stock.Industry,
                        MarketCapital = stock.stock.MarketCapital
            }).ToListAsync();
        }
    }
}