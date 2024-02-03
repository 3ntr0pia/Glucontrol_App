# Glucontrol_App 📈

Glucontrol_App es una aplicación avanzada diseñada para facilitar la gestión y el seguimiento de la diabetes. Este proyecto es el resultado de una formación impartida por Inserta Arelance, combinando conocimientos teóricos y prácticos para desarrollar una solución tecnológica integral y accesible para pacientes con diabetes y profesionales de la salud.

## Backend: DiabetesNoteBook 📚

El backend, DiabetesNoteBook, es el corazón de la aplicación, encargado de la lógica de negocio, operaciones de base de datos, autenticación de usuarios, y mucho más.

### Características Principales

- **Gestión de Usuarios**: Registro, confirmación de correo electrónico, inicio de sesión, y manejo de perfiles.
- **Seguridad**: Hashing de contraseñas y manejo de tokens para autenticación segura.
- **Manejo de Mediciones**: Añadir, consultar y eliminar mediciones de glucosa y otros datos relevantes.
- **Operaciones de Auditoría**: Registro de acciones de usuarios para seguimiento y auditoría.

### Tecnologías Utilizadas

- **ASP.NET Core MVC**, **Entity Framework Core**, y **SQL Server**.

### 🚀 Instrucciones de Instalación

1. Clona el repositorio.
2. Instala .NET Core SDK y SQL Server.
3. Restaura la base de datos utilizando el archivo `.bak`.
4. **Configura los secretos de usuario** en el backend para asegurar la correcta autenticación y seguridad.
5. Configura la cadena de conexión en `appsettings.json`.
6. Ejecuta `dotnet restore` y `dotnet run`.

## Frontend 🖥️

El frontend proporciona una interfaz amigable y accesible, diseñada para ser utilizada eficientemente por todos los usuarios, incluidos aquellos con discapacidad visual.

### Características Principales

- **Registro e Inicio de Sesión**: Interfaces intuitivas.
- **Dashboard de Usuario**: Visualización y gestión de mediciones personales.
- **Accesibilidad**: Características de accesibilidad para personas con discapacidad visual.

### Tecnologías Aplicadas en el Frontend

- **Integración con APIs Externas**: 
  - **Generador de Avatares**: Utiliza una API externa para generar avatares personalizados.
  - **Vademécum**: Conecta con la API de CIMA para proporcionar información detallada sobre medicamentos.

- **Directivas**: 
  - **LazyLoadImageDirective**: Mejora el rendimiento mediante la carga diferida de imágenes.

- **Pipes**: 
  - **FiltroGenericosPipe** y **FiltroSinRecetaPipe**: Filtran medicamentos basándose en características específicas como si son genéricos o si requieren receta.

### 🚀 Instrucciones de Instalación

1. Navega al directorio del frontend.
2. Ejecuta `npm install` y `ng serve`.
3. Accede a `http://localhost:4200`.

Este proyecto, fruto de la formación de Inserta Arelance, demuestra el compromiso con la creación de soluciones tecnológicas accesibles y de alto impacto para la sociedad, abordando las necesidades de personas con condiciones crónicas como la diabetes.

---


Para más información sobre medicamentos, visita el [Vademécum de la AEMPS](https://cima.aemps.es/cima/publico/nomenclator.html).
