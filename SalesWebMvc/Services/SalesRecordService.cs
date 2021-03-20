using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? min, DateTime? max)
        {
            var result = from obj in _context.SalesRecord select obj;

            if (min.HasValue)
            {
                result = result.Where(sr => sr.Date >= min.Value);
            }
            if (max.HasValue)
            {
                result = result.Where(sr => sr.Date <= max.Value);
            }

            return await result
                .Include(sr => sr.Seller)
                .Include(sr => sr.Seller.Department)
                .OrderByDescending(sr => sr.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? min, DateTime? max)
        {
            var result = from obj in _context.SalesRecord select obj;

            if (min.HasValue)
            {
                result = result.Where(sr => sr.Date >= min.Value);
            }
            if (max.HasValue)
            {
                result = result.Where(sr => sr.Date <= max.Value);
            }

            return await result
                .Include(sr => sr.Seller)
                .Include(sr => sr.Seller.Department)
                .OrderByDescending(sr => sr.Date)
                .GroupBy(sr => sr.Seller.Department)
                .ToListAsync();
        }
    }
}
