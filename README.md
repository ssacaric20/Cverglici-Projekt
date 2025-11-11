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
Demo mock podaci za prijavu su (ime/email/lozinka): Ana, ana.kovac@student.hr, student123.

## Funkcionalnosti
- Registracija korisnika
- Prijava korisnika
- Dodjela uloge po korisničkom računu (razina studenta ili zaposlenika)

### Student:
- Pregled dnevnog menija
- Pregled detalja o jelu 
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

## Tehnologije
- ASP.NET Core Web API
- Entity Framework
- Azure DevOps, Azure Pipelines
- Azure AI Services, Blob Storage
- SQL Server

## Deployment
Aplikacija se gradi putem Azure Pipelinea

## Struktura repozitorija
Prijedlog strukture:<br>
API	- Za rukovanje HTTP zahtjevima, endpointima, validacijom inputa<br>
App	- Poslovna logika i komunikacija između slojeva<br>
Domena - Sadrži čiste entitete i pravila domene (klase, enums)<br>
Infrastruktura -	Implementacija servisa, baze podataka, pristup AI API-jima<br>
Shared -	Helperi, exception handleri, i zajedničke klase<br>
Testovi	- Pokriva unit i integration testove
