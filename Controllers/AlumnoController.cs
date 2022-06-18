using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Alumno")]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;

        public AlumnoController(KalumDbContext _DbContext, ILogger<AlumnoController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlumnoController>>> Get()
        {
            List<Alumno> alumnos = null;
            Logger.LogDebug("Iniciando proceso de consulta de alumnos");
            alumnos = await DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentaPorCobrar).ToListAsync();
            if(alumnos == null || alumnos.Count==0)
            {
                Logger.LogWarning("No existen alumnos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(alumnos);
        }

        [HttpGet("{carne}", Name = "GetAlumno")]
        public async Task<ActionResult<Alumno>> GetAlumno(string carne)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + carne);
            var alumno = await DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentaPorCobrar).FirstOrDefaultAsync(a => a.Carne == carne);
            if(alumno == null)
            {
                Logger.LogWarning("No existe el alumno con el carne " + carne);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(alumno);
        }

        // [HttpPost]
        //public async Task<ActionResult<Alumno>> Post([FromBody] Alumno value)
       // {
       //     Logger.LogDebug("Iniciando el proceso de agregar un Alumno nuevo");
       //     value.Carne = Guid.NewGuid().ToString().ToUpper();
       //     await DbContext.Alumno.AddAsync(value);
       //     await DbContext.SaveChangesAsync();
       //     Logger.LogInformation("Finalizando el proceso de agregar un Alumno");
       //     return new CreatedAtRouteResult("GetAlumno", new { id = value.Carne }, value);
       // }
        
        
        

    }
}