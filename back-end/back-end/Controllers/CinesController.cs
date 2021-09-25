using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/cines")]
    [ApiController]
    public class CinesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CinesController(ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CineDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Cines.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var cines = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<CineDTO>>(cines);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CineDTO>> Get(int id)
        {
            var cine = await context.Cines.FirstOrDefaultAsync(x => x.Id == id);

            if(cine == null)
            {
                return NotFound();
            }

            return mapper.Map<CineDTO>(cine);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CineCreacionDTO cineCreacionDTO)
        {
            var cine = this.mapper.Map<Cine>(cineCreacionDTO);
            this.context.Cines.Add(cine);
            await this.context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CineCreacionDTO cineCreacionDTO)
        {
            var cine = await context.Cines.FirstOrDefaultAsync(x => x.Id == id);

            if(cine == null)
            {
                return NotFound();
            }

            cine = mapper.Map(cineCreacionDTO, cine);
            //context.Entry(cine).State = EntityState.Modified;
            await context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Cines.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Cines.Remove(new Cine() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
     


    }
}
