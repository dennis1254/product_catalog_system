using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogSystem.Core.Models
{
    public class PaginatedResponse<T>:Response<List<T>> where T : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
       // public List<T> Data { get; set; }

        public PaginatedResponse(List<T> data, int pageNumber, int pageSize, int totalItems)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
            Data = data;
            Success = data.Any();
            Message = data.Any() ? "Successful" : "No records found";
            // Calculate total pages based on total items and page size
            TotalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);
        }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

}
