# 🎓 Bootcamp Platform Backend API - Antivirus para la Deserción

API Backend para una plataforma educativa (Antivirus para la Deserción) construida con .NET 9.0, que administra bootcamps, instituciones, usuarios, oportunidades y beneficios. Esta API permite a las instituciones educativas gestionar programas, estudiantes y recursos de forma segura.

## 🚀 Live Demo
**Backend API:** http://3.142.142.153:5000/

**Swagger UI (API Docs):** http://3.142.142.153:5000/swagger

## 📋 Tabla de Contenidos
- [Características](#-características)
- [Stack Tecnológico](#-stack-tecnológico)
- [Primeros Pasos](#-primeros-pasos)
- [Ejemplo de Consumo Frontend](#-ejemplo-de-consumo-frontend)
- [Autenticación](#-autenticación)
- [Documentación de la API](#-documentación-de-la-api-swagger)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Equipo de Desarrollo](#-equipo-de-desarrollo)

## ✨ Características

### 🔐 Autenticación y Autorización
- JWT Authentication (tokens)
- Control de acceso por roles (User/Admin)
- Gestión de usuarios, administradores, perfiles, roles

### 🔧 Características Adicionales
- CRUD de bootcamps, instituciones, oportunidades, servicios, beneficios
- Documentación Swagger (OAS 3.0)
- Permisos CORS habilitados para frontend
- Base de datos MySQL

## 🛠️ Stack Tecnológico

- **Backend**: .NET 9.0 Web API
- **ORM**: Entity Framework Core + MySQL
- **Docs**: Swagger/OpenAPI
- **Auth**: JWT Bearer Tokens

## 🚀 Primeros Pasos

### Requisitos:
- .NET 9.0 SDK
- MySQL
- Git

### Instalación local

1. **Clonar el repositorio**
   ```bash
   git clone <url-repositorio>
   cd AntivirusBackend/Antivirus
   ```

2. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

3. **Actualizar base de datos**
   ```bash
   dotnet ef database update
   ```

4. **Ejecutar la aplicación**
   ```bash
   dotnet run
   ```

5. **Acceder a la API**
   - API en: http://localhost:5000/
   - Swagger en: http://localhost:5000/swagger

## ⚡ Ejemplo de Consumo Frontend

### 1. Registro de usuario
```bash
curl -X POST "http://3.142.142.153:5000/api/users/register" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "usuario@demo.com",
    "password": "superseguro123",
    "name": "Juan",
    "lastName": "Pérez",
    "dateBirth": "2000-01-01"
  }'
```

**Nota:** Este endpoint es público, no requiere autenticación.

### 2. Login de usuario
```bash
curl -X POST "http://3.142.142.153:5000/api/users/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "usuario@demo.com",
    "password": "superseguro123"
  }'
```

**Response:** Recibirás un JWT token. Guárdalo y úsalo en el header Authorization para endpoints protegidos.

### 3. Usar JWT para endpoints protegidos
```bash
curl -H "Authorization: Bearer TU_TOKEN_JWT" \
     http://3.142.142.153:5000/api/bootcamps
```

**IMPORTANTE:** Solo los endpoints de registro y login NO requieren autenticación. El resto requiere el header:

```http
Authorization: Bearer TU_TOKEN_JWT
```

## 🔐 Autenticación

### Configuración JWT
- **Registro** (sin token):

  * `/api/users/register`
  * `/api/admins/register`

- **Login** (sin token): 

  * `/api/users/login`
  * `/api/admins/login`

- **El resto de endpoints** requieren JWT (usuarios o admins)

### Ejemplo de header para peticiones autenticadas:
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...
```

## 📚 Documentación de la API (Swagger)

API docs completas en http://3.142.142.153:5000/swagger

### 🔥 Resumen y ejemplos de endpoints principales

### Usuarios
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/api/users/register` | Crea un usuario normal (público) |
| POST | `/api/users/login` | Login de usuario (público) |
| POST | `/api/users/logout` | Logout (requiere token) |
| GET | `/api/users` | Listar usuarios (requiere token) |
| GET | `/api/users/{id}` | Traer usuario por ID (requiere token) |
| PUT | `/api/users/email/{email}` | Actualizar datos por email (requiere token) |
| DELETE | `/api/users/{id}` | Eliminar usuario (requiere token) |

### Admins
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/api/admins/register` | Crea un usuario admin (público) |
| POST | `/api/admins/login` | Login de admin (público) |
| POST | `/api/admins/logout` | Logout (requiere token) |
| GET | `/api/admins` | Listar admins (requiere token) |
| GET | `/api/admins/{id}` | Traer admin por ID (requiere token) |
| PUT | `/api/admins/email/{email}` | Actualizar admin por email (requiere token) |
| DELETE | `/api/admins/{id}` | Eliminar admin (requiere token) |

### Bootcamps
| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| GET | `/api/Bootcamp` | Lista bootcamps | Sí |
| POST | `/api/Bootcamp` | Crear bootcamp | Sí |
| GET | `/api/Bootcamp/{id}` | Bootcamp por ID | Sí |
| PUT | `/api/Bootcamp/{id}` | Actualizar | Sí |
| DELETE | `/api/Bootcamp/{id}` | Eliminar | Sí |

### Instituciones
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Institutions` | Lista instituciones |
| POST | `/api/Institutions` | Crear institución |
| GET | `/api/Institutions/{id}` | Institución por ID |
| PUT | `/api/Institutions/{id}` | Actualizar institución |
| DELETE | `/api/Institutions/{id}` | Eliminar institución |

### Oportunidades
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Opportunities` | Lista oportunidades |
| POST | `/api/Opportunities` | Crear oportunidad |
| GET | `/api/Opportunities/{id}` | Oportunidad por ID |
| PUT | `/api/Opportunities/{id}` | Actualizar oportunidad |
| DELETE | `/api/Opportunities/{id}` | Eliminar oportunidad |

### Beneficios
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Benefits` | Lista beneficios |
| POST | `/api/Benefits` | Crear beneficio |
| GET | `/api/Benefits/{id}` | Beneficio por ID |
| PUT | `/api/Benefits/{id}` | Actualizar beneficio |
| DELETE | `/api/Benefits/{id}` | Eliminar beneficio |

### Servicios
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Services` | Lista servicios |
| POST | `/api/Services` | Crear servicio |
| GET | `/api/Services/{id}` | Servicio por ID |
| PUT | `/api/Services/{id}` | Actualizar servicio |
| DELETE | `/api/Services/{id}` | Eliminar servicio |

## 📝 Estructura del Proyecto

```
AntivirusBackend/
├── Antivirus/
│   ├── Controllers/        # Controladores API
│   ├── Services/           # Servicios - Lógica de Negocio
│   ├── Models/             # Entidades DB
│   ├── Dtos/               # Data Transfer Objects
│   ├── Interfaces/         # Interfaces de Servicio
│   ├── Data/               # Contexto DB
│   ├── config/             # Configuración
│   ├── Migrations/         # EF Core Migrations
│   └── Program.cs          # Entry Point de la app
├── README.md
└── Antivirus.sln
```

## 👥 Equipo de Desarrollo


* [Anthony Muñoz](https://github.com/AnthonyCarmine)
* [María Camila Botero](https://github.com/mcamilabotero3)
* [María Alejandra Infante](https://github.com/MarialeInf)
* [Santiago Martínez](https://github.com/SantiagoMartinez22)
* [Esteban Montoya](https://github.com/emontoyab)
* [María Melisa Serna](https://github.com/Pantone7427)
* [Geny Marcela Vargas](https://github.com/genyvarsua)

