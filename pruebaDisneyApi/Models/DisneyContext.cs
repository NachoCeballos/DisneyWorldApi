using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using pruebaDisneyApi.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace pruebaDisneyApi.Models
{
    public class DisneyContext : DbContext
    {
        public DisneyContext()
        {
        }

        public DisneyContext(DbContextOptions<DbContext> options) : base(options)
        {

        }

        public DbSet<Personaje> Personajes { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        
        public DbSet<PeliculaPersonaje> PeliculaPersonaje { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeliculaPersonaje>()
                .HasKey(t => new { t.PeliculaId, t.PersonajeId });

            modelBuilder.Entity<PeliculaPersonaje>()
                .HasOne(pt => pt.Pelicula)
                .WithMany(p => p.PeliculaPersonajes)
                .HasForeignKey(pt => pt.PeliculaId);

            modelBuilder.Entity<PeliculaPersonaje>()
                .HasOne(pt => pt.Personaje)
                .WithMany(t => t.PeliculaPersonajes)
                .HasForeignKey(pt => pt.PersonajeId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = @"YourConnectionString";
            optionsBuilder.UseSqlServer(connection);
        }
    }

    public class Personaje
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int Peso { get; set; }
        public string Historia { get; set; }
        public List<PeliculaPersonaje> PeliculaPersonajes { get; set; }
        public string UrlImg { get; set; }
    }

    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Clasificacion { get; set; }
        public List<PeliculaPersonaje> PeliculaPersonajes { get; set; }
        public int GeneroId { get; set; }
        public Genero Genero { get; set; }
        public string UrlImg { get; set; }
    }

    public class PeliculaPersonaje
    {
        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; }
        public int PersonajeId { get; set; }
        public Personaje Personaje { get; set; }
    }

    public class Genero
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public ICollection<Pelicula> Peliculas { get; set; }
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
    }
}
