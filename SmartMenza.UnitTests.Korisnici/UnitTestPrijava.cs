using Xunit;
using System.Threading.Tasks;
using SmartMenza.API.Data;
using SmartMenza.API.Controllers;
using Microsoft.EntityFrameworkCore;
using SmartMenza.API.Models;
using Microsoft.AspNetCore.Mvc;


namespace SmartMenza.UnitTests.Korisnici
{
    public class UnitTestPrijava
    {
        // Postavljanje okoline za testiranje
        private readonly AppDBContext _context;
        private readonly KorisniciController _korisniciController;

        public UnitTestPrijava()
        {
            // Postavljanje privremene baze
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "SmartMenzaTestDB")
                .Options;

            // Deklaracija baze
            _context = new AppDBContext(options);

            // Dodavanje testnih korisnika u bazu
            _context.Korisnici.Add(
                new Korisnik
                {
                    Ime = "Test",
                    Prezime = "Testic",
                    Email = "test@gmail.com",
                    LozinkaHash = "test1234",
                    UlogaId = 2
                });
            _context.Korisnici.Add(
                new Korisnik
                {
                    Ime = "Radnik",
                    Prezime = "Radnikic",
                    Email = "radnik@gmail.com",
                    LozinkaHash = "radnikpass",
                    UlogaId = 1
                });

            // Spremanje korisnika u bazu
            _context.SaveChanges();

            // Instanciranje kontrolera sa privremenom bazom kako bi se mogle testirati funkcije
            _korisniciController = new KorisniciController(_context);
        }

        // Testira funkciju LoginKorisnik sa prazim poljima u zahtjevu
        [Fact]
        public async Task LoginKorisnik_EmptyInput_ReturnsUnauthorized()
        {
            // Arrange
            var request = new Prijava
            {
                Email = "",
                LozinkaHash = ""
            };

            // Act
            var result = await _korisniciController.LoginKorisnik(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        // Testira funkciju LoginKorisnik sa praznim email poljem u zahtjevu
        [Fact]
        public async Task LoginKorisnik_EmptyEmail_ReturnsUnauthorized()
        {
            // Arrange
            var request = new Prijava
            {
                Email = "",
                LozinkaHash = "test1234"
            };

            // Act
            var result = await _korisniciController.LoginKorisnik(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        // Testira funkciju LoginKorisnik sa praznim lozinka poljem u zahtjevu
        [Fact]
        public async Task LoginKorisnik_EmptyPassword_ReturnsUnauthorized()
        {
            // Arrange
            var request = new Prijava
            {
                Email = "test@gmail.com",
                LozinkaHash = ""
            };

            // Act
            var result = await _korisniciController.LoginKorisnik(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        // Testira funkciju LoginKorisnik sa pogrešnom lozinkom u zahtjevu
        [Fact]
        public async Task LoginKorisnik_WrongPassword_ReturnUnauthorized()
        {
            // Arrange
            var request = new Prijava
            {
                Email = "test@gmail.com",
                LozinkaHash = "test12345"
            };

            // Act
            var result = await _korisniciController.LoginKorisnik(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        // Testira funkciju LoginKorisnik sa podacima postojeceg korisnika u zahtjevu
        [Fact]
        public async Task LoginKorisnik_CorrectInput_ReturnsOk()
        {
            // Arrange
            var request = new Prijava
            {
                Email = "test@gmail.com",
                LozinkaHash = "test1234"
            };

            // Act
            var result = await _korisniciController.LoginKorisnik(request);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        // Testira funkciju LoginKorisnik sa ispravnim emailom ali lozinkom drugog korisnika u zahtjevu
        [Fact]
        public async Task LoginKorisnik_CorrectEmailOtherUsersPassword_ReturnsUnauthorized()
        {
            // Arrange
            var request = new Prijava
            {
                Email = "test@gmail.com",
                LozinkaHash = "radnikpass"
            };

            // Act
            var result = await _korisniciController.LoginKorisnik(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }
    }
}
