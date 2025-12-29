# 📚 Bokeditor Millennium 9000

Ett komplett bokhandelshanteringssystem byggt i C# WPF med modern UI/UX design och full databasintegration.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat-square&logo=csharp)
![WPF](https://img.shields.io/badge/WPF-Framework-0078D4?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL_Server-Database-CC2927?style=flat-square&logo=microsoftsqlserver)

## 🎯 Projektöversikt

Bokeditor Millennium 9000 är ett omfattande inventerings- och orderhanteringssystem för bokhandelskedjor. Projektet demonstrerar avancerade WPF-tekniker, Entity Framework Core integration, och professionell MVVM-arkitektur.

## ✨ Huvudfunktioner

### 📦 Lagerhantering
- **Drag & Drop** funktionalitet för att flytta böcker mellan butiker
- Realtidssökning med dynamisk filtrering
- Visuell feedback med animationer och färgkodning
- Direktredigering av lagersaldon
- Automatisk validering och felhantering

### 📊 Orderhistorik
- Detaljerad orderöversikt per butik
- Statusspårning (Pågående, Skickad, Levererad)
- Färgkodade leveransstatus
- Beräkning av totala belopp och bokantal
- Sök och filtrera historiska ordrar

### 📝 Logghistorik
- Komplett spårning av alla lagerändringar
- Tidsstämplar för varje transaktion
- Visuell representation av händelser
- Filtreringsmöjligheter per butik

### ✏️ Bokhantering
- Lägg till nya böcker med ISBN-validering
- Automatisk författarkoppling
- Stöd för flera författare per bok
- Direktredigering av bokdetaljer

## 🏗️ Teknisk Stack

### Frontend
- **WPF (Windows Presentation Foundation)** - UI-ramverk
- **XAML** - Deklarativ UI-design
- **GongSolutions.WPF.DragDrop** - Drag & Drop-funktionalitet
- **Modern Design System** - Konsekvent färgpalett och typografi

### Backend
- **C# 12.0** - Programmeringsspråk
- **Entity Framework Core 8.0** - ORM
- **SQL Server** - Databas
- **MVVM Pattern** - Arkitekturmönster

### Databasdesign
- **8+ entitetstabeller** med normalisering
- **Views** för komplex datarepresentation
- **Stored Procedures** för affärslogik
- **Foreign Key-relationer** med referensintegritet

## 📂 Projektstruktur
```
Bokhandel_Labb/
├── Commands/           # RelayCommand och ICommand-implementationer
├── DTO/               # Data Transfer Objects för view-bindningar
├── Models/            # Entity Framework-modeller (genererade från DB)
├── ViewModels/        # MVVM ViewModels med affärslogik
├── Views/             # XAML-vyer för UI
├── App.xaml          # Applikationskonfiguration
└── MainWindow.xaml    # Huvudmeny
```

## 🚀 Installation och Setup

### Förutsättningar
- Visual Studio 2022 eller senare
- .NET 8.0 SDK
- SQL Server (LocalDB eller Full)

### Steg-för-steg

1. **Klona repositoryt**
```bash
   git clone https://github.com/nabblir/Bokhandel_Labb.git
   cd Bokhandel_Labb
```

2. **Uppdatera connection string**
   
   Öppna `Models/BokhandelContext.cs` och uppdatera connection string till din SQL Server-instans:
```csharp
   optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Bokhandel;...");
```

3. **Återställ NuGet-paket**
```bash
   dotnet restore
```

4. **Kör databasemigrationer** (om applicable)
```bash
   dotnet ef database update
```

5. **Bygg och kör**
```bash
   dotnet build
   dotnet run
```

## 💾 Databas

### Entiteter
- **Butiker** - Bokhandelskedjans filialer
- **Böcker** - Bokregistret med ISBN
- **Författare** - Författarinformation
- **LagerSaldo** - Lagerbalans per butik och bok
- **Ordrar** - Kundordrar
- **OrderRader** - Orderdetaljer
- **Kunder** - Kundinformation
- **LagerLogg** - Audit trail för lagerändringar

### Relationer
- Many-to-Many: Böcker ↔ Författare
- One-to-Many: Butiker → LagerSaldo
- One-to-Many: Ordrar → OrderRader
- Foreign Keys med CASCADE på vissa relationer

## 🎨 Design & UX

### Färgschema
- **Primär**: `#FF1A3333` (Mörk teal)
- **Sekundär**: `#FF3A5A5A` (Mellangrön)
- **Accent**: `#2196F3` (Blå)
- **Success**: `#81C784` (Grön)
- **Danger**: `#E57373` (Röd)

### Typografi
- **Font**: Bahnschrift SemiLight
- **Headers**: 26-32px Bold
- **Body**: 12-13px Regular

### Interaktiva element
- DropShadow-effekter för djup
- Scale-transformationer vid klick
- Hover-states med färgförändringar
- Smooth scrolling

## 🔑 Kortkommandon

| Tangent | Funktion |
|---------|----------|
| `Alt + S` | Spara ändringar |
| `Alt + Z` | Återställ ändringar |
| `Alt + E` | Ändra lagersaldo |
| `Alt + Q` | Avsluta/Stäng fönster |
| `F1` | Hjälp |

## 📸 Screenshots

### Huvudmeny
Modern 2x2 grid-layout med stora, lätta att klicka på knappar.

### Lagerhantering
Dual-pane vy med drag & drop mellan butiker, realtidssökning och papperskorgsyta.

### Orderhistorik
Färgkodad statusvisning med detaljerad orderinformation.

## 🧪 Testning

Projektet har testats med:
- Flera samtidiga butiker
- Stora bokdatabaser (100+ böcker)
- Komplexe drag & drop-scenarier
- Datavalidering och felhantering

## 📋 Krav (VG-nivå)

- [x] 8+ tabeller i databasen
- [x] Views för komplex datarepresentation
- [x] Stored Procedures
- [x] Normaliserad databasdesign
- [x] MVVM-arkitektur
- [x] Drag & Drop-funktionalitet
- [x] Realtidssökning
- [x] Fullständig CRUD-funktionalitet
- [x] Professionell UI/UX
- [x] Loggning av alla ändringar

## 🤝 Bidrag

Detta är ett skolprojekt, men feedback och förslag är välkomna via Issues.

## 📝 Licens

Detta projekt är skapat för utbildningsändamål.

## 👨‍💻 Författare

**Kevin** - [nabblir](https://github.com/nabblir)

---

*Utvecklat som del av databaskursen - Labb 2*
