﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Repozytory.IRepo;

namespace Repozytory.Models
{


    public class OgloszeniaContext : IdentityDbContext, IPortalOgloszenContext
    {
        public OgloszeniaContext()
            : base("DefaultConnection")
        {
        }

        public static OgloszeniaContext Create()
        {
            return new OgloszeniaContext();
        }

        public DbSet<Kategoria> Kategorie { get; set; }
        public DbSet<Ogloszenie> Ogloszenia { get; set; }
        public DbSet<Uzytkownik> Uzytkownik { get; set; }
        public DbSet<OgloszenieKategoria> OgloszenieKategoria { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Potrzebne dla klas Identity
            base.OnModelCreating(modelBuilder);

            // using System.Data.Entity.ModelConfiguration.Conventions;
            //Wyłącza konwencję, która automatycznie tworzy liczbę mnogą dla nazw tabel w bazie danych
            // Zamiast Kategorie zostałaby stworzona tabela o nazwie Kategories
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Wyłącza konwencję CascadeDelete
            // CascadeDelete zostanie włączone za pomocą Fluent API
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Używa się Fluent API, aby ustalić powiązanie pomiędzy tabelami 
            // i włączyć CascadeDelete dla tego powiązania
            modelBuilder.Entity<Ogloszenie>().HasRequired(x => x.Uzytkownik)
                .WithMany(x => x.Ogloszenia)
                .HasForeignKey(x => x.UzytkownikId)
                .WillCascadeOnDelete(true);
        }
    }


}