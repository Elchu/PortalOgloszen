using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Repozytory.Models;

namespace Repozytory.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Repozytory.Models.OgloszeniaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Repozytory.Models.OgloszeniaContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // Do debugowania metody seed
            // if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
            SeedRoles(context);
            SeedUsers(context);
            SeedOgloszenia(context);
            SeedKategorie(context);
            SeedOgloszenieKategoria(context);


        }

        private void SeedRoles(OgloszeniaContext context)
        {
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>());

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
        }

        private void SeedUsers(OgloszeniaContext context)
        {
            var store = new UserStore<Uzytkownik>(context);
            var manager = new UserManager<Uzytkownik>(store);
            if (!context.Users.Any(u => u.UserName == "Admin"))
            {

                var user = new Uzytkownik { UserName = "Admin", Email = "admin@admin.pl" };

                var adminresult = manager.Create(user, "12345678");

                if (adminresult.Succeeded)
                    manager.AddToRole(user.Id, "Admin");
            }
        }

        private void SeedOgloszenia(OgloszeniaContext context)
        {

            var idUzytkownika = context.Set<Uzytkownik>().Where(u => u.UserName == "Admin").FirstOrDefault().Id;
            for (int i = 1; i <= 10; i++)
            {
                var ogl = new Ogloszenie()
                {
                    UzytkownikId = idUzytkownika,
                    Tresc = "Treœæ og³oszenia" + i.ToString(),
                    Tytul = "Tytu³ og³oszenia" + i.ToString(),
                    DataDodania = DateTime.Now.AddDays(-i)
                };
                context.Set<Ogloszenie>().AddOrUpdate(ogl);
            }
            context.SaveChanges();
        }

        private void SeedKategorie(OgloszeniaContext context)
        {
            for (int i = 1; i <= 10; i++)
            {
                var kat = new Kategoria()
                {
                    Nazwa = "Nazwa kategorii" + i.ToString(),
                    Tresc = "Treœæ og³oszenia" + i.ToString(),
                    MetaTytul = "Tytu³ kategorii" + i.ToString(),
                    MetaOpis = "Opis kategorii" + i.ToString(),
                    MetaSlowa = "S³owa kluczowe do kategorii" + i.ToString(),
                    ParentId = i
                };
                context.Set<Kategoria>().AddOrUpdate(kat);
            }
            context.SaveChanges();
        }

        private void SeedOgloszenieKategoria(OgloszeniaContext context)
        {
            for (int i = 1; i < 10; i++)
            {
                int losoweIdOgloszenia = context.Ogloszenia.OrderBy(d => Guid.NewGuid()).First().OgloszenieId;
                int losoweIdkategorii = context.Kategorie.OrderBy(d => Guid.NewGuid()).First().KategoriaId;
                var okat = new OgloszenieKategoria()
                {
                    OgloszenieId = losoweIdOgloszenia,
                    KategoriaId = losoweIdkategorii
                };

                context.Set<OgloszenieKategoria>().AddOrUpdate(okat);
            }
            context.SaveChanges();
        }

    }
}
