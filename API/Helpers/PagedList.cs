using System.Reflection.Metadata.Ecma335;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> where T : class
    {
        public PagedList(IEnumerable<T> items, int pageNumber, int pageSize, int count)
        {
            this.TotalPage = (int)Math.Ceiling(count / (double)pageSize); // 20/10 = 2;
            this.CurrentPage = pageNumber;
            this.PageSize = pageSize;
            this.TotalCount = count;
            this.items = items;
        }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> items { get; set; } = new List<T>();
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, pageNumber, pageSize, count);
        }
    }
}