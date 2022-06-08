namespace WebApiKalum.Entities
{
    public class Alumno
    {
        public string Carne { get; set;}

        public string Apellidos { get; set;}

        public string Nombres { get; set;}

        public string Direccion { get; set;}

        public string telefono { get; set;}

        public string Email { get; set;}

        public virtual List<Inscripcion> Inscripciones {get; set;}

        
    }
}