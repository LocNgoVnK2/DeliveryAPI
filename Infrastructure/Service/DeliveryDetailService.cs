using Infrastructure.Entities;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface IDeliveryDetailService
    {
        Task<IQueryable<DeliveryDetail>> GetDeliveryDetails();
        Task<DeliveryDetail> GetDeliveryDetail(int id);
        Task<List<DeliveryDetail>> GetDeliveryDetailCompleteByAccountId(int id);
        Task InsertDeliveryDetail(DeliveryDetail deliveryDetail);
        Task UpdateDeliveryDetail(DeliveryDetail deliveryDetail);
        Task DeleteDeliveryDetail(DeliveryDetail deliveryDetail);
    }

    public class DeliveryDetailService : IDeliveryDetailService
    {
        private IDeliveryDetailRepository deliveryDetailRepository;

        public DeliveryDetailService(IDeliveryDetailRepository deliveryDetailRepository)
        {
            this.deliveryDetailRepository = deliveryDetailRepository;
        }

        public async Task<IQueryable<DeliveryDetail>> GetDeliveryDetails()
        {
            return await Task.FromResult(deliveryDetailRepository.GetAll());
        }

        public async Task<DeliveryDetail> GetDeliveryDetail(int id)
        {
            return await deliveryDetailRepository.GetByIdAsync(id);
        }
        public async Task<List<DeliveryDetail>> GetDeliveryDetailCompleteByAccountId(int id)
        {
            var res = await Task.FromResult(deliveryDetailRepository.GetAll());
            List<DeliveryDetail> deliveryDetail = res.Where(e=>e.AccountID == id && e.DeliveryStatus == true).ToList();
            return deliveryDetail;
        }
        public async Task InsertDeliveryDetail(DeliveryDetail deliveryDetail)
        {
            await deliveryDetailRepository.InsertAsync(deliveryDetail);
        }

        public async Task UpdateDeliveryDetail(DeliveryDetail deliveryDetail)
        {
            await deliveryDetailRepository.UpdateAsync(deliveryDetail);
        }

        public async Task DeleteDeliveryDetail(DeliveryDetail deliveryDetail)
        {
            await deliveryDetailRepository.DeleteAsync(deliveryDetail);
        }
    }
}
