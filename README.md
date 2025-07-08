🎓 Bootcamp Platform Backend API
API Backend para una plataforma educativa construida con .NET 9.0, que administra bootcamps, instituciones, usuarios, oportunidades y beneficios. Esta API permite a las instituciones educativas gestionar programas, estudiantes y recursos de forma segura.

🚀 Live Demo
Backend API: http://3.142.142.153:5000/

Swagger UI (API Docs): http://3.142.142.153:5000/swagger

🗂️ Tabla de Contenidos
Características

Stack Tecnológico

Primeros Pasos

Ejemplo de Consumo Frontend

Autenticación

Documentación de la API

Estructura del Proyecto

Notas de Seguridad

Equipo de Desarrollo

✨ Características
JWT Authentication (tokens)

Control de acceso por roles (User/Admin)

Gestión de usuarios, admins, perfiles, roles

CRUD de bootcamps, instituciones, oportunidades, servicios, beneficios

Documentación Swagger (OAS 3.0)

Permisos CORS habilitados para frontend

Base de datos MySQL

💻 Stack Tecnológico
Backend: .NET 9.0 Web API

ORM: Entity Framework Core + MySQL

Docs: Swagger/OpenAPI

Auth: JWT Bearer Tokens

🏁 Primeros Pasos
Requisitos:

.NET 9.0 SDK

MySQL

Git

Instalación local
bash
Copiar
Editar
git clone <repository-url>
cd AntivirusBackend/Antivirus
dotnet restore
dotnet ef database update
dotnet run
API en: http://localhost:5000/

Swagger en: http://localhost:5000/swagger

⚡ Ejemplo de Consumo Frontend
1. Registro de usuario
bash
Copiar
Editar
curl -X POST "http://3.142.142.153:5000/api/users/register" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "usuario@demo.com",
    "password": "superseguro123",
    "name": "Juan",
    "lastName": "Pérez",
    "dateBirth": "2000-01-01"
  }'
Nota: Este endpoint es público, no requiere autenticación.

2. Login de usuario
bash
Copiar
Editar
curl -X POST "http://3.142.142.153:5000/api/users/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "usuario@demo.com",
    "password": "superseguro123"
  }'
Response:
Recibirás un JWT token. Guárdalo y úsalo en el header Authorization para endpoints protegidos.

3. Usar JWT para endpoints protegidos
bash
Copiar
Editar
curl -H "Authorization: Bearer TU_TOKEN_JWT"
     http://3.142.142.153:5000/api/bootcamps
IMPORTANTE:
Solo los endpoints de registro y login NO requieren autenticación. El resto requiere el header:

makefile
Copiar
Editar
Authorization: Bearer TU_TOKEN_JWT
🔐 Autenticación
Registro: /api/users/register y /api/admins/register (sin token)

Login: /api/users/login y /api/admins/login (sin token)

El resto de endpoints requieren JWT (usuarios o admins)

Ejemplo de header para peticiones autenticadas:

http
Copiar
Editar
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...
📚 Documentación de la API (Swagger)
API docs completas en http://3.142.142.153:5000/swagger

🔥 Resumen y ejemplos de endpoints principales
### Usuarios
POST /api/users/register
Descripción: Crea un usuario normal (público)

POST /api/users/login
Descripción: Login de usuario (público)

POST /api/users/logout
Descripción: Logout (requiere token)

GET /api/users
Descripción: Listar usuarios (requiere token)

GET /api/users/{id}
Descripción: Traer usuario por ID (requiere token)

PUT /api/users/email/{email}
Descripción: Actualizar datos por email (requiere token)

DELETE /api/users/{id}
Descripción: Eliminar usuario (requiere token)

### Admins
POST /api/admins/register
Descripción: Crea un usuario admin (público)

POST /api/admins/login
Descripción: Login de admin (público)

POST /api/admins/logout
Descripción: Logout (requiere token)

GET /api/admins
Descripción: Listar admins (requiere token)

GET /api/admins/{id}
Descripción: Traer admin por ID (requiere token)

PUT /api/admins/email/{email}
Descripción: Actualizar admin por email (requiere token)

DELETE /api/admins/{id}
Descripción: Eliminar admin (requiere token)

### Bootcamps
GET /api/Bootcamp
Descripción: Lista bootcamps
Auth: Sí

POST /api/Bootcamp
Descripción: Crear bootcamp
Auth: Sí

GET /api/Bootcamp/{id}
Descripción: Bootcamp por ID
Auth: Sí

PUT /api/Bootcamp/{id}
Descripción: Actualizar
Auth: Sí

DELETE /api/Bootcamp/{id}
Descripción: Eliminar
Auth: Sí

### Instituciones
GET /api/Institutions

POST /api/Institutions

GET /api/Institutions/{id}

PUT /api/Institutions/{id}

DELETE /api/Institutions/{id}

### Oportunidades
GET /api/Opportunities

POST /api/Opportunities

GET /api/Opportunities/{id}

PUT /api/Opportunities/{id}

DELETE /api/Opportunities/{id}

### Beneficios
GET /api/Benefits

POST /api/Benefits

GET /api/Benefits/{id}

PUT /api/Benefits/{id}

DELETE /api/Benefits/{id}

### Servicios
GET /api/Services

POST /api/Services

GET /api/Services/{id}

PUT /api/Services/{id}

DELETE /api/Services/{id}

