# Taller Mecánico — API REST en capas (Lab 13)

Web API en ASP.NET Core (.NET 10) con Entity Framework Core, DTOs y AutoMapper.
Base de datos: PostgreSQL (Npgsql). El script con datos de ejemplo está en [`database.sql`](database.sql).

## Tema
Un taller mecánico necesita llevar el control de los **vehículos** de sus clientes y de las **órdenes de trabajo** que se les realizan.
- **Vehiculo** (1) → **OrdenTrabajo** (N): una orden pertenece a un vehículo (`vehiculo_id`); un vehículo puede tener muchas órdenes.

### Reglas de negocio (capa Negocio, `ReglaNegocioException` → 409 Conflict)
1. No se puede registrar/actualizar un vehículo con una placa que ya existe.
2. No se puede eliminar un vehículo que tiene órdenes de trabajo asociadas.
3. Una orden solo puede crearse/actualizarse para un vehículo existente.
4. El estado de la orden debe ser `Pendiente`, `EnProceso`, `Completada` o `Cancelada`.

## Arquitectura
```
Api  →  Negocio  →  Datos  →  Entidades
```
| Proyecto | Contenido |
|---|---|
| `Entidades` | Entidades (`Vehiculo`, `OrdenTrabajo`) y DTOs |
| `Datos` | `TallerDbContext` y repositorios (con interfaz) |
| `Negocio` | Servicios (con interfaz), reglas de negocio, excepciones y `MappingProfile` |
| `Api` | Controladores, `Program.cs` (DI, Swagger, CORS) |

Los controladores solo usan servicios (por interfaz) y DTOs. Swagger muestra únicamente DTOs.

## Endpoints
| Método | Ruta | Respuestas |
|---|---|---|
| GET | `api/vehiculos?marca=&modelo=&anioMin=&anioMax=&placa=` (filtros) | 200 |
| GET | `api/vehiculos/{id}` | 200, 404 |
| GET | `api/vehiculos/{id}/ordenes` (ruta anidada) | 200, 404 |
| POST | `api/vehiculos` | 201, 400, 409 |
| PUT | `api/vehiculos/{id}` | 204, 400, 404, 409 |
| DELETE | `api/vehiculos/{id}` | 204, 404, 409 |
| GET | `api/ordenes?estado=&vehiculoId=` (filtros) | 200 |
| GET | `api/ordenes/{id}` | 200, 404 |
| POST | `api/ordenes` | 201, 400, 409 |
| PUT | `api/ordenes/{id}` | 204, 400, 404, 409 |
| DELETE | `api/ordenes/{id}` | 204, 404 |

Pruebas listas en [`Api/Taller.Api.http`](Api/Taller.Api.http).

## Sustentación de los mapeos (`MappingProfile`)
| Mapeo | Origen → Destino | ¿Por qué en ese sentido? | Configuración especial |
|---|---|---|---|
| Vehiculo → VehiculoDto | Entidad → DTO de lectura | Es lo que la API devuelve; la entidad no sale de la API | Ninguna: los nombres coinciden |
| OrdenTrabajo → OrdenTrabajoDto | Entidad → DTO de lectura | Es lo que la API devuelve al cliente | `ForMember`: `Vehiculo` toma la `Placa` del vehículo relacionado |
| CrearVehiculoDto → Vehiculo | DTO de entrada → Entidad | Llega del cliente y se convierte en entidad para guardarla | `ForMember`: `FechaRegistro` = hoy si no se envía |
| ActualizarVehiculoDto → Vehiculo | DTO de entrada → Entidad | Se aplica sobre la entidad existente (`Map(origen, destino)`) | `Id` ignorado; `FechaRegistro` solo si viene valor |
| CrearOrdenTrabajoDto → OrdenTrabajo | DTO de entrada → Entidad | Llega del cliente y se guarda | `ForMember`: `FechaIngreso` = hoy si no se envía |
| ActualizarOrdenTrabajoDto → OrdenTrabajo | DTO de entrada → Entidad | Se aplica sobre la entidad existente | `Id` ignorado; `FechaIngreso` solo si viene valor |

No se usa `ReverseMap()`: los DTOs de entrada y de lectura son distintos y el flujo es unidireccional.

## Ejecutar
1. Crear la base y ejecutar `database.sql` en PostgreSQL.
2. Configurar la cadena de conexión **sin subirla al repositorio**:
   `dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=...;Database=...;Username=...;Password=..." --project Api`
   (o variable de entorno `ConnectionStrings__DefaultConnection`).
3. `dotnet run --project Api` → Swagger en `/swagger`.
