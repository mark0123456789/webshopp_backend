using System;
using System.Linq;
using Computer_shopp.modells;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Computer_shopp.Controllers
{
    [Route("api/query")]
    [ApiController]
    public class TovabbiFeladatokController : ControllerBase
    {
            private readonly CumputerShoppDbContext context = new CumputerShoppDbContext();

            [HttpGet("os")]
            public IActionResult GetAllOsWithComputers()
            {
                var list = context.Osystems
                    .Include(o => o.Computers)
                    .ToList();
                return Ok(new { message = "sikeres lekérdezés", Result = list });
            }

            [HttpGet("computers")]
            public IActionResult GetAllComputersWithOs()
            {
                var list = context.Computers
                    .Include(c => c.Osys)
                    .ToList();
                return Ok(new { message = "sikeres lekérdezés", Result = list });
            }

            [HttpGet("os/{id}")]
            public IActionResult GetOsById(Guid id)
            {
                var os = context.Osystems
                    .Include(o => o.Computers)
                    .FirstOrDefault(o => o.Id == id);
                if (os == null) return NotFound(new { message = "Nincs ilyen id", Result = (object?)null });
                return Ok(new { message = "sikeres lekérdezés", Result = os });
            }

            [HttpGet("computer/{id}")]
            public IActionResult GetComputerByIdWithOs(Guid id)
            {
                var computer = context.Computers
                    .Include(c => c.Osys)
                    .FirstOrDefault(c => c.ID == id);
                if (computer == null) return NotFound(new { message = "Nincs ilyen id", Result = (object?)null });

                var result = new
                {
                    computer.ID,
                    computer.Brand,
                    computer.Type,
                    computer.Display,
                    computer.memory,
                    computer.CreatedTime,
                    OsystemName = computer.Osys?.Name,
                    OsystemVersion = computer.Osys?.version
                };
                return Ok(new { message = "sikeres lekérdezés", Result = result });
            }

            [HttpGet("os-computer-count")]
            public IActionResult GetOsComputerCounts()
            {
                var list = context.Osystems
                    .Select(o => new
                    {
                        o.Id,
                        o.Name,
                        ComputerCount = o.Computers == null ? 0 : o.Computers.Count
                    })
                    .ToList();
                return Ok(new { message = "sikeres lekérdezés", Result = list });
            }

            [HttpGet("brand/{brand}")]
            public IActionResult GetByBrand(string brand)
            {
                var list = context.Computers
                    .Include(c => c.Osys)
                    .Where(c => c.Brand != null && c.Brand.ToLower() == brand.ToLower())
                    .ToList();
                return Ok(new { message = "sikeres lekérdezés", Result = list });
            }

            [HttpGet("computers-sorted")]
            public IActionResult GetComputersSorted()
            {
                var list = context.Computers
                    .Include(c => c.Osys)
                    .OrderBy(c => c.Brand)
                    .ThenBy(c => c.Type)
                    .ToList();
                return Ok(new { message = "sikeres lekérdezés", Result = list });
            }

            [HttpGet("min-display/{size}")]
            public IActionResult GetByMinDisplay(double size)
            {
                var list = context.Computers
                    .Include(c => c.Osys)
                    .Where(c => c.Display > size)
                    .ToList();
                return Ok(new { message = "sikeres lekérdezés", Result = list });
            }

            [HttpGet("os-stats")]
            public IActionResult GetOsStats()
            {
                var stats = context.Osystems
                    .Select(o => new
                    {
                        o.Id,
                        o.Name,
                        ComputerCount = o.Computers == null ? 0 : o.Computers.Count,
                        AverageMemory = o.Computers == null || !o.Computers.Any() ? 0 : o.Computers.Average(c => c.memory),
                        MaxMemory = o.Computers == null || !o.Computers.Any() ? 0 : o.Computers.Max(c => c.memory),
                        AverageDisplay = o.Computers == null || !o.Computers.Any() ? 0 : o.Computers.Average(c => c.Display)
                    })
                    .ToList();
                return Ok(new { message = "sikeres lekérdezés", Result = stats });
            }

            [HttpGet("by-os")]
            public IActionResult GetComputersByOs()
            {
                var groups = context.Computers
                    .Include(c => c.Osys)
                    .AsEnumerable()
                    .GroupBy(c => c.Osys?.Name ?? "(No OS)")
                    .Select(g => new { OsName = g.Key, Computers = g.ToList() })
                    .ToList();
                return Ok(new { message = "sikeres lekérdezés", Result = groups });
            }

            [HttpGet("os-without-computers")]
            public IActionResult GetOsWithoutComputers()
            {
                var list = context.Osystems
                    .Include(o => o.Computers)
                    .Where(o => o.Computers == null || !o.Computers.Any())
                    .ToList();
                return Ok(new { message = "sikeres lekérdezés", Result = list });
            }

            [HttpGet("max-memory-per-os")]
            public IActionResult GetMaxMemoryPerOs()
            {
                var data = context.Osystems
                    .Include(o => o.Computers)
                    .Select(o => new
                    {
                        o.Id,
                        o.Name,
                        TopComputer = o.Computers == null || !o.Computers.Any() ? null : o.Computers.OrderByDescending(c => c.memory).FirstOrDefault()
                    })
                    .ToList();
                return Ok(new { message = "sikeres lekérdezés", Result = data });
            }

            [HttpGet("filter")]
            public IActionResult Filter([FromQuery] int? minMemory, [FromQuery] int? maxMemory, [FromQuery] int? minOsVersion)
            {
                var query = context.Computers.Include(c => c.Osys).AsQueryable();
                if (minMemory.HasValue) query = query.Where(c => c.memory >= minMemory.Value);
                if (maxMemory.HasValue) query = query.Where(c => c.memory <= maxMemory.Value);
                if (minOsVersion.HasValue) query = query.Where(c => c.Osys != null && c.Osys.version >= minOsVersion.Value);
                var list = query.ToList();
                return Ok(new { message = "sikeres lekérdezés", Result = list });
            }

            [HttpGet("latest")]
            public IActionResult GetLatest([FromQuery] int take = 5)
            {
                var list = context.Computers
                    .Include(c => c.Osys)
                    .OrderByDescending(c => c.CreatedTime)
                    .Take(take)
                    .ToList();
                return Ok(new { message = "sikeres lekérdezés", Result = list });
            }

            [HttpGet("brand-summary")]
            public IActionResult GetBrandSummary()
            {
                var summary = context.Computers
                    .GroupBy(c => c.Brand)
                    .Select(g => new
                    {
                        Brand = g.Key,
                        Count = g.Count(),
                        AverageMemory = g.Average(c => c.memory),
                        AverageDisplay = g.Average(c => c.Display)
                    })
                    .ToList();
                return Ok(new { message = "sikeres lekérdezés", Result = summary });
            }
        }
    }


