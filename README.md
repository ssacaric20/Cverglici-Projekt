# Cverglici-Projekt

Predložak za SmartMenzu<br>
Razvoj mobilne aplikacije u sklopu JCC-a koja studentima pomaže u personalizaciji obroka, praćenju nutritivnih vrijednosti, i postavljanju ciljeva vezanih uz prehranu.

# UPUTE ZA POKRETANJE
Za Frontend dio:
1. Otvoriti Android Studio
2. Preko Azure Repos, potrebno je kopirati URL za kloniranje repozitorija.
3. Unutar Android Studia, odabrati opciju za kloniranje repozitorija.
4. Kada se prikaže mogućnost za to, unjeti prethodno kopirati URL za kloniranje i odabrati prazan folder.
5. Dozvoliti da se aplikacija izgradi i potpuno učita.
6. Prebaciti na granu "Frontend" preko "Checkout" opcije.
7. Pričekati ako je potrebno, prihvatiti opcije ukoliko dođu upozorenja o potrebnom ažuriranju (Reload).
8. Ako još nije vidljiva mogućnost za debuggiranjem ili pokretanjem aplikacije, zatvoriti i ponovno pokrenuti Android Studio.
9. U suprotnome, pokrenuti aplikaciju.

Ako imate imate **Gradle problema**...
Nekada zbog cashe memorije izbacuje grešku za Gradle Sync projekta. Ukoliko Vam čak i nakon čekanja automatski ne postavi projekt, pokušajte:
1. pod Build Tools omogućiti Auto-Sync ("Project Sync mode: Always Sync projects automatically")
2. pod "File>Invalidate cashes" pritisnuti "Invalidate and Restart" opciju.
3. pod "File>Sync Project with Gradle files"
Ako ni to ne radi, probajte:
4. u Gradle settings omogućiti "Enable Parallel Gradle model fetching" i "Download external annotations for dependencies". 

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
