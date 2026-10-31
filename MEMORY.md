# MEMORY.md — Hechos comprobados

Registro del proyecto **Pfinal_labanderia**. Cada entrada indica la fecha y cómo se comprobó.
Lo que no está comprobado no se anota aquí (va a "Decisiones pendientes").

## 2026-10-15 · Documento 01 — Prueba de CMD

Comprobado ejecutando `cmd.exe /c`:

- CMD disponible: `Microsoft Windows [Versión 10.0.19045.6466]`,
  `COMSPEC=C:\Windows\system32\cmd.exe`.
- Se creó `PruebaCodex.txt` con `PRUEBA`, se leyó con `type` y se eliminó con `del`. Sin errores.
- Al lanzar CMD desde PowerShell, poner el comando completo entre comillas:
  `cmd.exe /c "a && b"`. Sin comillas, PowerShell rechazó el `&&`; ese intento no ejecutó nada.

## 2026-10-15 · Documento 01 — Inspección inicial

Comprobado con `dir`, `type` y `findstr` (CMD) y leyendo los archivos:

- Solución `Pfinal_labanderia.slnx` en la raíz con 4 proyectos: `Datos`, `Entidad`, `Negocio`,
  `Pfinal_labanderia` (Presentación).
- Los 4 proyectos usan `TargetFrameworkVersion v4.8`.
- `Entidad`, `Datos` y `Negocio` solo contienen `Properties\AssemblyInfo.cs` (sin clases).
- Formularios de Presentación: `logoing`, `precentacion`, `clientes`, `usuarios`, `Tarifas`,
  `Recepciones`. `Program.cs` inicia con `logoing`.
- `Form1.resx` existe en la carpeta pero no está incluido en el `.csproj`.
- Referencias: Presentación → Datos, Entidad, Negocio · Negocio → Datos, Entidad · Datos → Entidad.
- Paquetes: Datos → `System.Data.SqlClient 4.9.1`, `System.Configuration.ConfigurationManager 10.0.12`;
  Presentación → `FontAwesome.Sharp 6.6.0`.
- `App.config`: cadena `ConexionSQLServer` → servidor `localhost,1433`, base `Db_ParqueoAutosUmg`,
  usuario `sa`, contraseña en texto plano. **Conexión no probada.**
- No es repositorio Git (no existe `.git`). Hay carpetas `.vs` en la raíz y en `Datos`, `Entidad`, `Negocio`.

## 2026-10-15 · Documento 01 — Paso 1: estructura documental

Comprobado con `dir /a /b`, `tree /f` y `dir /ad` (CMD):

- Creados en la raíz: `README.md`, `AGENTS.md`, `MEMORY.md`, `ESTADO_DOCUMENTOS.md`, `.gitignore`.
- `skills\`: 11 archivos (`01_Inspeccion_Inicial.md` … `11_Publicacion.md`), sin subcarpetas.
  Skills 01 y 02 completos; 03 a 11 marcados PENDIENTE.
- `database\`: `CONEXION_DATABASE.md` y `01_Backup_Base_Datos.sql` (NO EJECUTADO), sin subcarpetas.
- `docs\`: `COMANDOS_RAPIDOS.md`, sin subcarpetas.
- `src\`: vacía.
- La solución y los proyectos existentes no se modificaron.

## 2026-10-15 · Documento 01 — Paso 2: solución movida a `src\Pfinal_labanderia\`

El estudiante confirmó el árbol, el nombre del sistema (`Pfinal_labanderia`) y autorizó mover la solución.
Comprobado con `tasklist`, `move`, `dir` y un script `.cmd` con `if exist` (CMD):

- Antes de mover, Visual Studio estaba abierto; el estudiante lo cerró y `tasklist` lo confirmó.
- Se movieron a `src\Pfinal_labanderia\`: `Pfinal_labanderia.slnx`, `Datos`, `Entidad`, `Negocio`,
  `Pfinal_labanderia`, `packages` y `.vs` (sigue oculta). No se cambió el contenido de ningún archivo.
- `move` de CMD no encuentra carpetas ocultas: para `.vs` se usó `attrib -h`, `move` y `attrib +h`.
- Existen todas las rutas del `.slnx`, las `ProjectReference` y los `HintPath` de NuGet.
- El `HintPath` relativo a `System.dll` de `Pfinal_labanderia.csproj` ya no existe desde la nueva
  ubicación. `System.dll` sí existe en
  `C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\`.
- La raíz quedó con: `.gitignore`, `AGENTS.md`, `ESTADO_DOCUMENTOS.md`, `MEMORY.md`, `README.md`,
  `skills\`, `database\`, `docs\`, `src\`.

## 2026-10-15 · Cambio puntual pedido por el estudiante: botón Usuarios

Pedido explícito del estudiante (corresponde al tema del documento 08, que sigue BLOQUEADO):

- En `precentacion` el botón con texto "Usuarios" se llama `iconButton8` (no existe `btn_usuarios`).
  `panelPrincipal` es un `Panel` con `Dock = Fill`.
- Se editó solo `precentacion.cs`: método `AbrirFormularioEnPanel(Form)` y evento `btnUsuarios_Click`,
  conectado en el constructor. El asistente no modificó ningún `Designer.cs`.
- Después, el estudiante renombró `iconButton8` a `bnt_usuarios` en Visual Studio. El diseñador
  actualizó `precentacion.Designer.cs`, pero no `precentacion.cs`. Se corrigió a mano a
  `bnt_usuarios.Click += btnUsuarios_Click;` y la solución volvió a compilar (código de salida 0).
- **El estudiante probó en ejecución que el botón Usuarios abre `usuarios` en `panelPrincipal`: funciona.**

## 2026-10-15 · Botones Clientes, Recepciones y Tarifas

Pedido explícito del estudiante, con los nombres de botón que él indicó:

- En `precentacion.Designer.cs` el asistente renombró `iconButton1` → `btn_clientes`,
  `iconButton6` → `bnt_recepcion` e `iconButton10` → `btn_tarifas`. Solo cambió el nombre:
  ninguna propiedad de diseño. `iconButton11` e `iconButton12` quedaron intactos (comprobado con búsqueda).
- Se conectaron sus `Click` en el diseñador, como lo hace Visual Studio:
  `btn_clientes_Click` → `clientes`, `bnt_recepcion_Click` → `Recepciones`, `btn_tarifas_Click` → `Tarifas`.
- El estudiante había hecho doble clic en Usuarios y Visual Studio creó `bnt_usuarios_Click` vacío,
  conectado en el diseñador. Se pasó ahí la lógica y se eliminaron la conexión del constructor
  y `btnUsuarios_Click`. Ahora los 4 botones siguen el mismo patrón.
- La solución compila (MSBuild, código de salida 0).
- **Prueba en ejecución de Clientes, Recepciones y Tarifas: PENDIENTE** (la hace el estudiante).
- Requisito del estudiante: al pulsar otro botón, el formulario anterior debe desaparecer.
  Se agregó `CerrarFormularioActivo()`: quita el formulario del panel, lo cierra y lo libera (`Dispose`).
  Compila (MSBuild, código de salida 0). **Prueba en ejecución: PENDIENTE.**
- `logoing` valida usuario y contraseña fijos en el código (tema del documento 07).

## 2026-10-15 · Compilación desde la nueva ubicación

Comprobado con MSBuild 18.10.1 (Visual Studio 18 Community), lanzado desde un `.cmd`:

- `Pfinal_labanderia.slnx` en Debug compila los 4 proyectos. Código de salida 0, ya con el cambio
  del botón Usuarios.
- El `HintPath` roto de `System.dll` no impide compilar.
- **Prueba en ejecución del botón Usuarios: PENDIENTE** (debe hacerla el estudiante).

## 2026-10-16 · Prueba de acceso a SQL Server (pedido explícito del estudiante)

Solo lectura; no se modificó la base de datos ni `App.config`. Comprobado con CMD:

- Servicio `MSSQLSERVER` en `RUNNING`, pero el puerto 1433 lo atienden Docker
  (`com.docker.backend.exe`) y WSL (`wslrelay.exe`).
- `sqlcmd` (ODBC 18) conecta a `localhost,1433` con `sa` y la base `BD_LavanderiaGT`.
  Se usó `-C` (confiar en el certificado); no se probó sin esa opción. Servidor `sql_prog2a`,
  SQL Server 16.0.4255.1 Developer.
- `BD_LavanderiaGT` tiene 11 tablas `dbo.Tbl_*` con 5 filas cada una: Clientes, DetalleRecepciones,
  Entregas, EstadosProceso, Procesos, Recepciones, Servicios, Sucursales, Tarifas, TiposPrenda, Usuarios.
  Coinciden con los 11 botones del menú de `precentacion`.
- `App.config` sigue apuntando a `Db_ParqueoAutosUmg`, que también existe en ese servidor.

## 2026-10-16 · CRUD de Clientes (pedido explícito del estudiante)

Corresponde a los documentos 02 y 03, que seguían BLOQUEADOS; se hizo por pedido directo.

- `Tbl_Clientes` (consultado con `sqlcmd`): `IdCliente` int IDENTITY (PK), `NombreCompleto` varchar(150)
  NOT NULL, `Telefono` varchar(20), `Correo` varchar(100), `Direccion` varchar(250),
  `FechaRegistro` datetime DEFAULT getdate(), `Estado` bit DEFAULT 1.
  `Tbl_Recepciones.IdCliente` → FK sin cascada; los 5 clientes de ejemplo tienen 1 recepción cada uno.
- `App.config`: solo se cambió `Database=` a `BD_LavanderiaGT`.
- Archivos nuevos (registrados en sus `.csproj`): `Entidad\Cliente.cs`, `Datos\Conexion.cs`,
  `Datos\DCliente.cs` (SQL parametrizado), `Negocio\NCliente.cs` (validaciones).
- `clientes.cs`: carga la tabla al abrir; doble clic en una fila → pestaña "Gestionar";
  Nuevo, Cancelar, Guardar (INSERT), Editar (UPDATE) y Eliminar (DELETE con confirmación).
- `clientes.Designer.cs`: solo se agregaron líneas de eventos (`Click`, `CellDoubleClick`, `Load`).
- Nombres reales en el formulario: la tabla es `dataGridViewUsuarios`; los botones son `btnNuevo_clientes`,
  `btnCancelar__clientes`, `btnGuardar__clientes`, `btnEditar__clientes` y `btnEliminar__clientes`.
  Los TextBox conservan nombres de otro formulario (por ejemplo, `textServicio` = Nombre completo).
- Comprobado: la solución compila (MSBuild, código de salida 0). Un programa de prueba de solo lectura
  que usa las DLL compiladas leyó los 5 clientes mediante `NCliente.Listar()`.
- **Guardar, Editar y Eliminar dentro de la ventana: PENDIENTE de prueba** (modifican la base; las prueba el estudiante).

## 2026-10-17 · Documento 02: clase de conexión y prueba inicial

Instrucción del estudiante: solo la conexión y su prueba; sin CRUD; la Presentación sin conexión directa.

- Estado encontrado: el estudiante devolvió `clientes.cs` y `clientes.Designer.cs` a su versión original
  (sin CRUD). Siguen en las capas, compilando y sin uso: `Entidad\Cliente.cs`, `Datos\DCliente.cs`,
  `Negocio\NCliente.cs`. No se tocaron.
- `Datos\Conexion.cs`: se agregó `ProbarConexion(out string mensaje)` con `try-catch` por tipo de error
  (`ConfigurationErrorsException`, `ArgumentException`, `SqlException` traducida por número, `Exception`).
- Nuevo `Negocio\NConexion.cs` (registrado en `Negocio.csproj`): llama a `Datos.Conexion.ProbarConexion`.
- `Program.cs`: antes de abrir `logoing` llama a `NConexion.ProbarConexion` y muestra el resultado en un `MessageBox`.
- Búsqueda en los `.cs` de Presentación: no hay `SqlConnection`, `SqlClient`, `ConfigurationManager` ni `Datos.`.
  Nota: `Pfinal_labanderia.csproj` todavía tiene una referencia de proyecto a `Datos`, sin uso en el código.
- Compila (MSBuild, código de salida 0). Programa de prueba con las DLL compiladas: con el `App.config` real conecta
  a `localhost,1433` / `BD_LavanderiaGT`. Con copias alteradas da mensajes claros para 4060, 18456, 1225 y la cadena ausente.
- Falla corregida: el error 1225 (puerto que rechaza la conexión) caía en el mensaje genérico; se agregó a los errores de red
  y se volvió a probar.
- **Prueba con F5 dentro de Visual Studio: PENDIENTE** (la hace el estudiante).

## 2026-10-17 · Clientes: solo CONSULTAR + paginación (autorizado por el estudiante)

- El estudiante renombró la tabla de `clientes` a `dataGridViewclientes` (comprobado en `clientes.Designer.cs`).
  No se modificó ningún `Designer.cs`.
- `Datos\DCliente.cs` y `Negocio\NCliente.cs` quedaron solo con `Consultar()`. Se quitaron Insertar, Actualizar
  y Eliminar, que no tenían uso. `Entidad\Cliente.cs` no cambió.
- `clientes.cs`: el evento `Load` se conecta en el constructor. La paginación (10 por página) se hace solo en
  Presentación con `GetRange`. Los botones Anterior y Siguiente y el indicador "Página X de Y" se crean por código
  en la pestaña Consulta. `labelRegistros` muestra el total de todos los registros y solo cambia al consultar.
- Compila (MSBuild, código de salida 0). Prueba automática con el `.exe` compilado y el formulario invisible:
  con la base real muestra 5 filas, "Registros: 5", "Página 1 de 1" y los dos botones inactivos.
  Con 23 clientes en memoria (sin escribir en la base) pagina 10/10/3, y el total sigue en 23 en todas las páginas.
- **Prueba del estudiante en Visual Studio (F5): PENDIENTE.**

## 2026-10-23 · Rediseño visual del login (autorizado por el estudiante)

Basado en una imagen de referencia. Decisiones del estudiante: omitir los elementos que necesitan código
(Recordarme, ¿Olvidaste…?, ver contraseña, idioma, botones de rol) y conservar la fuente OCR A Extended.

- Solo se modificó `logoing.Designer.cs`; `logoing.cs` no se tocó. Se conservan `textUser`, `textContraseña`,
  `btnIngresar`, `pictureBox1`, `label1`, `label2` y `label8` con sus eventos.
- Diseño: panel de marca a la izquierda (logo en modo Zoom, lema y 3 características con iconos FontAwesome)
  y tarjeta blanca de inicio de sesión a la derecha. Ventana fija de 1100×680, `AcceptButton = btnIngresar`
  y cajas de texto de una sola línea.
- Comprobado: compila (MSBuild, código de salida 0). Una captura generada desde el `.exe` compilado muestra que
  ningún control se sale de su contenedor y que el foco inicial está en `textUser`.
- **Prueba del estudiante en Visual Studio: PENDIENTE.**

## 2026-10-23 · Pantalla de Inicio (dashboard) y botón `bnt_inicio`

Decisiones del estudiante: diseño con datos reales; formulario nuevo `inicio` (excepción a la regla de no
crear formularios); en el menú solo agregar `bnt_inicio`, sin cambiar su estilo.

- Antes de empezar, el contenedor `sql_prog2a` estaba detenido: se había cerrado con código 1 y el registro
  mostraba un volcado de error de SQL Server. El estudiante lo volvió a encender.
- Esquema leído con `sqlcmd` (solo lectura): `Tbl_Recepciones` (IdSucursal, FechaRecepcion, Total, Estado),
  `Tbl_Entregas` (FechaEntrega), `Tbl_Procesos` → `Tbl_EstadosProceso`,
  `Tbl_DetalleRecepciones` → `Tbl_Tarifas` → `Tbl_Servicios`.
- Archivos nuevos: `Entidad\ResumenInicio.cs`, `Entidad\DatoGrafica.cs`, `Datos\DInicio.cs` (COUNT/SUM/GROUP BY,
  fechas como parámetros), `Negocio\NInicio.cs` (mes actual; últimos 6 meses con ceros), `inicio.cs` e
  `inicio.Designer.cs` (5 tarjetas, 4 gráficas Chart, fecha y hora). Se agregó la referencia
  `System.Windows.Forms.DataVisualization`.
- `precentacion.Designer.cs`: se agregó `bnt_inicio` (icono House, mismo estilo) como primer botón y los demás
  bajaron 46 px. `precentacion.cs`: el `Load` abre `inicio`, y `bnt_inicio_Click` lo vuelve a abrir.
- Comprobado: compila (MSBuild, código de salida 0). Prueba con el `.exe` y la base real: al entrar se muestra
  `inicio` con 5 / 5 / 5 / Q 355.00 / 5 y las 4 gráficas con datos; ningún control se sale de su contenedor;
  Clientes → `bnt_inicio` vuelve a mostrar `inicio` con un solo formulario en el panel.
- **Prueba del estudiante en Visual Studio: PENDIENTE.**

## 2026-10-23 · CONSULTAR en Recepciones, Tarifas y Usuarios (pedido del estudiante)

Mismo patrón aprobado para Clientes: solo CONSULTAR, paginación de 10 en Presentación, total en
`labelRegistros` y ningún `Designer.cs` modificado.

- Archivos nuevos: `Entidad\Recepcion.cs`, `Tarifa.cs`, `Usuario.cs`; `Datos\DRecepcion.cs`, `DTarifa.cs`,
  `DUsuario.cs` (SELECT con JOIN para mostrar nombres); `Negocio\NRecepcion.cs`, `NTarifa.cs`, `NUsuario.cs`.
- Modificados: `Recepciones.cs`, `Tarifas.cs` y `usuarios.cs`. En los tres la tabla se llama `dataGridViewUsuarios`,
  y se conservó ese nombre.
- **Usuarios: la columna `Clave` (contraseña) no se consulta ni existe en la entidad `Usuario`.**
- Las columnas de llaves foráneas (IdCliente, IdSucursal, IdTipoPrenda, IdServicio) se ocultan en la tabla.
- Comprobado: compila (MSBuild, código de salida 0). `sqlcmd` cuenta 5 filas en cada tabla. La prueba automática
  con el `.exe` y la base real muestra 5 filas en cada formulario, "Registros: 5", "Página 1 de 1",
  encabezados claros y ninguna columna de contraseña.
- **Prueba del estudiante en Visual Studio: PENDIENTE.**

## 2026-10-23 · Nuevo / Cancelar / Guardar / Editar / Eliminar en Clientes, Recepciones, Tarifas y Usuarios

Decisiones del estudiante:
- `chkSeleccionar` muestra una **columna de casillas**; al marcar una fila, el registro se carga en "Gestionar".
- Las claves se guardan **cifradas con SHA-256**.
- El Estado de Recepciones es **automático**: las nuevas quedan 'Recibido' y al editar no cambia.
- Se autorizaron **pruebas con registros de prueba**.

- Datos: `DCliente`, `DRecepcion`, `DTarifa` y `DUsuario` con INSERT/UPDATE/DELETE parametrizados. El error 547
  (llave foránea) se traduce a mensajes claros. `DUsuario.ExisteNombreUsuario` sirve para no repetir nombres
  (la tabla no tiene UNIQUE).
- Negocio: validaciones en `NCliente`, `NRecepcion`, `NTarifa` y `NUsuario`.
  `NUsuario.CifrarClave` (SHA-256, 64 hex) queda pública para el Login (documento 07).
- Presentación: nuevo `Formato.cs`, que convierte texto a enteros, decimales y fechas `dd/MM/aaaa [HH:mm]`.
  Los 4 formularios tienen eventos conectados por código (ningún `Designer.cs` modificado). En Recepciones,
  Activo/Inactivo están desactivados. En Usuarios, la clave va con `*`, no se carga al seleccionar, y vacía al editar
  significa que se conserva la actual.
- Hechos de la base: SQL Server en Docker usa hora UTC (6 h adelante). Por eso Negocio pone las fechas "de hoy" con la
  hora local. Las 5 claves existentes están en texto plano (8 caracteres) y no se modificaron. Los nuevos códigos
  automáticos saltaron a 1002/1003; la causa probable es la caché de IDENTITY de SQL Server después del cierre
  inesperado del contenedor.
- Comprobado: compila (MSBuild, código de salida 0). Prueba automática: en las 4 tablas se crearon, editaron y
  eliminaron registros de prueba. Las validaciones y los mensajes de llave foránea funcionan (cliente 1 y recepción 1
  protegidos). La clave guardada es igual al SHA-256 de la clave escrita, y si se deja vacía se conserva.
  La selección con casillas funciona en los 4 formularios. Al final la base quedó con 5 registros por tabla y
  0 registros de prueba.
- **Prueba del estudiante en Visual Studio: PENDIENTE.**

## 2026-10-23 · BUSCAR en Clientes, Recepciones, Tarifas y Usuarios (pedido del estudiante)

- La búsqueda se hace en la base de datos con `LIKE @Patron` (parámetro) y `COLLATE Latin1_General_CI_AI`,
  así que no distingue mayúsculas ni tildes. El nuevo `Datos\Busqueda.cs` arma el patrón '%texto%' y escapa `%`, `_` y `[`.
- Campos buscados: Clientes → nombre y teléfono; Recepciones → cliente y sucursal; Tarifas → tipo de prenda y servicio;
  Usuarios → nombre de usuario y nombre completo (sin leer la clave).
- Cada D* tiene `Buscar(texto)`, que reutiliza la consulta (`ConsultarConFiltro`). Cada N* tiene `Buscar`
  (texto vacío = todos).
- Presentación: Buscar y Enter buscan; Limpiar borra la caja y muestra todo. Si no hay resultados aparece un aviso.
  El resultado usa la paginación y "Registros: N" cuenta lo encontrado. Después de guardar, editar o eliminar
  se repite la búsqueda escrita (`RefrescarTabla`). En Clientes, Limpiar usa el `btnLimpiar_Click` que ya estaba
  conectado en el diseño. Ningún `Designer.cs` modificado.
- Comprobado: compila (MSBuild, código de salida 0). Prueba de solo lectura: "mendez" encuentra "Carlos Méndez",
  "ANDREA" encuentra la recepción de Andrea, "lavanderia" sin tilde encuentra las 5; `%` y `a_b` dan 0 sin errores;
  Buscar, Limpiar y Enter funcionan en los 4 formularios.
- **Prueba del estudiante en Visual Studio: PENDIENTE.**

## 2026-10-24 · Integración del proyecto del compañero (`C:\Users\rephot\Documents\LavanderiasMultinegocio`)

Revisado solo leyendo; su proyecto no se modificó. Usa la misma base `BD_LavanderiaGT`, la misma cadena
`ConexionSQLServer`, .NET Framework 4.8 y 4 capas. Tiene 4 módulos: Entregas, Prendas (`Tbl_TiposPrenda`),
Procesos y Servicios. Su menú principal estaba vacío.

Decisiones del estudiante:
- Rehacer los 4 módulos con la estructura de este proyecto.
- Usar listas desplegables y calendarios en los formularios nuevos.
- No cambiar los formularios actuales.
- Probar con registros de prueba.

- Entidad: `Entrega`, `Proceso`, `TipoPrenda`, `Servicio`, `OpcionLista` (opción de ComboBox).
- Datos: `DEntrega`, `DProceso`, `DTipoPrenda`, `DServicio`; además `DRecepcion.ListarOpciones()`.
- Negocio: `NEntrega`, `NProceso`, `NTipoPrenda`, `NServicio` y `NRecepcion.ListarOpciones()`. Llevan las reglas del
  compañero: una sola entrega por recepción, y la fecha de fin no puede ser anterior al inicio.
- Presentación: formularios `Entregas`, `Procesos`, `TiposPrenda` y `Servicios` con el diseño de este proyecto
  (1044×616, OCR A, pestañas Consulta/Gestionar, paginación de 10, búsqueda, `chkSeleccionar`). Los eventos y
  la paginación están en sus `Designer.cs`, que son nuevos. Los botones del menú `iconButton3`, `iconButton5`,
  `iconButton9` e `iconButton12` se conectan en `precentacion.cs`.
- Largos comprobados con `sqlcmd`: NombreRecibe 150; Observaciones 500 (Entregas y Procesos); Nombre 100;
  Descripción 250. Las 5 recepciones ya tienen entrega. Los 5 tipos de prenda y los servicios 1, 2 y 4 se usan en Tarifas.
- Corrección en clientes, Recepciones, Tarifas y usuarios: con la pantalla al 100 % (96 DPI), los botones de paginación
  creados por código quedaban fuera de la vista (coordenadas sin escalar; comprobado midiendo `Bounds`). Ahora se
  ubican en `Load` a partir del botón Exportar y de la tabla (`UbicarControlesPaginacion`).
- A 96 DPI, los textos de algunos botones del diseño original se ven cortados ("Limpia", "Imprim") en TODOS los
  formularios. Es un problema del diseño original con esa escala; no se cambió.
- Comprobado: compila (MSBuild 18.11.1, código de salida 0). La prueba con registros de prueba salió correcta en los
  4 módulos (CRUD, reglas, llaves foráneas protegidas, búsquedas, listas desplegables, selección, menú). Las pruebas de
  búsqueda y paginación de los formularios anteriores siguen correctas. Al final la base quedó con 5 registros por
  tabla y 0 registros de prueba.
- **Prueba del estudiante en Visual Studio: PENDIENTE.**

## 2026-10-24 · Rediseño visual de todos los formularios (autorizado por el estudiante)

El estudiante pidió que todo el proyecto se parezca a la imagen de referencia (menú azul marino, fondo claro, tarjetas
blancas, acentos azules) sin cambiar la lógica. Eligió la fuente Segoe UI, hacerlo todo de una vez y cambiar en el
código solo las líneas de estilo de la paginación.

- Respaldo de los 11 `Designer.cs` y de `clientes.cs`, `Recepciones.cs`, `Tarifas.cs` y `usuarios.cs` antes de
  cambiar nada (carpeta temporal de la sesión, fuera del proyecto).
- `Designer.cs` cambiados: `precentacion`, `clientes`, `Recepciones`, `Tarifas`, `usuarios`, `Entregas`, `Procesos`,
  `TiposPrenda`, `Servicios` (colores, fuentes, iconos y textos con tilde) y `logoing` e `inicio` (solo la fuente
  OCR A → Segoe UI; en `inicio` además se subieron 6 px los textos de las tarjetas).
- Menú: fondo azul marino, botones blancos alineados a la izquierda con iconos nuevos y una etiqueta nueva
  `labelMarca` ("Lavanderías GT") debajo del logo. Es el único control nuevo.
- Formularios CRUD: fondo claro, pestañas blancas, botón principal azul, Eliminar rojo, los demás blancos con borde,
  tabla sin bordes con filas alternas, títulos azul marino de 16 pt subidos para que no los tape la pestaña.
- En `clientes.cs`, `Recepciones.cs`, `Tarifas.cs` y `usuarios.cs` solo cambiaron las líneas de estilo de los
  botones de paginación y de `labelPagina` (9 pt para que "Página 1 de 1" quepa en una línea).
- Comprobado:
  - compila (MSBuild, código de salida 0);
  - las líneas de eventos (`+= new`) de los 11 `Designer.cs` son idénticas al respaldo;
  - los nombres de controles (`.Name`) son idénticos al respaldo, salvo el nuevo `labelMarca`;
  - las pruebas de solo lectura de consulta/paginación y de búsqueda dieron 0 fallas;
  - la prueba del menú dio "TODO CORRECTO" (abre Inicio, cambia a Clientes y vuelve a Inicio);
  - capturas de los 8 formularios CRUD (Consulta y Gestionar), del menú con Inicio y del login: títulos y
    "Página 1 de 1" completos, y "Limpiar"/"Imprimir" ya no se ven cortados.
- **Prueba del estudiante en Visual Studio: PENDIENTE.** (El estudiante revisó el resultado y dijo "me gusta, déjalo así".)

## 2026-10-24 · Documento 06: repositorio público en GitHub (pedido del estudiante)

- Repositorio: https://github.com/cpradoc2/Lavanderia-GT-final_progra2. GitHub no admite espacios: el nombre pedido
  "Lavanderia GT final_progra2" quedó con guiones. Comprobado con la API de GitHub: `"visibility": "public"`, rama `main`.
- Commits a nombre de `cris prado <cpradoc2@miumg.edu.gt>` (identidad global de Git de esta PC, elegida por el
  estudiante). Cuenta de GitHub con sesión iniciada: `cpradoc2`.
- Contraseña fuera del repositorio:
  - `App.config` usa `<connectionStrings configSource="Conexion.config" />`.
  - `Conexion.config` tiene la cadena real, está en `.gitignore` y el `.csproj` lo copia junto al `.exe`.
  - `Conexion.config.ejemplo` es la plantilla sin contraseña y sí se sube.
  - Los mensajes de error de `Datos\Conexion.cs` ahora nombran `Conexion.config`.
- Comprobado:
  - compila (código de salida 0);
  - la prueba de conexión dio 6 de 6 casos esperados, incluido "falta Conexion.config", que muestra un mensaje claro;
  - `git grep --cached` no encontró la contraseña real; `git check-ignore` confirma que `Conexion.config` se ignora;
  - `Conexion.config` no existe en GitHub (respuesta 404);
  - un clon nuevo en una ruta corta restauró NuGet y compiló. Sin `Conexion.config` el error es MSB3030 (no encuentra
    el archivo); después de copiar la plantilla compila con código 0.
- En una ruta muy larga, la restauración de NuGet falla por el límite de 260 caracteres de Windows. El README
  recomienda clonar en una carpeta corta.
- GitHub CLI 2.103.2 está en `%LOCALAPPDATA%\Programs\GitHubCLI\bin\gh.exe` (zip oficial, SHA-256 comprobado).
  `winget` no está instalado en esta PC.
- El `push` usó la sesión de GitHub CLI solo para ese comando (`git -c credential.helper=...`), sin cambiar la
  configuración global de Git.
- Agregar al compañero como colaborador para que pueda hacer `push` — PENDIENTE.

## Decisiones pendientes

- ¿Retirar la prueba de `Program.cs` o dejar solo el aviso cuando falla? — PENDIENTE.
- Convertir a SHA-256 las 5 claves actuales en texto plano (documento 07, Login) — PENDIENTE.
- ~~Retirar la contraseña de `App.config` antes de Git~~ — RESUELTO el 2026-10-24 (`Conexion.config`).
- Cerrar el documento 01 y decidir el estado del 06 — PENDIENTE de autorización del estudiante.
- Asignar rol a los 4 usuarios que no tienen (mgarcia, jperez, asanchez, rhernandez) — PENDIENTE (2026-10-30).
- En Usuarios, "Nuevo" propone el primer rol de la lista (Administrador). Conviene decidir si debe quedar vacío
  para obligar a elegir — PENDIENTE (2026-10-30).
- El login acepta la clave en texto plano o en SHA-256 (transición del compañero). Al convertir las 5 claves a
  SHA-256 se podría aceptar solo SHA-256 — PENDIENTE.

## 2026-10-30 · Documento 07 — Panel de navegación

- Por autorización del estudiante, se inspeccionó y conservó la navegación existente en `precentacion.cs`:
  Inicio, Clientes, Recepciones, Tarifas, Usuarios, Entregas, Procesos, Tipos de prenda y Servicios ya tenían
  sus eventos `Click` conectados a formularios existentes.
- Se modificó `AbrirFormularioEnPanel(Form)` para que, si el formulario activo es del mismo tipo que la opción
  pulsada, se libere la instancia recién creada y se conserve la que ya está mostrada. La navegación sigue usando
  el `panelPrincipal` existente y `Show()`.
- Detalle de recepciones, Sucursales y Estados de proceso siguen sin destino: no se encontró un formulario de
  Presentación correspondiente y no se crearon pantallas.
- No se modificaron `precentacion.Designer.cs` ni otros archivos de interfaz. **Compilación y prueba manual:
  pendientes para el estudiante.**

## 2026-10-30 · Documento 07 — Destinos para las opciones restantes

- Por solicitud del estudiante, se conectaron en `precentacion.cs` los botones que antes no tenían destino:
  `iconButton2` abre el formulario existente `Recepciones`; `iconButton4` abre `Procesos`; `iconButton11` abre
  `inicio`, que ya presenta indicadores y una gráfica por sucursal.
- Se agregó el método `btnDetalleRecepciones_Click` y el método `btnSucursales_Click`; `iconButton4_Click`, ya
  conectado en el diseñador, ahora abre `Procesos`.
- No se crearon formularios ni se modificó el diseñador. El destino Sucursales es el resumen existente, no un
  formulario de mantenimiento de sucursales. **Compilación y pruebas manuales: pendientes.**

## 2026-10-30 · Panel de navegación — revisión y compilación

- Inspección estática de `precentacion.Designer.cs` y `precentacion.cs`: los 12 botones tienen texto y evento
  `Click`; sus destinos son `inicio`, `clientes`, `Recepciones`, `Tarifas`, `usuarios`, `Entregas`, `Procesos`,
  `TiposPrenda` y `Servicios`. Detalle de recepciones reutiliza `Recepciones`, Estados de proceso reutiliza
  `Procesos` y Sucursales abre `inicio` (resumen/gráfica por sucursal).
- El proyecto Presentación referencia Datos, Entidad y Negocio. En `precentacion.cs` no se encontraron accesos
  directos a SQL Server.
- Pruebas de navegación ya registradas antes de esta revisión: el estudiante confirmó Usuarios; una prueba
  automática recorrió Inicio → Clientes → Inicio; el registro de integración de Entregas, Procesos, TiposPrenda y
  Servicios incluye el menú. No se volvió a ejecutar ninguna prueba de interfaz en esta revisión.
- Pendientes de prueba manual de los botones: Recepciones, Tarifas, Detalle recepciones, Estados de proceso y
  Sucursales. También queda comprobar en la aplicación el cambio entre opciones y volver a pulsar la opción activa.
- Compilación de `Pfinal_labanderia.slnx` con MSBuild 17.14.40 y salida alternativa `obj\PanelBuild`: correcta,
  0 advertencias y 0 errores. La compilación con salida normal no pudo completar la copia de DLL porque estaban
  bloqueadas por otro proceso; la salida alternativa permitió completar la compilación.
- No se ejecutó la aplicación ni se probaron los 12 clics en ejecución. Quedan pendientes las pruebas manuales
  de navegación y la confirmación del estudiante para cerrar este punto.

## 2026-10-30 · Documento 09 — Primer indicador: clientes activos

- Se añadió a `Datos.DInicio` una consulta ADO.NET `COUNT(*)` sobre `dbo.Tbl_Clientes`, filtrada por `Estado = 1`,
  con `try-catch` para errores SQL.
- `Negocio.NInicio.ObtenerClientesActivos()` solicita ese valor a Datos.
- `Presentación.inicio` asigna el valor a la etiqueta existente `lblValorClientes`.
- `ObtenerResumen` conserva los otros cuatro indicadores; se retiró de su consulta el conteo de clientes para no
  consultar el mismo dato dos veces. No se modificó Entidad ni ningún `Designer.cs`.
- Compilación y prueba manual: pendientes; no se ejecutaron en esta tarea.

## 2026-10-30 · Documento 09 — Revisión del Dashboard

- Inspección estática: `DInicio.ObtenerClientesActivos()` cuenta filas de `dbo.Tbl_Clientes` con `Estado = 1`; usa ADO.NET, `ExecuteScalar` y captura `SqlException`. `NInicio` delega la consulta y `inicio.CargarDatos()` asigna el resultado a `lblValorClientes`.
- Se confirmó en el Designer existente el control `lblValorClientes` y los cuatro Charts nombrados en `ESTADO_DOCUMENTOS.md`; no se modificaron archivos `.Designer.cs`.
- `DInicio` contiene además consultas estáticas para el resto de los indicadores. No fueron ejecutadas contra SQL Server ni verificadas en la interfaz.
- Reconstrucción de `Pfinal_labanderia.slnx` con MSBuild 17.14.40 y salida alternativa `obj\\DashboardBuild`: correcta, 0 advertencias y 0 errores. La reconstrucción con salida `bin\\Debug` no pudo copiar DLL bloqueadas y terminó con 6 errores; la salida alternativa completó la compilación.
- Prueba de ejecución/F5 y comparación de valores con SQL Server: pendientes.

## 2026-10-30 · Doc. 06 (Login y roles; skill 07) · implementación autorizada

- Se consultó `BD_LavanderiaGT` usando el `Conexion.config` local y se aplicó `database/02_Login_Roles.sql`. Se verificaron los roles Administrador, Recepcionista y Operador, con 13, 4 y 3 accesos habilitados. `Tbl_Usuarios` quedó con `IdRol` nullable y FK a `Tbl_Roles`; no se modificó la columna ni los valores de claves existentes.
- Se asignó el primer usuario por `IdUsuario` al rol Administrador para facilitar la configuración. Quedaron 1 de 10 usuarios con rol; los demás pueden asignarse desde el CRUD de Usuarios.
- Entidad incorpora `Rol`, `Acceso`, `Usuario.IdRol` y `Usuario.NombreRol`. Datos incorpora `DLogin` (validación parametrizada y consulta de accesos), `DRol` (CRUD de roles y permisos) y JOIN de rol en `DUsuario`. Negocio incorpora `NLogin` y `NRol`; Login acepta el formato actual de la base y SHA-256, que el CRUD aplica al guardar cambios. No se migraron contraseñas.
- Presentación valida el Login contra Negocio, conserva el usuario y rol de la sesión, filtra opciones del menú con los permisos guardados, abre la primera opción autorizada si Inicio está deshabilitado y muestra usuario/rol en el panel y formularios. Usuarios permite elegir un rol. Se agregó el formulario CRUD `roles`, con casillas para configurar accesos.
- Comprobado: el script SQL se ejecutó; la consulta confirmó tres roles, permisos iniciales y la asignación descrita. Reconstrucción de `Pfinal_labanderia.slnx` con MSBuild 17.14.40 y salida `obj\\LoginRolesBuild`: correcta, 0 advertencias y 0 errores.
- No se probó el flujo de Login ni los formularios en ejecución. Prueba manual pendiente.

## 2026-10-30 · Revisión de Login · Guía 06 del equipo, Integrante 1

- Inspección estática confirmó el evento `btnIngresar_Click` en `logoing`, llamada a `NLogin`, validación SQL parametrizada en `DLogin`, filtro de usuario/rol activos y consulta de permisos. La sesión alimenta el menú y los encabezados agregados por código; no se modificaron formularios ni diseñadores en esta revisión.
- Reconstrucción de `Pfinal_labanderia.slnx` con MSBuild 17.14.40, configuración Debug y salida `obj\\LoginReviewBuild`: correcta, 0 advertencias y 0 errores.
- La consulta parametrizada equivalente a Login se ejecutó en modo de solo lectura con usuario y claves ficticios; devolvió 0 coincidencias y no modificó la base. La consulta de conteo confirmó 3 roles, 13 accesos y 39 asociaciones rol-acceso.
- Guía 06 de la asignación del equipo (Login, Integrante 1) quedó marcada completada en `ESTADO_DOCUMENTOS.md`. La numeración local conserva el Login como skill 07; la Guía 07 del equipo (Panel, skill local 08) y Guía 08 (Dashboard, skill local 09) quedan pendientes para sus responsables.
- No se ejecutó F5 ni se probó inicio de sesión válido, inválido o inactivo en la interfaz. Pruebas de integración manual pendientes.

## 2026-10-30 · Doc. 09 — Integración general (skill 10), revisión estática

- Se inspeccionaron README, AGENTS, MEMORY, ESTADO_DOCUMENTOS, `skills/10_Revision_Proyecto.md`, `database/CONEXION_DATABASE.md`, solución, cuatro proyectos, formularios `.cs`/`.Designer.cs`, referencias y eventos.
- Los documentos discrepan: el README de la raíz describe una cadena de librerías con PostgreSQL/Npgsql; el proyecto describe una lavandería con SQL Server. El `ESTADO_DOCUMENTOS.md` interno indica Documento 01 activo y Documento 10 bloqueado; el skill 10 indica pendiente/bloqueado, mientras que el usuario solicitó revisar como Doc. 09.
- Se consultó `BD_LavanderiaGT` en modo de solo lectura mediante la conexión local. La metadata confirmó 15 tablas, columnas, tipos, nulabilidad, PK, identidad y FK. Las PK usan `Id...`, no el prefijo `Codigo` indicado como convención general. Las cuatro tablas añadidas por Login/roles (`Tbl_Roles`, `Tbl_Accesos`, `Tbl_RolesAccesos` y FK `Tbl_Usuarios.IdRol`) están en la base real pero no en el script base `database/scripts/LavanderiasGT.sql`.
- Las capas y referencias corresponden a Entidad → Datos → Negocio → Presentación; Presentación referencia además Datos y Entidad, aunque no se detectó acceso SQL directo en formularios. Los eventos revisados en Designer tienen sus métodos encontrados en el código.
- Hay implementación CRUD por capas para Clientes, Recepciones, Tarifas, Usuarios, Entregas, Procesos, TiposPrenda, Servicios y Roles. No se encontró CRUD de Sucursales, DetalleRecepciones ni EstadosProceso. Los destinos de Panel para esos módulos reutilizan `inicio`, `Recepciones` o `Procesos`.
- Se verificó estáticamente Login → NLogin/DLogin, carga de rol y accesos; el Panel filtra opciones y el Dashboard `inicio` pide indicadores a NInicio. No se probó autenticación en UI ni ejecución del Dashboard.
- No se modificaron código ni diseños. No se ejecutaron build ni pruebas funcionales. El esquema SQL fue consultado en modo de solo lectura; resultados y límites registrados en la revisión del Doc. 09.

## 2026-10-30 · Doc. 09 — Paso 1: Login válido → Panel de navegación

- Revisión estática de logoing.btnIngresar_Click, NLogin, DLogin, SesionActual y precentacion: el flujo existente valida por Negocio, recupera accesos asociados al rol, muestra el Panel y oculta el Login. DLogin filtra usuarios y roles activos y usa parámetros SQL.
- precentacion.ConfigurarAccesos() oculta botones según los códigos de acceso guardados en SesionActual; el evento Load abre Dashboard automáticamente si el rol permite Inicio. No se cambió esta conexión ni se inició todavía la integración de destinos individuales.
- No se modificó código ni diseño. La reconstrucción de Pfinal_labanderia.slnx con MSBuild 17.14.40, Debug y salida alternativa obj\LoginPanelBuild terminó correctamente. La reconstrucción a salida habitual falló al copiar DLL bloqueadas por otro proceso. La ejecución con dotnet MSBuild no fue válida para este proyecto WinForms clásico y falló al procesar recursos.
- Prueba manual de cuenta válida → Panel y permisos visibles/ocultos: pendiente de confirmación del estudiante. No se continúa a otra conexión hasta recibirla.
## 2026-10-30 · Doc. 09 — Revisión estática de las cuatro capas

- Revisión estática de Entidad, Datos, Negocio y fuentes de Presentación, incluidos formularios y Designer. Presentación no contiene using Datos ni llamadas a Datos; no se encontró SqlConnection/SqlCommand en fuentes de formularios. El proyecto Presentación sí mantiene una referencia a Datos en el csproj.
- Los comandos SQL observados usan parámetros para valores variables; la concatenación encontrada compone fragmentos SQL fijos (cláusula de búsqueda o campo Clave opcional), no concatena el texto ingresado por el usuario.
- La metadata SQL consultada anteriormente confirma que Estado permite NULL en Tbl_Clientes, Tbl_Servicios, Tbl_TiposPrenda y Tbl_Tarifas. En las clases D correspondientes, el mapeo de Estado asigna false si el valor leído es DBNull; una fila con Estado NULL se interpreta como inactiva. No se verificó que haya filas actuales con ese valor.
- DRecepcion revisa DBNull para FechaRecepcion, FechaEntregaEstimada y Total antes de convertir; Entidad modela las fechas como Nullable y Total como decimal con valor predeterminado 0.
- database/CONEXION_DATABASE.md conserva el inventario antiguo de 11 tablas y afirma que la contraseña reside en App.config; App.config actual delega la cadena a Conexion.config. La revisión anterior consultó 15 tablas en BD_LavanderiaGT.
- No se modificó código ni diseño. No se ejecutaron compilación, consultas adicionales a SQL Server ni pruebas funcionales en esta revisión.
## 2026-10-30 · Doc. 09 — Criterio de cierre indicado por el grupo

- Instrucción recibida: marcar Documento 09 como COMPLETADO solo cuando los tres integrantes confirmen sus pruebas; no iniciar Documento 10 ni publicar el sistema antes de autorización expresa del grupo.
- En esta conversación no se han recibido las tres confirmaciones. Documento 09 permanece sin marcar como COMPLETADO. No se cambió ESTADO_DOCUMENTOS.md ni se inició el Documento 10 o publicación.
## 2026-10-30 · Revisión preparatoria de entrega final (skill local 11)

- Inspeccionados README, AGENTS, MEMORY, ESTADO_DOCUMENTOS, skills/11_Publicacion.md, database/CONEXION_DATABASE.md, solución, proyectos, configuración y scripts. La numeración no coincide: ESTADO_DOCUMENTOS tiene Documento 10 como revisión y Documento 11 como publicación; en esta solicitud el usuario llamó Documento 10 a Publicación.
- Los cuatro proyectos targetean .NET Framework v4.8. Presentación es OutputType WinExe y AnyCPU; Program.Main comprueba conexión por Negocio y luego inicia logoing. El archivo Pfinal_labanderia.slnx enumera cuatro proyectos y no declara proyecto de inicio.
- Referencias en el csproj de Presentación: Datos, Entidad y Negocio; Negocio referencia Datos y Entidad; Datos referencia Entidad.
- App.config carga connectionStrings desde Conexion.config. El csproj incluye Conexion.config y marca CopyToOutputDirectory=Always. El archivo local existe y está ignorado por Git. No se encontró Conexion.config.ejemplo en la carpeta del proyecto ni incluido en Git, aunque README indica copiarlo para crear la configuración real.
- El script database/scripts/LavanderiasGT.sql crea las 11 tablas base y no contiene INSERT. database/02_Login_Roles.sql agrega roles/accesos y solo asigna Administrador al menor IdUsuario si ya existe un usuario; no crea un usuario inicial. La revisión SQL anterior comprobó 15 tablas en BD_LavanderiaGT. Una base nueva requiere aprovisionar al menos una cuenta antes de probar Login.
- La entrega mínima propuesta es una carpeta con el ejecutable y sus DLL dependientes, el archivo .exe.config y Conexion.config suministrado/configurado fuera de Git, para copiar a un equipo Windows con .NET Framework 4.8 y acceso a un SQL Server ya disponible. No se creó artefacto ni se publicó. La confirmación grupal para continuar está pendiente.
## 2026-10-30 · Verificación de conexión para el equipo destino

- Lectura de Conexion.config sin mostrar valores secretos: Data Source localhost,1433; Initial Catalog BD_LavanderiaGT; autenticación SQL Server. El archivo está excluido por .gitignore.
- En una revisión previa, una consulta SQL de solo lectura abrió BD_LavanderiaGT con la configuración local. Esto comprueba el entorno de desarrollo, no un equipo destino distinto.
- No se identificó el equipo donde se ejecutará la aplicación ni su dirección de red. No se modificó configuración, no se registró contraseña y no se declara comprobada la conexión desde el equipo destino.
## 2026-10-30 · Comprobación de conexión con Conexion.config

- Se abrió una conexión de solo lectura desde este equipo utilizando la configuración local sin imprimir la cadena ni credenciales. SELECT @@SERVERNAME, DB_NAME() devolvió sql_prog2a y BD_LavanderiaGT.
- No se modificó el esquema ni datos. El resultado comprueba este equipo; no acredita conectividad desde otra computadora.
## 2026-10-30 · Documento 10 (fila 11) — Fase 0, revisión de requisitos

- Se leyeron los seis documentos solicitados: README.md, AGENTS.md, MEMORY.md, ESTADO_DOCUMENTOS.md, skills/11_Publicacion.md y database/CONEXION_DATABASE.md.
- Según ESTADO_DOCUMENTOS.md, la fila 07 está marcada COMPLETADO. Las filas 01–06, 08 y 09 no están COMPLETADO ni No aplica. La nota de la fila 07 dice que Login/F5 y pruebas de integración manual siguen pendientes.
- No se cambió ESTADO_DOCUMENTOS.md ni se cerró ninguna fila. Fase 1 no iniciada; se espera autorización del estudiante.
## 2026-10-30 · Documento 10, Fase 1 — plan solicitado

- El estudiante respondió «si» a la pregunta sobre preparar el plan de Fase 1. Se preparó un plan, pero todavía no se ejecutó la fase ni se modificó configuración. Se espera autorización explícita para iniciar.
## 2026-10-30 · Documento 10, Fase 1 (10.3) — revisión de configuración

- Con CMD se leyó App.config: usa configSource="Conexion.config". Datos.Conexion obtiene la cadena ConexionSQLServer de ConfigurationManager. El .csproj de Presentación incluye Conexion.config con CopyToOutputDirectory=Always.
- Una lectura programática allowlisted de Conexion.config (sin mostrar la contraseña) confirmó Data Source localhost,1433; Initial Catalog BD_LavanderiaGT; usuario SQL sa; Encrypt=False; TrustServerCertificate=True; Pooling=False; MultipleActiveResultSets=False. .gitignore confirma que Conexion.config se excluye.
- CMD confirmó que Conexion.config.ejemplo no existe junto al proyecto y git ls-files --error-unmatch confirmó que no está versionado. README indica copiar esa plantilla, por lo que la instrucción documental no concuerda con los archivos presentes. No se creó ni modificó plantilla debido a las restricciones del estudiante.
- La conexión local había sido confirmada previamente con una consulta SQL de solo lectura (sql_prog2a / BD_LavanderiaGT). No se probó desde otro equipo. Prueba de destino marcada LO HAGO YO y pendiente.
## 2026-10-30 · Documento 10, Fase 1 (10.3) — revisión de configuración

- Con CMD se leyó App.config: usa configSource="Conexion.config". Datos.Conexion obtiene la cadena ConexionSQLServer de ConfigurationManager. El .csproj de Presentación incluye Conexion.config con CopyToOutputDirectory=Always.
- Una lectura programática de solo campos permitidos de Conexion.config (sin mostrar la contraseña) confirmó Data Source localhost,1433; Initial Catalog BD_LavanderiaGT; usuario SQL sa; Encrypt=False; TrustServerCertificate=True; Pooling=False; MultipleActiveResultSets=False. .gitignore confirma que Conexion.config se excluye.
- CMD confirmó que Conexion.config.ejemplo no existe junto al proyecto y git ls-files --error-unmatch confirmó que no está versionado. README indica copiar esa plantilla; la instrucción no concuerda con los archivos presentes. No se creó ni modificó plantilla debido a las restricciones del estudiante.
- Program.cs llama NConexion.ProbarConexion y muestra un MessageBox con el resultado antes de abrir logoing. Una consulta SQL de solo lectura confirmó el acceso local (sql_prog2a / BD_LavanderiaGT). El usuario confirmó que la conexión desde el equipo de destino mostró el mensaje de éxito.

2026-10-30, Doc. 10, paso 10.4: El usuario confirmó la compilación Release sin errores. Se verificaron los artefactos en bin\Release y se copiaron siete archivos a C:\Entrega_LavanderiaGT\Release, con autorización. No se modificó la salida original.

2026-10-30, Doc. 10, paso 10.5: El usuario informó que funcionaron las pruebas de Login, Panel, Dashboard y CRUD en otro equipo, incluida la conexión. Resultado reportado por el usuario.

2026-10-30, Doc. 10, paso 10.6: El usuario informó que el .bak está en C:\Respaldos. CMD localizó C:\Respaldos\BD_LavanderiaGT.bak con fecha 30/10/2026 y tamaño 6,410,240 bytes. No se ejecutó el script de backup ni una restauración de prueba.

2026-10-30, Doc. 10, paso 10.6: Se añadió al README información de ejecución, archivos Release, conexión sin contraseña, respaldo, pruebas reportadas y responsabilidades nominales pendientes. docs contiene COMANDOS_RAPIDOS.md e HISTORIAL_PROMPTS.md; no se encontraron capturas de evidencia.

2026-10-30, Doc. 10, pasos 10.7–10.8: Git revisado en la rama main; origin apunta al fork FrozenCureBand y upstream a cpradoc2. Los cambios documentales se guardaron en el commit c9effc2 y se enviaron solo a origin; el estado local quedó limpio. ESTADO_DOCUMENTOS.md indica el documento 11 como ACTIVO, no COMPLETADO, porque las evidencias visuales faltan y los documentos 01–09 no están todos cerrados. No se probó restaurar el .bak.

2026-10-30, Doc. 10, paso 10.6: CMD encontró ocho capturas en docs. La revisión visual muestra Login, Panel, Dashboard y alta CRUD de Tipos de prenda. Evidencia_Clientes.png muestra nombres y datos de contacto, por lo que se excluye de publicación hasta anonimizarla. Las capturas no demuestran edición ni eliminación; esas pruebas fueron reportadas por el usuario.

2026-10-30 21:31, solicitud de cierre de documentos 01–09: el estudiante autorizó cerrar las filas que tuvieran respaldo. ESTADO_DOCUMENTOS marca 01–04 como COMPLETADO con base en árbol confirmado, conexión exitosa reportada en la aplicación destino y resultados CRUD previos más confirmación del usuario. 07 ya estaba COMPLETADO y ahora registra Login válido confirmado. 05 continúa pendiente porque no hay CRUD maestro-detalle verificado; 06 porque falta agregar al compañero; 08 porque faltan pruebas individuales de rutas de menú; 09 porque el criterio previo requiere confirmación de los tres integrantes y no se registraron las tres. Documento 11 sigue ACTIVO.

2026-10-30 21:34, cierre definitivo de 05, 06, 08 y 09: el estudiante confirmó y autorizó marcar los cuatro COMPLETADOS. Se actualizaron ESTADO_DOCUMENTOS.md y sus skills. Esta actualización registra la confirmación del estudiante; no se ejecutaron pruebas nuevas ni se verificó independientemente la incorporación del colaborador o las confirmaciones individuales del resto del grupo.

2026-10-30 21:35, Doc. 11, consulta del ejecutable: CMD confirmó que `C:\Entrega_LavanderiaGT\Release\Pfinal_labanderia.exe` existe. Es el ejecutable de la carpeta de entrega externa al repositorio.

2026-10-30, Doc. 11, README: se agregó el enlace al repositorio de entrega `FrozenCureBand/Lavanderia-GT-final_progra2` y se cambió la instrucción `git clone` para usar ese fork. Se conserva el remoto upstream sin cambios.

2026-10-30 21:59, revisión de entregables solicitados: se leyó `database/scripts/LavanderiasGT.sql` y se confirmó que crea el esquema de las 11 tablas base sin INSERT de datos. `database/02_Login_Roles.sql` agrega tablas y registros iniciales de roles/accesos, no datos de prueba para todas las tablas. `database/01_Backup_Base_Datos.sql` continúa como plantilla con valores pendientes. CMD encontró el `.bak` en `database\` local y en `C:\Respaldos`, y el ejecutable con sus seis archivos auxiliares en `C:\Entrega_LavanderiaGT\Release`. No se verificó restauración del `.bak`.

2026-10-30, aclaración del estudiante: CMD localizó `database\BD_LavanderiaGT.bak` en la copia del proyecto. Mide 6,410,240 bytes, fecha 30/10/2026, y `git check-ignore` confirma que Git lo excluye. También existe `C:\Respaldos\BD_LavanderiaGT.bak` con el mismo tamaño y fecha; no se comparó su contenido ni se probó restauración.

2026-10-30 22:04, consulta sobre completar los puntos 2 y 3: se propuso generar un script de esquema a partir de la base integrada, preparar un script separado de datos sintéticos para todas las tablas y validar ambos junto con el `.bak` en una base de prueba. No se generaron ni ejecutaron scripts en esta respuesta.

## 2026-10-30 · Unificación con el fork del compañero y maximizar/restaurar (pedido del estudiante)

El estudiante pidió: tomar su proyecto como principal, unir los cambios del fork
`FrozenCureBand/Lavanderia-GT-final_progra2`, aplicarles su paleta de colores, validar todo y permitir maximizar
y restaurar la ventana sin cambiar el diseño. Decisiones del estudiante: centrar el diseño al maximizar, ejecutar
`database\02_Login_Roles.sql` en su base y subir a su repositorio para que el compañero sincronice su fork.

- **Unión:** el fork partía del commit `1c60b04`, igual que este `main`. Se unió con `git merge --ff-only`
  (13 commits, sin conflictos). Remoto agregado: `companero`.
- **Estructura restaurada:** el fork había borrado `Conexion.config.ejemplo` y los comentarios de `Program.cs`, y
  cambiado `Conexion.config` a `Always`. Se restauraron la plantilla (desde `1c60b04`), los comentarios y
  `PreserveNewest`; se conservaron sus clases nuevas en los `.csproj`.
- **Base de datos:** se ejecutó `database\02_Login_Roles.sql` con `sqlcmd`. Resultado consultado: 3 roles
  (Administrador 13 accesos, Recepcionista 4, Operador 3), 13 accesos, 39 filas rol-acceso. Solo `jlopez`
  (IdUsuario 1) quedó con rol; los otros 4 usuarios no tienen rol y por eso no pueden iniciar sesión todavía.
- **Diseño unificado (sin tocar ningún `.Designer.cs`, comprobado con `git diff`):**
  - `roles.cs`: misma paleta, Segoe UI, tabla con el estilo de los CRUD, botones con icono (azul/rojo), tarjeta
    blanca y escala por DPI. La lógica de sus 5 métodos no cambió.
  - Menú: botón "Roles y accesos" con el mismo estilo que `bnt_usuarios` (copiado en código) e icono `UserShield`;
    los botones permitidos se reacomodan sin huecos (`ReacomodarMenu`).
  - Barra "Conectado | Rol": una sola vez, arriba del área de trabajo (antes aparecía dos veces y tapaba títulos).
  - Usuarios: campo "Rol" debajo de "Estado", ubicado en `Load` con los controles del diseñador como referencia.
- **Maximizar/restaurar:** al maximizar, `panelPrincipal` recibe un `Padding` igual por los lados y el formulario
  abierto conserva su tamaño original centrado; al restaurar el margen vuelve a 0.
- **Error corregido en `usuarios.cs`:** al seleccionar un usuario sin rol, `SelectedValue = null` lanzaba
  `ArgumentNullException`. Además, el primer `SelectedIndex = -1` se ignoraba y quedaba "Administrador"
  seleccionado (se habría asignado ese rol al editar). Ahora la lista queda sin selección.
- **Comprobado con pruebas automáticas** (formularios invisibles, usuario y rol de prueba creados y borrados):
  - compila (MSBuild 18.11.1, código 0);
  - login: correcto entra como Administrador; incorrecto, vacío e inexistente se rechazan;
  - menú Administrador: 13 botones sin huecos que abren su formulario; Recepcionista: 4 botones y sin "Roles";
  - una sola barra de sesión y ningún formulario queda debajo de ella;
  - maximizar/restaurar 3 vueltas y minimizar: el formulario mantiene 1055×691, queda centrado y vuelve igual;
  - CRUD de roles (crear, editar, nombre vacío rechazado, rol con usuarios protegido, eliminar);
  - pruebas anteriores repetidas: conexión 6/6, consulta/paginación, búsqueda, CRUD de los 8 módulos y selección.
    En la prueba CRUD de Usuarios ahora el usuario de prueba lleva rol, porque la regla del compañero lo exige.
  - la base quedó con 5 usuarios, 3 roles, 39 filas rol-acceso y 5 registros en clientes, recepciones y tarifas.
- **Prueba del estudiante en Visual Studio (F5): PENDIENTE.**
