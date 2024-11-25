using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository.Common
{
    public interface IRoleTypeRepository
    {
        Task<String> GetByIdAsync(Guid id);
    }
}
