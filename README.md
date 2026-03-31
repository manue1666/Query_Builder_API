# Query Builder API

Una API REST moderna construida con .NET Core 8 que facilita la generación, almacenamiento y gestión de consultas SQL usando inteligencia artificial mediante el servicio Groq.

## 📋 Descripción

Query Builder API es una aplicación backend que permite a los usuarios:
- Registrarse y autenticarse de forma segura
- Crear y gestionar bases de datos
- Generar consultas SQL automáticamente usando IA
- Almacenar y recuperar histórico de consultas
- Acceder a múltiples bases de datos simultáneamente

## 🚀 Características Principales

- **Autenticación JWT**: Sistema de autenticación seguro con tokens JWT
- **Generación de Queries IA**: Integración con Groq API para generar consultas SQL inteligentes
- **ORM Moderno**: Entity Framework Core con PostgreSQL
- **Documentación Interactiva**: Swagger/OpenAPI integrado
- **CORS Habilitado**: Configurado para conectar con frontend en `localhost:3000`
- **Migraciones DB**: Historial completo de cambios en la base de datos

## 🛠️ Stack Tecnológico

- **Framework**: ASP.NET Core 8.0
- **Lenguaje**: C#
- **Base de Datos**: PostgreSQL
- **ORM**: Entity Framework Core 8.0
- **Autenticación**: JWT Bearer Token
- **IA**: Groq API Library
- **Documentación API**: Swagger/Swashbuckle

## 📦 Dependencias

```xml
- GroqApiLibrary v2.0.0
- Microsoft.AspNetCore.Authentication.JwtBearer v8.0
- Microsoft.AspNetCore.OpenApi v8.0.23
- Microsoft.EntityFrameworkCore v8.0
- Microsoft.EntityFrameworkCore.Tools v8.0
- Microsoft.IdentityModel.Tokens v8.17.0
- Npgsql.EntityFrameworkCore.PostgreSQL v8.0
- Swashbuckle.AspNetCore v6.6.2
- System.IdentityModel.Tokens.Jwt v8.17.0
```

## 📁 Estructura del Proyecto

```
QueryBuilderAPI/
├── Controllers/              # Controladores de la API
│   ├── AuthController.cs    # Endpoints de autenticación
│   ├── DatabaseController.cs # Endpoints de gestión de bases de datos
│   └── QueryController.cs   # Endpoints de generación de consultas
├── Data/
│   └── AppDbContext.cs      # Contexto de Entity Framework
├── Models/                  # Modelos de datos
│   ├── Users.cs
│   ├── Database.cs
│   ├── Query.cs
│   ├── LoginDto.cs
│   ├── RegisterDto.cs
│   ├── GenerateQueryRequest.cs
│   ├── ApiResponse.cs
│   └── ...
├── Services/                # Lógica de negocio
│   ├── AuthService.cs      # Autenticación y autorización
│   ├── DatabaseService.cs  # Gestión de bases de datos
│   ├── QueryService.cs     # Generación de consultas
│   ├── GroqService.cs      # Integración con Groq API
│   ├── TokenService.cs     # Gestión de JWT
│   └── GroqPrompts.cs      # Prompts para IA
├── Migrations/              # Migraciones de base de datos
├── Program.cs              # Configuración de la aplicación
├── appsettings.json        # Configuración general
├── appsettings.Development.json
└── QueryBuilderAPI.csproj  # Archivo del proyecto

```

## 🔧 Configuración

### Requisitos Previos

- .NET Core 8.0 SDK o superior
- PostgreSQL 12 o superior
- Clave API de Groq

### Instalación

1. **Clonar el repositorio**
```bash
git clone <repository-url>
cd Query_Builder_API
```

2. **Configurar la Base de Datos**

Edita `appsettings.json` con tu conexión PostgreSQL:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=query_builder;Username=your_user;Password=your_password"
  },
  "Jwt": {
    "SecretKey": "your-secret-key-min-32-chars-long"
  },
  "GroqApiKey": "your-groq-api-key"
}
```

3. **Aplicar Migraciones**
```bash
dotnet ef database update
```

4. **Restaurar Dependencias**
```bash
dotnet restore
```

5. **Ejecutar la Aplicación**
```bash
dotnet run
```

La API estará disponible en: `https://localhost:5001`

## 📚 Endpoints Disponibles

### Autenticación

#### Registro
```http
POST /api/auth/register
Content-Type: application/json

{
    "name": "Juan Pérez",
    "email": "juan@example.com",
    "password": "SecurePassword123"
}
```

**Validaciones:**
- `name`: Requerido, mínimo 2 caracteres
- `email`: Requerido, formato válido de email, debe ser único
- `password`: Requerido, mínimo 8 caracteres

**Respuestas:**

| Status | Descripción | Respuesta |
|--------|-------------|-----------|
| 201 | Registro exitoso | `{ "success": true, "message": "Usuario registrado exitosamente", "data": { "userId": 1, "email": "juan@example.com" } }` |
| 400 | Validación fallida | `{ "success": false, "message": "Email ya está registrado" }` |
| 500 | Error del servidor | `{ "success": false, "message": "Error interno del servidor" }` |

---

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "juan@example.com",
  "password": "SecurePassword123"
}
```

**Validaciones:**
- `email`: Requerido, debe existir en la base de datos
- `password`: Requerido, debe ser correcto

**Respuestas:**

| Status | Descripción | Respuesta |
|--------|-------------|-----------|
| 200 | Login exitoso | `{ "success": true, "message": "Login exitoso", "data": { "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...", "userId": 1, "email": "juan@example.com" } }` |
| 401 | Credenciales inválidas | `{ "success": false, "message": "Email o contraseña incorrectos" }` |
| 400 | Campos requeridos faltantes | `{ "success": false, "message": "Email y contraseña son requeridos" }` |
| 500 | Error del servidor | `{ "success": false, "message": "Error interno del servidor" }` |

---

### Bases de Datos

#### Crear Base de Datos
```http
POST /api/database/create
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "name": "Mi Base de Datos",
  "description": "Base de datos de prueba",
  "sqlSchema": "CREATE TABLE users (id INT PRIMARY KEY, email VARCHAR(255), name VARCHAR(100));"
}
```

**Autenticación:** Bearer Token (JWT) requerido

**Validaciones:**
- `name`: Requerido
- `description`: Requerido
- `sqlSchema`: Requerido, esquema SQL válido

**Respuestas:**

| Status | Descripción | Respuesta |
|--------|-------------|-----------|
| 200 | Base de datos creada | `{ "success": true, "message": "Database created successfully" }` |
| 401 | No autenticado | `{ "success": false, "message": "Invalid user ID in token." }` |
| 500 | Error del servidor | `{ "success": false, "message": "Error al crear la base de datos" }` |

---

#### Obtener Todas las Bases de Datos del Usuario
```http
GET /api/database/all
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Autenticación:** Bearer Token (JWT) requerido

**Parámetros:** Ninguno

**Respuestas:**

| Status | Descripción | Respuesta |
|--------|-------------|-----------|
| 200 | Éxito | Array de objetos Database pertenecientes al usuario |
| 401 | No autenticado | `{ "success": false, "message": "Invalid user ID in token." }` |
| 404 | Sin bases de datos | `{ "success": false, "message": "No databases found." }` |

---

#### Obtener Base de Datos por ID
```http
GET /api/database/{id}
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Autenticación:** Bearer Token (JWT) requerido

**Parámetros:**
- `id` (path): ID de la base de datos (entero positivo)

**Respuestas:**

| Status | Descripción | Respuesta |
|--------|-------------|-----------|
| 200 | Éxito | Objeto Database solicitado |
| 401 | No autenticado | `{ "success": false, "message": "Invalid user ID in token." }` |
| 404 | No encontrada | `{ "success": false, "message": "No database Found" }` |

---

#### Actualizar Base de Datos
```http
PUT /api/database/{id}
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "name": "Base de Datos Actualizada",
  "description": "Descripción actualizada",
  "sqlSchema": "CREATE TABLE users (id INT PRIMARY KEY, email VARCHAR(255));"
}
```

**Autenticación:** Bearer Token (JWT) requerido

**Parámetros:**
- `id` (path): ID de la base de datos

**Validaciones:**
- `name`: Requerido
- `description`: Requerido
- `sqlSchema`: Requerido, esquema SQL válido

**Respuestas:**

| Status | Descripción | Respuesta |
|--------|-------------|-----------|
| 200 | Actualización exitosa | `{ "success": true, "message": "Database updated successfully" }` |
| 401 | No autenticado | `{ "success": false, "message": "Invalid user ID in token." }` |
| 404 | No encontrada | `{ "success": false, "message": "No database Found" }` |

---

#### Eliminar Base de Datos
```http
DELETE /api/database/{id}
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Autenticación:** Bearer Token (JWT) requerido

**Parámetros:**
- `id` (path): ID de la base de datos a eliminar

**Nota:** La eliminación es permanente y eliminará también todas las consultas asociadas.

**Respuestas:**

| Status | Descripción | Respuesta |
|--------|-------------|-----------|
| 200 | Eliminación exitosa | `{ "success": true, "message": "Database deleted successfully" }` |
| 401 | No autenticado | `{ "success": false, "message": "Invalid user ID in token." }` |
| 404 | No encontrada | `{ "success": false, "message": "No database Found" }` |

---

### Consultas

#### Generar Consulta con IA
```http
POST /api/query/generate
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "databaseId": 1,
  "description": "Dame los usuarios con más de 5 pedidos y muestra su email y número total de pedidos"
}
```

**Autenticación:** Bearer Token (JWT) requerido

**Validaciones:**
- `databaseId`: Requerido, debe existir y pertenece al usuario
- `description`: Requerido, descripción clara en lenguaje natural

**Proceso:**
1. Valida que la base de datos existe y pertenece al usuario
2. Obtiene el esquema de la base de datos
3. Envía la descripción a Groq API junto con el esquema
4. Groq genera una consulta SQL optimizada
5. Almacena la consulta en la base de datos
6. Retorna la consulta generada

**Respuestas:**

| Status | Descripción | Respuesta |
|--------|-------------|-----------|
| 200 | Consulta generada | `{ "success": true, "message": "Query successfully generated", "data": { Objeto Query generado } }` |
| 404 | Base de datos no encontrada | `{ "success": false, "message": "Database not found for the provided ID" }` |

---

#### Obtener Todas las Consultas de una Base de Datos
```http
GET /api/query/all/{databaseId}
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Autenticación:** Bearer Token (JWT) requerido

**Parámetros:**
- `databaseId` (path): ID de la base de datos

**Respuestas:**

| Status | Descripción | Respuesta |
|--------|-------------|-----------|
| 200 | Éxito | `{ "success": true, "message": "Queries retrieved successfully", "data": [ Array de objetos Query ] }` |

---

#### Obtener Consulta por ID
```http
GET /api/query/{id}
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Autenticación:** Bearer Token (JWT) requerido

**Parámetros:**
- `id` (path): ID de la consulta

**Respuestas:**

| Status | Descripción | Respuesta |
|--------|-------------|-----------|
| 200 | Éxito | `{ "success": true, "message": "Query retrieved successfully", "data": { Objeto Query } }` |
| 404 | No encontrada | `{ "success": false, "message": "Query not found" }` |

---

#### Eliminar Consulta
```http
DELETE /api/query/{id}
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Autenticación:** Bearer Token (JWT) requerido

**Parámetros:**
- `id` (path): ID de la consulta a eliminar

**Respuestas:**

| Status | Descripción | Respuesta |
|--------|-------------|-----------|
| 200 | Eliminación exitosa | `{ "success": true, "message": "Query deleted successfully" }` |
| 404 | No encontrada | `{ "success": false, "message": "Query not found" }` |

---

## 📋 Códigos de Estado HTTP

| Código | Descripción |
|--------|-------------|
| 200 | Solicitud exitosa |
| 201 | Recurso creado exitosamente |
| 400 | Solicitud inválida / validación fallida |
| 401 | No autenticado / token inválido |
| 403 | Acceso prohibido / sin permisos |
| 404 | Recurso no encontrado |
| 500 | Error interno del servidor |
| 503 | Servicio no disponible |

---

## 🔐 Autenticación

La API utiliza **JWT Bearer Token** para autenticación. Incluye el token en el header `Authorization`:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

El token JWT tiene una duración de **24 horas**. Después, será necesario hacer login nuevamente.

---

## 🤖 Generación de Consultas con IA

El servicio utiliza Groq API para generar consultas SQL basadas en prompts en lenguaje natural. El proceso:

1. Usuario envía un prompt con su descripción
2. Sistema obtiene el esquema de la base de datos
3. Groq genera una consulta SQL optimizada
4. La consulta se almacena en la base de datos
5. Se retorna al usuario

## 📊 Modelos de Datos

### Usuario
```csharp
public class Users
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}
```

### Base de Datos
```csharp
public class Database
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string ConnectionString { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### Consulta
```csharp
public class Query
{
    public int Id { get; set; }
    public int DatabaseId { get; set; }
    public int UserId { get; set; }
    public string Prompt { get; set; }
    public string GeneratedQuery { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

## 🧪 Pruebas de API

Usa el archivo `QueryBuilderAPI.http` incluido en el proyecto para probar los endpoints:

```
@baseUrl = https://localhost:5001
@token = your-jwt-token-here

### Registro
POST {{baseUrl}}/api/auth/register
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Test123"
}
```

También puedes usar Swagger en: `https://localhost:5001/swagger/index.html`

## 🔗 CORS

La API está configurada para aceptar solicitudes desde:
- `http://localhost:3000` (Frontend)

Para agregar más orígenes, modifica la configuración en `Program.cs`.

## 📝 Variables de Entorno

Crea un archivo `.env` o usa User Secrets:

```bash
dotnet user-secrets set "Jwt:SecretKey" "your-secret-key"
dotnet user-secrets set "GroqApiKey" "your-groq-api-key"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
```



**Última actualización**: Marzo 2026  
**Versión**: 1.0.0
