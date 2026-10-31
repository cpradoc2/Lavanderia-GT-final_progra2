# Pfinal_labanderia — Sistema de Lavandería

Proyecto final del curso **Programación II 2026**.
Aplicación de escritorio para gestionar una lavandería: clientes, recepciones, tarifas, usuarios,
entregas, procesos, tipos de prenda y servicios, con una pantalla de Inicio (dashboard).

> **Repositorio de entrega:** [FrozenCureBand/Lavanderia-GT-final_progra2](https://github.com/FrozenCureBand/Lavanderia-GT-final_progra2)

> Estado de entrega, 30 de octubre de 2026: Release compilado sin errores según el usuario. El usuario también confirmó que la copia funcionó en otro equipo con conexión, Login, Panel, Dashboard y CRUD.
> Los documentos 01–09 están COMPLETADOS por confirmación del estudiante; el documento 11 sigue ACTIVO. Las evidencias principales están en `docs`; la captura de Clientes permanece local por contener datos de contacto. Ver [ESTADO_DOCUMENTOS.md](ESTADO_DOCUMENTOS.md).

## Tecnologías (comprobadas en los archivos `.csproj`)

| Elemento | Valor |
|---|---|
| Lenguaje | C# |
| Plataforma | .NET Framework 4.8 |
| Interfaz | Windows Forms |
| Base de datos | SQL Server (proveedor `System.Data.SqlClient`) |
| Solución | `Pfinal_labanderia.slnx` (formato XML de solución) |
| Paquetes NuGet | `System.Data.SqlClient 4.9.1`, `System.Configuration.ConfigurationManager 10.0.12` (Datos) · `FontAwesome.Sharp 6.6.0` (Presentación) |

## Arquitectura por capas

| Capa | Proyecto | Contenido actual |
|---|---|---|
| Entidad | `Entidad` | Una clase por tabla (`Cliente`, `Recepcion`, `Tarifa`, `Usuario`, `Entrega`, `Proceso`, `TipoPrenda`, `Servicio`, `Rol`, `Acceso`) y apoyo (`OpcionLista`, `ResumenInicio`, `DatoGrafica`) |
| Datos | `Datos` | `Conexion` y una clase `D...` por módulo (incluye `DLogin` y `DRol`), con consultas parametrizadas |
| Negocio | `Negocio` | Una clase `N...` por módulo (incluye `NLogin` y `NRol`) con las validaciones y reglas |
| Presentación | `Pfinal_labanderia` | Formularios: `logoing`, `precentacion`, `inicio`, `clientes`, `usuarios`, `Tarifas`, `Recepciones`, `Entregas`, `Procesos`, `TiposPrenda`, `Servicios`, `roles`; sesión en `SesionActual` |

Referencias entre proyectos (tal como están hoy):

```
Presentación (Pfinal_labanderia) ──► Negocio, Datos, Entidad
Negocio ──► Datos, Entidad
Datos   ──► Entidad
```

## Estructura de carpetas

```
Pfinal_labanderia\
├─ README.md               Este archivo
├─ AGENTS.md               Reglas de trabajo para asistentes de IA
├─ MEMORY.md               Hechos comprobados del proyecto
├─ ESTADO_DOCUMENTOS.md    Estado de los documentos 01 a 11
├─ .gitignore
├─ skills\                 Guías paso a paso (carpeta plana, 11 archivos .md)
├─ database\               Conexión y scripts SQL (carpeta plana)
├─ docs\                   Documentación de apoyo (carpeta plana)
└─ src\
   └─ Pfinal_labanderia\         Solución del sistema (NombreSistema)
      ├─ Pfinal_labanderia.slnx
      ├─ Entidad\  Datos\  Negocio\
      ├─ Pfinal_labanderia\      Presentación (formularios, App.config y Conexion.config.ejemplo)
      └─ packages\               Paquetes NuGet (no se suben; Visual Studio los restaura)
```

## Cómo clonar y abrir el proyecto

Requisitos: Git, Visual Studio con soporte para .NET Framework 4.8 y archivos `.slnx`, y SQL Server con la base
`BD_LavanderiaGT`.

1. Clonar (en CMD) en una carpeta de ruta corta, por ejemplo `C:\Proyectos`. Windows limita las rutas a
   260 caracteres y algunos paquetes NuGet tienen nombres largos:
   ```
   cd /d C:\Proyectos
   git clone https://github.com/FrozenCureBand/Lavanderia-GT-final_progra2.git
   ```
2. Crear el archivo de conexión. **No viene en el repositorio** porque lleva la contraseña de SQL Server:
   ```
   cd Lavanderia-GT-final_progra2\src\Pfinal_labanderia\Pfinal_labanderia
   copy Conexion.config.ejemplo Conexion.config
   notepad Conexion.config
   ```
   Cambiar el servidor, el usuario y `ESCRIBE_AQUI_TU_CONTRASENA` por los datos de tu SQL Server y guardar.
   `Conexion.config` está en `.gitignore`: nunca se sube a GitHub.
3. Si la base `BD_LavanderiaGT` todavía no tiene roles, ejecutar una vez `database\02_Login_Roles.sql`.
   Crea los roles Administrador, Recepcionista y Operador y deja como Administrador al primer usuario.
   Los demás usuarios necesitan un rol (se asigna en Usuarios) para poder iniciar sesión.
4. Abrir `src\Pfinal_labanderia\Pfinal_labanderia.slnx` con Visual Studio y compilar. Visual Studio descarga
   los paquetes NuGet la primera vez.

Si al compilar aparece el error **MSB3030** "No se pudo copiar el archivo ...\Conexion.config porque no se
encontró", falta hacer el paso 2. Si al iniciar el programa aparece "Problema con la configuración de la
conexión", revisa el contenido de `Conexion.config`.

## Trabajo en equipo con Git

```
git pull                         Traer los cambios del compañero antes de empezar
git status                       Ver qué archivos cambiaron
git add .                        Preparar los cambios
git commit -m "Qué se hizo"      Guardar los cambios con un mensaje claro
git push                         Subir los cambios a GitHub
```

Para poder hacer `git push`, el dueño del repositorio debe agregar al compañero como colaborador
(GitHub → Settings → Collaborators → Add people).

## Orden de trabajo

1. 01 Inspección inicial
2. 02 Conexión a SQL Server
3. 03 a 05 CRUD (simple, relacional, maestro-detalle)
4. 06 Git y GitHub colaborativo (después de los CRUD)
5. 07 Login y roles (al final)
6. 08 Panel de navegación · 09 Dashboard · 10 Revisión · 11 Publicación

## Documentación

- [AGENTS.md](AGENTS.md) — reglas obligatorias
- [ESTADO_DOCUMENTOS.md](ESTADO_DOCUMENTOS.md) — qué está activo o bloqueado
- [MEMORY.md](MEMORY.md) — hechos comprobados
- [docs/COMANDOS_RAPIDOS.md](docs/COMANDOS_RAPIDOS.md) — comandos CMD
- [database/CONEXION_DATABASE.md](database/CONEXION_DATABASE.md) — estado de la conexión

## Actualizacion de publicacion y entrega, 30 de octubre de 2026

Requisito de ejecucion: Windows con .NET Framework 4.8 y acceso de red al SQL Server configurado.

Ejecutable Release: src\Pfinal_labanderia\Pfinal_labanderia\bin\Release\Pfinal_labanderia.exe.

Archivos necesarios junto al ejecutable: Pfinal_labanderia.exe.config, Conexion.config, Datos.dll, Entidad.dll, Negocio.dll y FontAwesome.Sharp.dll.

Carpeta de entrega generada: C:\Entrega_LavanderiaGT\Release. Conserva junta la carpeta completa.

Conexion.config debe estar junto al ejecutable y contiene la cadena privada de conexion. En ese archivo se configuran servidor y puerto en Data Source, base en Initial Catalog, y usuario SQL en User ID. La contraseña se mantiene solo en ese archivo local; no se sube a GitHub. App.config carga Conexion.config.

El usuario confirmo que el ejecutable se conecto a SQL Server desde el equipo de destino y que funcionaron las pruebas de Login, Panel, Dashboard y CRUD.

Se encontraron archivos `BD_LavanderiaGT.bak` en `database\` de esta copia local y en `C:\Respaldos`; cada uno figura con 6,410,240 bytes y fecha 30/10/2026. El `.bak` de `database\` está ignorado por Git, así que no forma parte de GitHub ni de un clon nuevo. No se verificó que ambos archivos sean idénticos ni se probó restaurarlos.

Evidencias visuales disponibles en docs, carpeta plana: `Evidencia_Login.png`, `Evidencia_Panel.png`, `Evidencia_Dashboard.png`, `Evidencia_CRUD_OtroModulo.png` y `Evidencia_CRUD_OtroModuloAgregar1.png` a `Evidencia_CRUD_OtroModuloAgregar3.png`. Las capturas CRUD muestran el módulo Tipos de prenda y el alta de un registro; edición y eliminación fueron reportadas por el usuario, sin capturas específicas.

`Evidencia_Clientes.png` permanece local y no se incluye en GitHub porque muestra nombres, teléfonos, correos y direcciones. Debe anonimizarse antes de compartirla públicamente.

Responsables: asignacion nominal pendiente de confirmacion por el equipo.
