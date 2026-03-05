# Feedback på Lufthavns Informationssystem (Emil)

Virkelig flot arbejde, Emil! Du har fået implementeret et komplet system med både Web API, RabbitMQ integration og en konsol-applikation. Din brug af et delt bibliotek (`AirportModelsLibrary`) til modellerne er en rigtig god arkitektonisk beslutning, da det sikrer konsistens på tværs af dine projekter.

---

## 🌟 Hvad der fungerer rigtig godt
- **Arkitektur:** Opdelingen i Web API, Models Library og Console App er helt efter bogen.
- **Fuld CRUD:** Du har implementeret både POST, PUT og DELETE, hvilket opfylder opgavebeskrivelsen fuldt ud.
- **RabbitMQ Integration:** Du har fået hul igennem til RabbitMQ og bruger både Exchanges (Fanout) og Queues korrekt.

---

## 🛠️ Forslag til forbedring og optimering

### 1. Asynkron programmering (`async void` vs `async Task`)
I din `Publisher.cs` brugte du `async void`. 
- **Hvorfor ændre det?** Man bør næsten altid bruge `async Task`. `async void` gør det umuligt for kalderen at afvente (await) opgaven, og hvis der sker en fejl, kan den crashe hele applikationen, fordi fejlen ikke kan fanges korrekt. Jeg har rettet dette for dig, så dit API nu afventer, at beskeden faktisk er sendt.

### 2. Lagring af data (`Session` vs `Static/DB`)
Du bruger `HttpContext.Session` til at gemme dine flyafgange.
- **Observation:** Session-data er unik for den enkelte browser-session. Det betyder, at hvis jeg opretter et fly, kan du ikke se det i din browser (eller i din konsol-app, hvis den lytter på en anden session).
- **Forslag:** I et system som dette, hvor alle skal se de samme data, ville en `static List<FlightDeparture>` eller en rigtig database være mere passende.

### 3. RabbitMQ Forbindelser
Lige nu åbner og lukker du en ny forbindelse (`factory.CreateConnectionAsync()`) hver eneste gang, du sender en besked.
- **Optimering:** Det er meget "dyrt" for systemet at oprette forbindelser. I en produktions-app ville man typisk oprette forbindelsen én gang (f.eks. som en Singleton via Dependency Injection) og holde den åben så længe applikationen kører.

### 4. `Console.ReadLine()` i Web API
Jeg lagde mærke til en `Console.ReadLine()` i din `Publisher`. 
- **Tip:** I et Web API skal man undgå at blokere tråden med brugerinput, da der ikke er nogen konsol at trykke "Enter" i, når appen kører på en server. Det kan få dit API til at "hænge".

---

## 📋 Næste skridt
Prøv at kigge på **Dependency Injection**. I stedet for at skrive `private Publisher publisher = new Publisher();`, så prøv at registrere din `Publisher` i `Program.cs` og få den leveret via controllerens constructor. Det gør din kode meget lettere at teste!

Godt gået! Du er nået rigtig langt med de svære koncepter. ✈️
