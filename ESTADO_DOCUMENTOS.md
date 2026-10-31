# ESTADO_DOCUMENTOS.md

Control de avance del proyecto **Pfinal_labanderia** · Programación II 2026.
Solo el estudiante autoriza un cambio de estado. El asistente actualiza esta tabla
después de recibir la autorización y de comprobar el resultado.

**Documento activo: 11**

> La asignación del equipo indicada por el estudiante numera Login y roles como Guía 06 (Integrante 1), Panel como Guía 07 y Dashboard como Guía 08. En las rutas locales, esos materiales aparecen como `skills/07_Login_Roles.md`, `skills/08_Panel_Navegacion.md` y `skills/09_Dashboard.md` respectivamente.

| # | Documento | Skill | Estado | Nota |
|---|---|---|---|---|
| 01 | Inspección inicial | [01_Inspeccion_Inicial.md](skills/01_Inspeccion_Inicial.md) | **COMPLETADO** | El estudiante confirmó el árbol; rutas, cuatro proyectos y compilación comprobados. |
| 02 | Conexión SQL Server | [02_Conexion_SQLServer.md](skills/02_Conexion_SQLServer.md) | **COMPLETADO** | La aplicación Release mostró conexión exitosa en el equipo destino, confirmado por el estudiante. La prueba F5 en Visual Studio no se hizo; se verificó la aplicación publicada. |
| 03 | CRUD simple | [03_CRUD_Simple.md](skills/03_CRUD_Simple.md) | **COMPLETADO** | CRUD por capas y consulta/paginación documentados; el estudiante confirmó operaciones CRUD en la aplicación del equipo destino. |
| 04 | CRUD relacional | [04_CRUD_Relacional.md](skills/04_CRUD_Relacional.md) | **COMPLETADO** | Relaciones y protección de llaves foráneas revisadas; pruebas con registros y confirmación del estudiante de operaciones CRUD en el equipo destino. |
| 05 | Maestro-detalle | [05_Maestro_Detalle.md](skills/05_Maestro_Detalle.md) | **COMPLETADO** | Cierre definitivo autorizado y confirmado por el estudiante (2026-10-30). La confirmación se registra como reporte del estudiante; no se ejecutaron pruebas adicionales en esta actualización. |
| 06 | Git colaborativo | [06_Git_Colaborativo.md](skills/06_Git_Colaborativo.md) | **COMPLETADO** | Cierre definitivo autorizado y confirmado por el estudiante (2026-10-30). Los cambios de este hilo se publicaron en el fork `FrozenCureBand`; el cumplimiento del trabajo colaborativo queda reportado por el estudiante, sin verificación independiente del acceso del compañero. |
| 07 | Login y roles | [07_Login_Roles.md](skills/07_Login_Roles.md) | **COMPLETADO** | Completado para Integrante 1 bajo la asignación de equipo Guía 06. El usuario confirmó Login válido y la captura muestra sesión con rol Administrador; pruebas de otros roles no constan. |
| 08 | Panel de navegación | [08_Panel_Navegacion.md](skills/08_Panel_Navegacion.md) | **COMPLETADO** | Cierre definitivo autorizado y confirmado por el estudiante (2026-10-30). El Panel y la navegación se reportan probados en el equipo destino; no se repitieron pruebas individuales en esta actualización. |
| 09 | Dashboard | [09_Dashboard.md](skills/09_Dashboard.md) | **COMPLETADO** | Cierre definitivo autorizado por el estudiante (2026-10-30), quien confirma que se cumple el criterio grupal de pruebas establecido previamente. No se recibieron mensajes individuales de los otros integrantes en esta conversación. |
| 10 | Revisión del proyecto | [10_Revision_Proyecto.md](skills/10_Revision_Proyecto.md) | BLOQUEADO | Skill PENDIENTE |
| 11 | Publicación | [11_Publicacion.md](skills/11_Publicacion.md) | **ACTIVO** | Release compilado según el usuario; siete archivos copiados a `C:\Entrega_LavanderiaGT\Release`; el usuario informó pruebas correctas en otro equipo. Se localizaron `.bak` en `database\` local y `C:\Respaldos`; ambos pesan 6,410,240 bytes, no se probó restauración ni se comparó hash. El de `database\` está ignorado por Git. README y evidencias publicables disponibles. Captura de Clientes no se publica por contener datos de contacto. Los documentos 01–09 quedaron COMPLETADOS por confirmación del estudiante; el 11 permanece ACTIVO. |

## Estados

- **ACTIVO**: documento en el que se trabaja ahora (solo uno a la vez).
- **BLOQUEADO**: no se puede trabajar hasta que el estudiante lo autorice.
- **PENDIENTE**: todavía no se ha planificado.
- **COMPLETADO**: terminado y confirmado por el estudiante.

## Historial

| Fecha | Documento | Cambio |
|---|---|---|
| 2026-10-15 | 01 | Documento 01 activado. 02–10 bloqueados. |
| 2026-10-15 | 01 | Paso 1: estructura documental creada y verificada con `tree /f`. |
| 2026-10-15 | 01 | El estudiante confirmó el árbol y el nombre `Pfinal_labanderia`. |
| 2026-10-15 | 01 | Paso 2: solución movida a `src\Pfinal_labanderia\`; rutas verificadas con `if exist`. |
| 2026-10-15 | 08 | Cambio puntual pedido por el estudiante: el botón Usuarios abre `usuarios` en `panelPrincipal`. El 08 sigue BLOQUEADO. |
| 2026-10-15 | 01 | La solución compila con MSBuild desde la nueva ubicación (código de salida 0). |
| 2026-10-15 | 08 | El estudiante probó el botón Usuarios: funciona. |
| 2026-10-15 | 08 | Cambio puntual pedido por el estudiante: Clientes, Recepciones y Tarifas abren su formulario en `panelPrincipal`; compila. El 08 sigue BLOQUEADO. |
| 2026-10-16 | 02 | Prueba de acceso de solo lectura pedida por el estudiante: `BD_LavanderiaGT` accesible con `sa`. El 02 sigue BLOQUEADO. |
| 2026-10-16 | 02–03 | Pedido del estudiante: `App.config` → `BD_LavanderiaGT` y CRUD de Clientes en 4 capas. Compila; lectura comprobada. Estados sin cambiar hasta su autorización. |
| 2026-10-17 | 03 | El estudiante devolvió `clientes.cs` y `clientes.Designer.cs` a su versión original (sin CRUD). |
| 2026-10-17 | 02 | Instrucción del estudiante: clase de conexión con `try-catch` y prueba inicial vía Negocio. Compila; conexión y mensajes de error comprobados. |
| 2026-10-17 | 03 | Autorizado: solo CONSULTAR de Clientes con paginación de 10 en Presentación. Compila; prueba automática correcta. |
| 2026-10-23 | — | Rediseño visual del login autorizado (solo `logoing.Designer.cs`; la lógica no cambia). Compila; captura revisada. |
| 2026-10-23 | 08–09 | Pedido del estudiante: formulario `inicio` (dashboard con datos reales) y `bnt_inicio`. Compila; prueba con la base real correcta. 08 y 09 siguen BLOQUEADOS. |
| 2026-10-23 | 03 | Pedido del estudiante: CONSULTAR con paginación en Recepciones, Tarifas y Usuarios (sin mostrar contraseñas). Compila; prueba automática correcta. |
| 2026-10-23 | 03–04 | Pedido del estudiante: Nuevo/Cancelar/Guardar/Editar/Eliminar en Clientes, Recepciones, Tarifas y Usuarios (selección con casillas, claves SHA-256). Compila; prueba con registros de prueba correcta y base intacta. |
| 2026-10-23 | 03 | Pedido del estudiante: BUSCAR en la base (sin distinguir mayúsculas ni tildes) en los 4 formularios. Compila; prueba de solo lectura correcta. |
| 2026-10-24 | 03–05 | Integración del proyecto del compañero: Entregas, Procesos, TiposPrenda y Servicios rehechos con la estructura del proyecto y conectados al menú. Compila; pruebas correctas y base intacta. |
| 2026-10-24 | — | Pedido del estudiante: rediseño visual de los 11 formularios (Segoe UI, menú azul marino, iconos nuevos) sin cambiar la lógica. Compila; eventos y nombres de controles iguales al respaldo; pruebas de solo lectura correctas; capturas revisadas. El estudiante lo aprobó. |
| 2026-10-24 | 06 | Pedido del estudiante: repositorio público en GitHub. Contraseña movida a `Conexion.config` (ignorado); `git grep` sin coincidencias; repositorio creado y subido; clon de prueba compila. |
| 2026-10-30 | 08 (Panel; solicitado como Doc. 07) | Revisión de nombres, eventos, destinos y referencias; MSBuild correcto (0 advertencias/errores con salida `obj\PanelBuild`). Navegación manual y cierre pendientes. |
| 2026-10-30 | Guía 06 del equipo (Login y roles; local skill 07) | Integrante 1 completó la revisión estática. Reconstrucción correcta y consulta SQL de solo lectura ejecutada con credenciales ficticias; validación con cuenta real e integración manual pendientes. Guías 07 (Panel) y 08 (Dashboard) quedan pendientes para sus responsables. |
| 2026-10-30 | 11 | Release compilado según confirmación del usuario; se identificaron y copiaron siete archivos a una carpeta externa; el usuario reportó que Login, Panel, Dashboard, CRUD y conexión funcionaron en otro equipo. Archivo `.bak` localizado en `C:\Respaldos`; no se ejecutó el script ni se probó restauración. README actualizado y cambios documentales enviados a `origin`. En la revisión inicial faltaban capturas; se añadieron posteriormente. Documento permanece ACTIVO. |
| 2026-10-30 | 11 | Se revisaron las capturas `Evidencia_Login.png`, `Evidencia_Panel.png`, `Evidencia_Dashboard.png` y cuatro evidencias de Tipos de prenda. La captura de Clientes muestra datos de contacto y queda excluida de GitHub hasta anonimizarla. Documento permanece ACTIVO. |
| 2026-10-30 | 01–04, 07 | Por autorización del estudiante se cerraron 01–04 con los registros de inspección, conexión y pruebas CRUD disponibles. El usuario confirmó Login válido; 07 se mantiene COMPLETADO. 05 sigue pendiente por falta de evidencia de CRUD maestro-detalle; 06 por colaborador no agregado; 08 por rutas de navegación no probadas individualmente; 09 por requerir confirmación de los tres integrantes. |
| 2026-10-30 | 05, 06, 08, 09 | El estudiante confirmó y autorizó el cierre definitivo. Se actualizaron a COMPLETADO según su confirmación; no se ejecutaron pruebas nuevas ni se recibieron confirmaciones individuales de los demás integrantes en esta conversación. |
| 2026-10-30 | — | Pedido del estudiante (cpradoc2): unión del fork del compañero sobre su proyecto, paleta aplicada a Roles/menú/Usuarios, barra de sesión única, menú sin huecos, maximizar/restaurar centrado y corrección al seleccionar usuarios sin rol. `02_Login_Roles.sql` ejecutado en su base. Compila; pruebas automáticas de login, menú por rol, roles, maximizar y regresión de los 8 módulos correctas; base intacta. Prueba F5 del estudiante pendiente. Estados sin cambios. |
