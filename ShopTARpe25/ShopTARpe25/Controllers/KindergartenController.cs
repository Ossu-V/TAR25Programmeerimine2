using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Kindergarten;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopTARpe25.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly ShopTARpe25Context _context;
        private readonly IKindergartenServices _kindergartenServices;
        private readonly IFileServices _fileServices;

        public KindergartenController
            (
                ShopTARpe25Context context,
                IKindergartenServices kindergartenServices,
                IFileServices fileServices
            )
        {
            _context = context;
            _kindergartenServices = kindergartenServices;
            _fileServices = fileServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _context.Kindergartens
                .Select(x => new KindergartenIndexViewModel
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
            var vm = new KindergartenCreateViewModel();
            return View("Create", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergartenCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var dto = new KindergartenDto
                {
                    GroupName = vm.GroupName,
                    ChildrenCount = vm.ChildrenCount,
                    KindergartenName = vm.KindergartenName,
                    TeacherName = vm.TeacherName,
                    Files = vm.Files?.GroupBy(f => f.FileName).Select(g => g.First()).ToList(),
                    FileToApiDtos = vm.Images?
                        .Select(file => new FileToApiDto
                        {
                            Id = file.ImageId,
                            ExistingFilePath = file.FilePath,
                            KindergartenId = file.KindergartenId
                        }).ToArray() ?? Array.Empty<FileToApiDto>()
                };

                var result = await _kindergartenServices.Create(dto);

                if (vm.Files != null && vm.Files.Count > 0)
                {
                    var latestKindergarten = await _context.Kindergartens
                        .OrderByDescending(x => x.CreatedAt)
                        .FirstOrDefaultAsync();

                    if (latestKindergarten != null)
                    {
                        dto.Id = latestKindergarten.Id;
                        var kindergartenEntity = new Kindergarten { Id = latestKindergarten.Id };
                        _fileServices.FilesToApi(dto, kindergartenEntity);
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarten = await _kindergartenServices.Details(id);
            if (kindergarten == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.KindergartenId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    KindergartenId = id
                }).ToArrayAsync();

            var vm = new KindergartenDetailsViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt,
            };

            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var kindergarten = await _kindergartenServices.Details(id);
            if (kindergarten == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.KindergartenId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    KindergartenId = id
                }).ToArrayAsync();

            var vm = new KindergartenUpdateViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt
            };

            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergartenUpdateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var dto = new KindergartenDto
                {
                    Id = vm.Id,
                    GroupName = vm.GroupName,
                    ChildrenCount = vm.ChildrenCount,
                    KindergartenName = vm.KindergartenName,
                    TeacherName = vm.TeacherName,
                    CreatedAt = vm.CreatedAt,
                    Files = vm.Files?.GroupBy(f => f.FileName).Select(g => g.First()).ToList(),
                    FileToApiDtos = vm.Images?
                        .Select(file => new FileToApiDto
                        {
                            Id = file.ImageId,
                            ExistingFilePath = file.FilePath,
                            KindergartenId = file.KindergartenId
                        }).ToArray() ?? Array.Empty<FileToApiDto>()
                };

                var result = await _kindergartenServices.Update(dto);

                if (result != null)
                {
                    if (vm.Files != null && vm.Files.Count > 0)
                    {
                        _fileServices.FilesToApi(dto, new Kindergarten { Id = vm.Id });
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarten = await _kindergartenServices.Details(id);
            if (kindergarten == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.KindergartenId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    KindergartenId = id
                }).ToArrayAsync();

            var vm = new KindergartenDeleteViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName
            };

            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var images = await _context.FileToApis.Where(x => x.KindergartenId == id).ToListAsync();

            foreach (var img in images)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "multipleFileUpload", img.ExistingFilePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                _context.FileToApis.Remove(img);
            }

            await _context.SaveChangesAsync();
            await _kindergartenServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(Guid imageId, Guid kindergartenId, string fromAction)
        {
            var image = await _context.FileToApis.FirstOrDefaultAsync(x => x.Id == imageId);

            if (image != null)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "multipleFileUpload", image.ExistingFilePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _context.FileToApis.Remove(image);
                await _context.SaveChangesAsync();
            }

            if (fromAction == "Delete")
            {
                return RedirectToAction(nameof(Delete), new { id = kindergartenId });
            }

            return RedirectToAction(nameof(Update), new { id = kindergartenId });
        }
    }
}
