using ProductCatalogSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogSystem.Infrastructure.Interfaces
{
    public interface ISwitchAPIService
    {
        Task<(List<SwitchResponse> Data, bool Success)> GetSwitches();
    }
}
