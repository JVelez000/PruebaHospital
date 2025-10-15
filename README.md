# 🏥 Sistema de Gestión Hospitalaria - Hospital San Vicente

## 📋 ¿De qué va esto?
Este es un sistema web hecho en **ASP.NET Core** para llevar todo el control de pacientes, médicos y citas del Hospital San Vicente. La idea es dejar atrás los procesos manuales y las hojas de Excel llenas de errores, todo digitalizado y más ordenadito.

## 🎯 Qué puedes hacer con el sistema

### 👥 Pacientes
- Registrar y editar pacientes con todos sus datos.
- Que no se repita el documento de identidad.
- Ver la lista completa de pacientes.
- Interfaz sencilla y moderna con Bootstrap.

### 🩺 Médicos
- Agregar y editar médicos con su especialidad.
- Documento único por médico.
- Evitar que haya dos médicos con el mismo nombre y especialidad.
- Filtrar la lista por especialidad.
- Todo responsive y profesional.

### 📅 Citas médicas
- Agendar citas seleccionando paciente, médico, fecha y hora.
- Detectar conflictos de horarios (que nadie se empalme).
- Estados de la cita: Pendiente, Confirmada, Atendida, Cancelada, No Asistió.
- Listado por paciente o por médico.
- Simulación de envío de email de confirmación.
- Historial de emails enviados.
- Cambiar estado o cancelar citas cuando sea necesario.

## 🛠️ Tecnologías que usamos

**Backend**
- ASP.NET Core 8.0
- Entity Framework Core para la base de datos
- FluentValidation para validaciones chidas
- LINQ para consultar la base
- MySQL como base de datos

**Frontend**
- Bootstrap 5.3
- Bootstrap Icons
- JavaScript ES6
- jQuery Validation para validar en el navegador

**Arquitectura**
- MVC (Modelo-Vista-Controlador)
- Repository Pattern para no andar con SQL directo por todos lados
- Service Layer para la lógica del negocio
- Dependency Injection para no crear objetos a mano

## 🛠️ Qué necesitas para correrlo
- .NET SDK 8.0 o superior
- MySQL 8.0 o superior
- Un navegador moderno (Chrome, Firefox, Edge)
- Base de datos lista y con permisos de creación

## 🚀 Cómo ponerlo a funcionar
1. Clonar el repositorio:
```bash
git clone https://github.com/tuusuario/pruebahospital.git
cd pruebahospital
```

2. Configurar la base de datos en `appsettings.json` con tus credenciales:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HospitalDB;Uid=tu_usuario;Pwd=tu_contraseña;"
  }
}
```

3. Aplicar migraciones:
```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

4. Ejecutar la app:
```bash
# Modo desarrollo con hot reload
dotnet watch

# Modo producción
dotnet run
```

5. Abrir en el navegador:
```
https://localhost:7000
```
*(el puerto puede variar, revisa la consola)*

## 📸 Cómo se ve el flujo
- **Home/Dashboard**: estadísticas y accesos rápidos a todo.
- **Pacientes**: listar, crear, editar y eliminar con confirmación.
- **Médicos**: listar, filtrar por especialidad y agregar nuevos.
- **Citas**: agendar, validar conflictos, cambiar estado y ver historial de emails.

## 🔒 Seguridad y validaciones
- Documento único por paciente y médico.
- Nombre + especialidad únicos para médicos.
- Evita conflictos de horarios.
- Validaciones de emails, teléfonos y edades.
- Anti-forgery tokens en formularios.
- Mensajes claros de error al usuario.

## 🐛 Problemas comunes
- No conecta la DB → revisar MySQL y credenciales.
- Migraciones pendientes → `dotnet ef migrations add InitialFix` y `dotnet ef database update`.
- Puerto ocupado → cambiar puerto o matar proceso.

## 👨‍💻 Info Personal
- Nombre: Jeims Velez
- Correo: jeims1221jeims@gmail.com
- Fecha de desarrollo: Octure 14 2025

## 📄 Licencia
Este proyecto es una prueba técnica de el modulo 5.3 C# Asp.net

