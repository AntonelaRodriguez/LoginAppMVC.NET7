using Microsoft.EntityFrameworkCore;
using LoginApp.Models;

//Este código define una clase llamada AppDbContext que hereda de DbContext, la clase base proporcionada por Entity Framework Core para interactuar con una base de datos.
namespace LoginApp.Data
{
    // Definen un nuevo espacio de nombres LoginApp.Data donde se encuentra la clase AppDbContext.
    public class AppDbContext : DbContext
    {
        //  Esta opción se usa para configurar el comportamiento del contexto de la base de datos, como la cadena de conexión y otros aspectos de configuración.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        // Esta propiedad se utilizará para interactuar con la tabla de usuarios en la base de datos.
        // DbSet representa una colección de entidades en la base de datos.
        public DbSet<User> Users { get; set; }

        // Este método OnModelCreating es llamado cuando el modelo de base de datos está siendo creado por Entity Framework Core.
        // Aquí se especifican las configuraciones del modelo, como las claves primarias, restricciones de longitud y el nombre de la tabla.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(tb =>
            {
                tb.HasKey(col => col.Id); // HasKey especifica que la propiedad Id de la clase User es la clave primaria.
                tb.Property(col => col.Id)
                .UseIdentityColumn() //valor aumenta de uno en uno automaticamente
                .ValueGeneratedOnAdd(); // se crea cada vez que se inserten registros en esa tabla
            
                tb.Property(col => col.FullName).HasMaxLength(50); //PAra que tenga un maximo de 50
                tb.Property(col => col.Email).HasMaxLength(50);
                tb.Property(col => col.Password).HasMaxLength(50);
            });

            modelBuilder.Entity<User>().ToTable("User"); // para que la tabla no se cree en plural (Users) sino en singular (User)
        }
    }
}
