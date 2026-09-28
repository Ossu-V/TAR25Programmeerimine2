using ShopTARpe25.Core.Dto;
using System;
using System.Threading.Tasks;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IKindergartenServices
    {
        Task<KindergartenDto> Create(KindergartenDto dto);
        Task<KindergartenDto> Update(KindergartenDto dto);
        Task<KindergartenDto> Delete(Guid id);
        Task<KindergartenDto> GetAsync(Guid id);
    }
}
