# Cverglici-Projekt

Predložak za SmartMenzu<br>
Razvoj mobilne aplikacije u sklopu JCC-a koja studentima pomaže u personalizaciji obroka, praćenju nutritivnih vrijednosti, i postavljanju ciljeva vezanih uz prehranu.
U donjim sekcijama ovog dokumenta, može se vidjeti zamišljeni popis glavnih funkcionalnosti prema ulogama, kao i upute pokretanja određenog dijela projekta. 

Upute za pokretanje Frontend dijela projekta:
1. Otvoriti Android Studio
2. Preko Azure Repos, potrebno je kopirati URL za kloniranje repozitorija.
3. Unutar Android Studia, odabrati opciju za kloniranje repozitorija.
4. Kada se prikaže mogućnost za to, unjeti prethodno kopirati URL za kloniranje i odabrati prazan folder.
5. Dozvoliti da se aplikacija izgradi i potpuno učita.
6. Prebaciti na granu "Frontend" preko "Checkout" opcije.
7. Pričekati ako je potrebno, prihvatiti opcije ukoliko dođu upozorenja o potrebnom ažuriranju.
8. Ako još nije vidljiva mogućnost za debuggiranjem ili pokretanjem aplikacije, zatvoriti i ponovno pokrenuti Android Studio.
9. U suprotnome, pokrenuti aplikaciju.

Ako se sve izvede kao zamišljeno, trebali biste vidjeti početni ekran za prijavu korisnika. Ako se aplikacija samostalno ne otvori, potrebno je među listom aplikacija pronaći aplikaciju pod nazivom "SmartMenza".
Bit će vidljiva dva fragmenta: login i register fragmenti koji se izmijenjuju prilikom klika na svaki pojedinačni tab button ("Prijava"/"Registracija").
Možete registrirati vlastiti račun, a možete i koristiti već gotove podatke za prijavu (email/pass): student@test.com, pass123. 

Prije svega, potrebno je pokrenuti backend dio projekta. To činite tako što otvorite .sln projekt u Visual Studiu. U Developer Powershellu unutar VS-a, prebacite se u folder gdje se nalazi Project.cs preko komande "cd SmartMenza", i zatim pokrenete komandu "dotnet run". To je ukoliko odličite pokretati projekt lokalno.

Za pokretanje Backend dijela projekta:
1. Otvoriti Visual Studio
2. Preko Azure Repos, potrebno je kopirati URL za kloniranje repozitorija.
3. Unutar Visual Studia, odabrati opciju za kloniranje repozitorija.
4. Kada se prikaže mogućnost za to, unjeti prethodno kopirati URL za kloniranje i odabrati prazan folder.
5. Kada se repozitorij klonira, pritisnuti na "Git Changes" i prebaciti se na "Backend" granu.
6. Unutar Visual Studija pritisnuti desni klik na solution i rebuildati ga.
7. Unutar Visual Studija pritisnuti desni klik na solution i pritisnuti na Restore NuGet Packages.
8. Pričekati ako je potrebno, prihvatiti opcije ukoliko dođu upozorenja o potrebnom ažuriranju.
9. Pokrenuti aplikaciju.

Kako bi Backend dio aplikacije radio, potrebno je imati instaliran sql express server i uspostavljenu bazu podataka.
Pokretanjem aplikacije pokrenut će se browser u koji omogućuje testiranje API endpointa uz pomoć swaggera.

## Funkcionalnosti koje će do finalne predaje biti implementirane:
* [x] Registracija korisnika 
* [x] Prijava korisnika 
* [x] Dodjela uloge po korisničkom računu (razina studenta ili zaposlenika)

### Student:
* [x] Pregled dnevnog menija 
* [x] Pregled detalja o jelu
- Dodavanje ciljeva
- Ažuriranje ciljeva
- Brisanje ciljeva
- AI preporuka obroka
- Označavanje najdražih jela
- Uklanjanje određenih jela iz lise najdražih
- Ocjenjivanje i komentiranje jela

### Zaposlenik:
- Unos menija
- Uređivanje menija
- Brisanje menija
- Nutritivna analiza menija putem AI
- Pregled osnovne statistike
- Generiranje slike jela putem AI

* [x] - je oznaka koja određuje da je funkcionalnost obavljena

## Tehnologije
- ASP.NET Core Web API
- Entity Framework (Code-First)
- Azure DevOps
- SQL Server, SSMS, Swagger


