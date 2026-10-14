using Computer_shopp.modells;
using Computer_shopp.modells.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Computer_shopp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputersController : ControllerBase
    {
        public CumputerShoppDbContext context = new CumputerShoppDbContext();
        [HttpGet("GetAll")]
        public object GetAllComputers()
        {
            var Computers = context.Computers.ToList();
            return new { message = "sikeres lekérdezés", Result = Computers };
        }
        [HttpGet("{id}")]
        public object GetComputerById(Guid id)
        {
            var computer = context.Computers.FirstOrDefault(computer => computer.ID == id);
            if (computer != null)
            {
                return new { message = "sikeres lekérdezés", Result = computer };
            }
            return StatusCode(404, new { message = "sikertelen lekérdezés", Result = computer });
        }
        [HttpPost]
        public object AddNewcomputer(AddNewComputerDTO addNewComputerDTO)
        {
            var Computers = new Computer()
            {
                ID = Guid.NewGuid(),
                Brand = addNewComputerDTO.Brand,
                Type = addNewComputerDTO.Type,
                Display = addNewComputerDTO.Display,
                memory = addNewComputerDTO.memory,
                Osys = addNewComputerDTO.Osys,
                CreatedTime = DateTime.Now,
            };
            return StatusCode(201, new { message = "sikeres hozzáadás", Result = Computers });
        }

        [HttpPut]
        public object UpdateComputer([FromQuery] Guid id, [FromBody] UpdateNewComputerDTO updateNewComputerDTO)
        {
            var computer = context.Computers.FirstOrDefault(computer => computer.ID == id);
            if (computer != null)
            {
                computer.Brand = updateNewComputerDTO.Brand;
                computer.Type = updateNewComputerDTO.Type;
                computer.Display = updateNewComputerDTO.Display;
                computer.memory = updateNewComputerDTO.memory;
                computer.Osys = updateNewComputerDTO.Osys;
                context.Computers.Update(computer);
                context.SaveChanges();
                return StatusCode(200, new { message = "sikeres frissítés", Result = computer });
            }
            return StatusCode(404, new { message = "sikertelen frissítés", Result = computer });
        }
        [HttpDelete]
        public object DeleteComputer([FromQuery] Guid id)
        {
            var computer = context.Computers.Find(id);
            if (computer != null)
            {
                context.Computers.Remove(computer);
                context.SaveChanges();
                return StatusCode(204, new { message = "sikeres törlés", Result = computer });
            }
            return StatusCode(404, new { message = "sikertelen törlés", Result = computer });

        }
    }
}

    

