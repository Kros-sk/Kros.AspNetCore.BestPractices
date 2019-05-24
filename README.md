# Kros.AspNetCore.BestPractices

Toto repo **bude** obsahovať demo príklad, ktorý bude ukazovať architektúru našich služieb.

Mal by obsahovať:
- vzorovú štruktúru projektov
- použitie našich knižníc
- použitie odporúčaných externých knižníc
- ukážku architektúry jednotlivých služieb

Na základne tohto dema by mal človek pochopiť architektúru našich služieb a mal by byť schopný vytvoriť nový projekt v podobnom duchu.

- [Kros.AspNetCore.BestPractices](#krosaspnetcorebestpractices)
  - [Quick start](#quick-start)
  - [Overview](#overview)
  - [Project physical structure](#project-physical-structure)
  - [Microservices internal architecture](#microservices-internal-architecture)
    - [Prehľad CQRS](#preh%C4%BEad-cqrs)
    - [CQRS](#cqrs)
  - [Použité knižnice](#pou%C5%BEit%C3%A9-kni%C5%BEnice)

## Quick start

## Overview

## Project physical structure

## Microservices internal architecture

Táto sekcia sa venuje tomu ako sú vnútorne organizované služby. Popisuje ich vnútornú architekúru.

Existuje niekoľko spôsobov ako sa k tomu postaviť. Pokiaľ je služba naozaj jednoduchá, zameraná na výkon a nepredpokladáme jej postupné zozložiťovanie, tak môžme využiť `CRUD` príst a úpne bezstarosti priamo v controlleroch *(alebo vo vlastnom routingu, ak je to naozaj kritická služba z pohľadu výkonu)* napísať požadovanú funkčnosť. Samozrejme na t musí by jasný dôvod.

My sme sa rozhodli pre netriviálne služby využiť architektonický návrhový vzor `CQRS`.

### Prehľad CQRS

Predstavme si triedu `ToDosService`. Štandardná trieda v prípade, že nechceme písať kód priamo v controlleroch. Takéto triedy obsahujú metódy typu `GetAllToDos()`, `GetToDoById(id)`, `CreateToDo(toDoViewModel)`, ... . Obsahovala implementáciu všetkej funkčnosti, ktorú očakávame od `ToDos`. Takéto triedy majú tendenciu narastať. Každá zmena si vyžaduje zložité skúmanie ako dané veci fungujú a aký by mohl byť vedľajší účinok týchto zmien. Nehovoriac o tom, že takúto triedu ťažko použijeme na inom mieste ak z nej potrebujeme nejakú funkčnosť.

Vo väčšine prípadov môžme v jednoduchosti hovoriť o dvoch typoch operácií. Operácie, ktoré čítajú dáta a ktoré ich menia. *(čítajú stav a menia stav systému)* Vo väčšine aplikácií (systémov) sú jasné rozdiely medzi operáciami, ktoré čítajú stav (**queries**) a ktoré menia stav (**commands**). Keď čítame dáta, tak zvyčajne nepotrebujeme validovať tieto dáta, alebo nejakú ďalšiu business logiku. Ale často práve potrebujeme kešovať. Model pre čítanie a zápis môže byť (často býva) iný.

### CQRS


Poznámky
- v query nemusý byť

## Použité knižnice

- [Kros.KORM](https://github.com/Kros-sk/Kros.KORM) je ORM knižnica pre prístup k MS SQL databázam.
- [Kros.KORM.Extensions.Asp](https://github.com/Kros-sk/Kros.KORM.Extensions.Asp) balíček obshuje rozšírenia KORM-u pre jednoduchšiu integráciu s ASP.NET Core službami. (Registrovanie do DI kontajnera, migrácie, ...)
- [Kros.Utils](https://github.com/Kros-sk/kros.utils) všeobecná knižnica obsahujúca pomôcky pre bežné programovanie v .NET.
- [Scrutor](https://github.com/khellang/Scrutor) umožňuje skenovať `Assembly` a automaticky registrovať služby do DI kontajnera.
- [Mapster](https://github.com/MapsterMapper/Mapster) pre automatické mapovanie entít, DTO, doménových tried, ...
- [MediatR](https://github.com/jbogard/MediatR) knižnica pre in-process komunikáciu. Pomocou tejto knižnice implemnetujeme CQRS pattern vrámci jednej služby.
- [FluentValidation](https://fluentvalidation.net/) používame na validovanie. Každý request v rámci `MediatR` je automaticky validovaný. Validačné pravidlá sa nachádzajú v triedach s postfixom `Validator`.
- [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) knižnica, ktorá automaticky vygeneruje API dokumentáciu pre jednotlivé endpointy.
- [MicroElements.Swashbuckle.FluentValidation](https://github.com/micro-elements/MicroElements.Swashbuckle.FluentValidation) rozšírenie, ktoré validačné pravidla písané cez `FluentValidation` prenesie do swagger dokumentácie.
- [Microsoft.Extensions.Caching.StackExchangeRedis](https://www.nuget.org/packages/Microsoft.Extensions.Caching.StackExchangeRedis) - používame na komunikáciu s `Redis` distribuovanou kešou.
- [Ocelot](https://github.com/ThreeMammals/Ocelot) framework na vytvorenie vlastnej `Api Gateway`. Umožňuje jednoducho vytvoriť proxy, ktorá zastreší presmerovanie na vnútorné služby. Umožňuje ale aj ďalšie veci ako aggregáciu, rate limity, ...
- [Flurl](https://github.com/tmenier/Flurl) fluent URL builder and testable HTTP client for .NET https://flurl.io
