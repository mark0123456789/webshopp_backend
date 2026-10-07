using Computer_shopp.modells;
using Computer_shopp.modells.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;

namespace Computer_shopp.Controllers
{
    [Route("osystem")]
    [ApiController]
    public class OsistemController : ControllerBase
    {
        public CumputerShoppDbContext context = new CumputerShoppDbContext();
        [HttpGet("GetAll")]
        public object GetAllOsystems()
        {
            var Osystems = context.Osystems.ToList();
            return new { message = "sikeres lekérdezés", Result = Osystems };
        }
    

    [HttpPost]
        public object AddNewOsystem(AddNewSystemDTO addNewSystemDTO) {
            
            
                var osystems = new Osystem()
                {
                    Id = Guid.NewGuid(),
                    Name = addNewSystemDTO.Name,
                    version = addNewSystemDTO.version,
                    RegisterTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
            return StatusCode(201, new { message = "sikeres hozzáadás", Result = osystems });
        }

        [HttpPut]
        public object UpdateOsystem([FromQuery] Guid id, [FromBody]UpdateNewsystemDTo updateNewSystemDTO)
        {
            var osystem = context.Osystems.FirstOrDefault(osystem => osystem.Id == id);
            if (osystem != null)
            {
                osystem.Name = updateNewSystemDTO.Name;
                osystem.version = updateNewSystemDTO.version;
                osystem.UpdateTime = DateTime.Now;
                context.Osystems.Update(osystem);
                context.SaveChanges();
                return StatusCode(200, new { message = "sikeres frissítés", Result = osystem });
            }
            return StatusCode(404, new { message = "nem található a rendszer", Result = osystem });
        }
    }
}
