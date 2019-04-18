# Kros.AspNetCore.BestPractices

Toto repo bude obsahovať demo príklad, ktorý bude ukazovať architektúru našich služieb.

Mal by obsahovať:
- vzorovú štruktúru projektov
- použitie našich knižníc
- použitie odporúčaných externých knižníc
- ukážku architektúry jednotlivých služieb

Na základne tohto dema by mal človek pochopiť architektúru našich služieb a mal by byť schopný vytvoriť nový projekt v podobnom duchu.

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