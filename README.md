# Sistema de Gestión de Órdenes de Servicio

Sistema full stack desarrollado como prueba técnica para la gestión de órdenes de servicio, técnicos y clientes.

---

## Tecnologías utilizadas
### Database
---
Se utiliza Dapper, consultas SQL manuales y JWT auth.

 - Ejecutarlo en PostgreSQL
      Opción 1: pgAdmin
            - Open Query Tool
            - Pegas el script
            - Ejecutas
     Opción 2: Terminal
            - psql -U postgres -d your_database -f database.sql
---
### Backend
- .NET 6+ Web API
- Dapper (acceso a datos)
- JWT Authentication
- Oracle / PostgreSQL
---
### Frontend
- Angular 14+
- Reactive Forms
- Standalone Components
- TailwindCSS (opcional)
---
### Control de versiones
- Git / GitHub

---

### Autenticación
- Login con usuario y contraseña
- Autenticación mediante JWT
- Protección de endpoints
- Usuario precargado en base de datos

---

### Módulo de Técnicos
- Crear técnicos
- Editar técnicos
- Eliminar técnicos
- Listar técnicos
- Campos:
  - Nombre completo
  - Documento
  - Teléfono
  - Especialidad

---

### Módulo de Clientes
- CRUD completo
- Validación de documento único
- Campos:
  - Nombre completo
  - Documento de identidad
  - Dirección
  - Teléfono

---

### Módulo de Órdenes de Servicio
- Crear, editar, eliminar órdenes
- Estados:
  - Pendiente
  - En progreso
  - Finalizada
- Asignación de:
  - Técnico
  - Cliente
- Fecha de creación automática

---

### Filtros de órdenes (Backend SQL con Dapper)
- Estado
- Técnico (nombre / especialidad)
- Cliente (nombre / documento)
- Rango de fechas
- Combinaciones dinámicas de filtros

---

## Arquitectura del proyecto
  - Frontend (Angular)
    ↓ HTTP (JWT)
    
  - Backend (.NET API)
    ↓ Dapper
    
  - Base de Datos (Oracle / PostgreSQL)

## Ejecutar Frontend (Angular)
 - cd Frontend/service-orders-ui
 - npm install
 - ng serve
 - http://localhost:4200/

## Ejecutar Backend (.NET API)

cd Backend
  - dotnet restore
  - dotnet run
    
  - https://localhost:5001
  - http://localhost:5000
---

## Endpoints principales
- Auth
    POST /api/auth/login
    GET /api/auth/me
  
- Técnicos
    GET /api/technicians
    POST /api/technicians
    PUT /api/technicians/{id}
    DELETE /api/technicians/{id}

- Clientes
    GET /api/clients
    POST /api/clients
    PUT /api/clients/{id}
    DELETE /api/clients/{id}
- Órdenes
  GET /api/orders
  POST /api/orders
  PUT /api/orders/{id}
  DELETE /api/orders/{id}

### Autor
Sebastian Chisavo Forero
  
