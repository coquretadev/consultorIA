# Documento de Requisitos del Bugfix

## Introducción

La aplicación AiConsulting.Api falla al iniciar porque Entity Framework Core intenta conectarse a PostgreSQL con una cadena de conexión nula. El código en `Program.cs` llama a `builder.Configuration.GetConnectionString("DefaultConnection")`, pero ninguno de los archivos de configuración (`appsettings.json` ni `appsettings.Development.json`) define una sección `ConnectionStrings` con la clave `DefaultConnection`. Esto provoca que `UseNpgsql()` reciba `null`, causando un fallo inmediato al arrancar la aplicación.

## Análisis del Bug

### Comportamiento Actual (Defecto)

1.1 CUANDO la aplicación inicia y `appsettings.json` no contiene una sección `ConnectionStrings` con la clave `DefaultConnection` ENTONCES el sistema pasa `null` a `UseNpgsql()` y falla con una excepción al intentar conectarse a la base de datos

1.2 CUANDO la aplicación inicia en el entorno de desarrollo y `appsettings.Development.json` no contiene una sección `ConnectionStrings` con la clave `DefaultConnection` ENTONCES el sistema no tiene cadena de conexión de respaldo y falla igualmente al arrancar

1.3 CUANDO el sistema intenta ejecutar el seed de datos en desarrollo (`SeedData.SeedAsync`) ENTONCES la operación falla porque el `DbContext` no puede establecer conexión con PostgreSQL debido a la cadena de conexión nula

### Comportamiento Esperado (Correcto)

2.1 CUANDO la aplicación inicia y `appsettings.json` contiene una sección `ConnectionStrings` con la clave `DefaultConnection` configurada con una cadena de conexión válida a PostgreSQL ENTONCES el sistema DEBERÁ pasar dicha cadena a `UseNpgsql()` y registrar el `DbContext` correctamente

2.2 CUANDO la aplicación inicia en el entorno de desarrollo y `appsettings.Development.json` contiene una sección `ConnectionStrings` con la clave `DefaultConnection` ENTONCES el sistema DEBERÁ utilizar esa cadena de conexión específica para el entorno de desarrollo

2.3 CUANDO el sistema ejecuta el seed de datos en desarrollo (`SeedData.SeedAsync`) ENTONCES la operación DEBERÁ completarse exitosamente utilizando la conexión configurada a PostgreSQL

### Comportamiento Sin Cambios (Prevención de Regresión)

3.1 CUANDO la configuración de logging está definida en `appsettings.json` ENTONCES el sistema DEBERÁ CONTINUAR utilizando los niveles de log configurados sin alteración

3.2 CUANDO la configuración JWT (`Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`, `Jwt:ExpiryMinutes`) está definida en `appsettings.json` ENTONCES el sistema DEBERÁ CONTINUAR utilizando esos valores para la autenticación JWT sin modificación

3.3 CUANDO `AllowedHosts` está configurado en `appsettings.json` ENTONCES el sistema DEBERÁ CONTINUAR respetando esa configuración sin cambios

3.4 CUANDO el `AiConsultingDbContext` se registra en el contenedor de inyección de dependencias ENTONCES el sistema DEBERÁ CONTINUAR usando `UseNpgsql()` como proveedor de base de datos

3.5 CUANDO los paquetes NuGet de infraestructura (`Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.AspNetCore.Identity.EntityFrameworkCore`) están referenciados ENTONCES el sistema DEBERÁ CONTINUAR utilizando las mismas versiones sin modificación
