
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
    {

    }


    public DbSet<Funcionario> Funcionario { get; set; }

    public DbSet<Folha> Folha { get; set; }


}