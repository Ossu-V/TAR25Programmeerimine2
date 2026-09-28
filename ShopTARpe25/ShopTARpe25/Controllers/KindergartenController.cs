using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopTARpe25.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly ShopTARpe25Context _context;
        private readonly IKindergartenServices _kindergartenServices;

        public KindergartenController
        (
            ShopTARpe25Context context,
            IKindergartenServices kindergartenServices
        )
        {
            _context = context;
            _kindergartenServices = kindergartenServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _context.Kindergartens
                .Select(x => new KindergartenDto
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergartenName = x.KindergartenName,
                    TeacherName = x.TeacherName,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergartenDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _kindergartenServices.Create(dto);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarten = await _kindergartenServices.GetAsync(id);
            if (kindergarten == null)
            {
                return NotFound();
            }

            return View(kindergarten);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var kindergarten = await _kindergartenServices.GetAsync(id);
            if (kindergarten == null)
            {
                return NotFound();
            }

            return View(kindergarten);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergartenDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _kindergartenServices.Update(dto);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarten = await _kindergartenServices.GetAsync(id);
            if (kindergarten == null)
            {
                return NotFound();
            }

            return View(kindergarten);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _kindergartenServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
