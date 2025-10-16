# Cverglici-Projekt

Predložak za AICareerBuddy
Razvoj mobilne aplikacije u sklopu JCC-a koja studentima pomaže u razvoju karijere koristeći Azure servise.

## Funkcionalnosti
- Registracija korisnika
- Prijava korisnika
- Dodjela uloge po korisničkom računu (razina studenta ili mentora/fakulteta)
- Upload i analiza životopisa
- Pregled i prijava na job oglase
- Simulacija AI intervjua

## Tehnologije
- ASP.NET Core Web API
- Entity Framework (Code-First)
- Azure DevOps, Azure Pipelines
- Azure AI Services, Blob Storage
- SQL Server

## Deployment
Aplikacija se gradi putem Azure Pipelinea

## Struktura repozitorija
Prijedlog strukture:
API	- Za rukovanje HTTP zahtjevima, endpointima, validacijom inputa
App	- Poslovna logika i komunikacija između slojeva
Domena - Sadrži čiste entitete i pravila domene (klase, enums)
Infrastruktura -	Implementacija servisa, baze podataka, pristup AI API-jima
Shared -	Helperi, exception handleri, i zajedničke klase
Testovi	- Pokriva unit i integration testove
