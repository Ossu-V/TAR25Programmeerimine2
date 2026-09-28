using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using System;
using System.Threading.Tasks;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly ShopTARpe25Context _context;

        public KindergartenServices(ShopTARpe25Context context)
        {
            _context = context;
        }

        public async Task<KindergartenDto> Create(KindergartenDto dto)
        {
            Kindergarten domain = new()
            {
                Id = Guid.NewGuid(),
                GroupName = dto.GroupName,
                ChildrenCount = dto.ChildrenCount,
                KindergartenName = dto.KindergartenName,
                TeacherName = dto.TeacherName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.Kindergartens.AddAsync(domain);
            await _context.SaveChangesAsync();

            return dto;
        }

        public async Task<KindergartenDto> Update(KindergartenDto dto)
        {
            var domain = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (domain == null) return null;

            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.KindergartenName = dto.KindergartenName;
            domain.TeacherName = dto.TeacherName;
            domain.UpdatedAt = DateTime.Now;

            _context.Kindergartens.Update(domain);
            await _context.SaveChangesAsync();

            return dto;
        }

        public async Task<KindergartenDto> Delete(Guid id)
        {
            var domain = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            if (domain == null) return null;

            _context.Kindergartens.Remove(domain);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<KindergartenDto> GetAsync(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null) return null;

            var dto = new KindergartenDto
            {
                Id = result.Id,
                GroupName = result.GroupName,
                ChildrenCount = result.ChildrenCount,
                KindergartenName = result.KindergartenName,
                TeacherName = result.TeacherName,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt
            };

            return dto;
        }
    }
}
