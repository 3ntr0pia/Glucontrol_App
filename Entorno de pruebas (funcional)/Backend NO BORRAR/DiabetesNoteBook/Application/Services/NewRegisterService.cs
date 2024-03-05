using Microsoft.EntityFrameworkCore;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz.
    public class NewRegisterService : INewRegister
    {
        //Llamamos a los servicios necesarios y base de datos para hacer uso de ellos 

        private readonly DiabetesNoteBookContext _context;
        private readonly HashService _hashService;
        

		//Creamos el contructor

		public NewRegisterService(DiabetesNoteBookContext context, HashService hashService
			 )
        {
            _context = context;
            _hashService = hashService;
           

        }
        //Agregamos el metodo que esta en la interfaz al cual se le pasa un DTORegister
        //que tiene los datos necesarios para registrar un usuario
        public async Task NewRegister(DTORegister userData)
        {
            try
            {
                var resultadoHash = _hashService.Hash(userData.Password);

                var newUsuario = new Usuario
                {
                    Avatar = userData.Avatar,
                    UserName = userData.UserName,
                    Email = userData.Email,
                    Password = resultadoHash.Hash,
                    Salt = resultadoHash.Salt,
                    Rol = "user",
                    Nombre = userData.Nombre,
                    PrimerApellido = userData.PrimerApellido,
                    SegundoApellido = userData.SegundoApellido,
                    Sexo = userData.Sexo,
                    Edad = userData.Edad,
                    Peso = userData.Peso,
                    Altura = userData.Altura,
                    Actividad = userData.Actividad,
                    TipoDiabetes = userData.TipoDiabetes,
                    Insulina = userData.Insulina
                };

                // Guardar el usuario
                await _context.Usuarios.AddAsync(newUsuario);
                //Guardamos los cambios

                await _context.SaveChangesAsync();
                // Dividir la cadena de medicamentos en medicamentos individuales
                var medicamentos = userData.Medicacion.SelectMany(m => m.Split(','));

                // Guardar los medicamentos proporcionados por el usuario como registros independientes en la tabla Medicaciones
                foreach (var medicacionNombre in medicamentos)
                {
                    // Eliminar espacios en blanco alrededor del nombre de la medicación
                    var nombreMedicacion = medicacionNombre.Trim();
					// Verificar si la medicación ya existe en la base de datos
					

				var medicacionExistente = await _context.Medicaciones.FirstOrDefaultAsync(m => m.Nombre == nombreMedicacion);
					if (medicacionExistente == null)
					{
					    // Si la medicación no existe, se crea un nuevo registro en la tabla Medicaciones
					    var nuevaMedicacion = new Medicacione { Nombre = nombreMedicacion };
					    _context.Medicaciones.Add(nuevaMedicacion);
					}
				}
				// Guardar los cambios en la base de datos
				await _context.SaveChangesAsync();

                // Asociar las medicaciones con el usuario en la tabla UsuarioMedicaciones
                foreach (var medicacionNombre in medicamentos)
                {
                    // Eliminar espacios en blanco alrededor del nombre de la medicación
                    var nombreMedicacion = medicacionNombre.Trim();
					//await _userMedicationAssociation.AssociateMedicationWithUser(newUsuario.Id, nombreMedicacion);

					// Obtener el objeto Medicacion correspondiente al nombre de la medicacion
					var medicacion = await _context.Medicaciones.FirstOrDefaultAsync(m => m.Nombre == nombreMedicacion);

					var usuarioMedicacion = new UsuarioMedicacion
					{
					    // Asignar el ID del nuevo usuario
					    IdUsuario = newUsuario.Id,
					    // Asignar el ID de la medicación
					   IdMedicacion = medicacion.IdMedicacion
					};
					//// Agregar la relación a la tabla UsuarioMedicaciones
					_context.UsuarioMedicacions.Add(usuarioMedicacion);
				}
				// Guardar los cambios en la base de datos
				await _context.SaveChangesAsync();

			}
            catch
            {
                
            }
        }


    }
}