using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;


namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Jornada")]
    public class JornadaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<JornadaController> Logger;

        public JornadaController(KalumDbContext dbContext, ILogger<JornadaController> _Logger)
        {
            this.DbContext = dbContext;
            this.Logger = _Logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jornada>>> Get()
        {
            List<Jornada> jornadas = null;
            Logger.LogDebug("Iniciando proceso consulta jornadas");
            jornadas = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).ToListAsync();
            
            if(jornadas == null || jornadas.Count==0)
            {
                Logger.LogWarning("No existen jornadas");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(jornadas);
        }

        [HttpGet("{id}", Name = "GetJornada")]
        public async Task<ActionResult<Jornada>> GetJornada(string id)
        {
            Logger.LogDebug("Iniciando proceso de busqueda con el id " + id);
            var jornada = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).FirstOrDefaultAsync(j => j.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning("No existe la jornada con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(jornada);
        }

         [HttpPost]
        public async Task<ActionResult<Jornada>> Post([FromBody] Jornada value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una Jornada");
            value.JornadaId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Jornada.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de agregar una Jornada");
            return new CreatedAtRouteResult("GetJornada", new { id = value.JornadaId }, value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Jornada>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de una Jornada");
            Jornada Jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if (Jornada == null)
            {
                Logger.LogWarning($"No se encontro ninguna Jornada con el id {id}");
                return NotFound();
            }
            else
            {
                DbContext.Jornada.Remove(Jornada);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha elimindo correctamente la carrera tecnica con el id {id} ");
                return Jornada;
            }
        }

         [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Jornada value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de una Jornada con el id {id}");
            Jornada Jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if(Jornada == null)
            {
                Logger.LogWarning($"No existe la Jornada con el ID {id}");
                return BadRequest();
            }
            Jornada.Descripcion = value.Descripcion;
            DbContext.Entry(Jornada).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }
        
        
    }
}