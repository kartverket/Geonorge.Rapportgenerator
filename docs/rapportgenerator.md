# Rapportgenerator

## Beskrivelse av funksjonalitet:
Generering av rapporter for ulike datasett 

**Teknisk**

**URL**:

-  **Dev**: http://rapportgenerator.dev.geonorge.no/
-  **Test**: https://rapportgenerator.test.geonorge.no/
-  **Prod**: https://rapportgenerator.geonorge.no/

**Kildekode**: https://github.com/kartverket/Geonorge.Rapportgenerator 


**Oversikt kildekode**:

Applikasjonen er utviklet med C# og .NET framework.

- **/Kartverket.ReportApi** .net klassebibliotek for generell datastruktur for rapporter
- **/Kartverket.ReportGenerator** Web applikasjon: ASP.NET MVC 5


  **Detaljer**:

For Det offentlige kartgrunnlaget er det laget 3 rapporter i registeret, se https://register.geonorge.no/Help/Api/POST-api-Report.

De andre rapportene er basert på wfs stored queries.  For å administrere hvilke datasett som skal benyttes vedlikeholder man tabellen MetadataEntries og feltet Uuid i databasen kartverket_reportgenerator.
Rapportgeneratoren vil da hente info fra kartkatalogen sitt api for å finne tittel på datasett og url til wfs tjenesten.

I definisjonen av stored queries er det valgt følgende navnekonvensjon for StoredQuery sin id: <objekttype>_PrAdmEnhet_<Totalt|objektegenskap>
Det er kun område, parameter admEnhNr som er inputt.
Eks. storedquery sin id/name:
TettstedHCparkering_PrAdmEnhet_LengdeMindreEnn6
TettstedHCparkering_PrAdmEnhet_Totalt 

StoredQuery må returnere følgende projeksjon for at punktet skal vises rett i kartet: srsName="urn:ogc:def:crs:EPSG::25833".
