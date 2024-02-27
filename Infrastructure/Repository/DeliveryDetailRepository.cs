using Infrastructure.EF;
using Infrastructure.Entities;
using Infrastructure.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IDeliveryDetailRepository : IRepository<DeliveryDetail>
    {
    }

    public class DeliveryDetailRepository : Repository<DeliveryDetail>, IDeliveryDetailRepository
    {
        public DeliveryDetailRepository(EXDbContext context) : base(context)
        {
        }
    }
    
}
