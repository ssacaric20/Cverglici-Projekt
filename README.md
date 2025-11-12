# Cverglici-Projekt

Predložak za SmartMenzu<br>
Razvoj mobilne aplikacije u sklopu JCC-a koja studentima pomaže u personalizaciji obroka, praćenju nutritivnih vrijednosti, i postavljanju ciljeva vezanih uz prehranu.

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
