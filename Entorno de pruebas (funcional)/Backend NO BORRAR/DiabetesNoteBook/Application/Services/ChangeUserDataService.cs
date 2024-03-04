using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class ChangeUserDataService : IChangeUserDataService
    {
        private readonly DiabetesNoteBookContext _context;
       
        
		//Creamos el constructor

		public ChangeUserDataService(DiabetesNoteBookContext context 
            )
        {
            _context = context;
            
          
          
        }
        //Ponemos el metodo que se encuentra en la interfaz el cual tiene un DTOChangeUserData que contiene
        //datos para poder hacer que este metodo cumpla su funcion
        
        public async Task ChangeUserData(DTOChangeUserData changeUserData)
        {
            try
            {
				//Como hemos implementado la separacion de responsabilidades ahora la logica de este get
				//se encuentra en la carpeta repositories-->GetOperations-->UserMedicationRetrieval.cs
				//var usuarioUpdate = await _userMedicationRetrieval.GetUserWithMedications(changeUserData.Id);
                // Buscar el usuario en la base de datos por su ID
                var usuarioUpdate = await _context.Usuarios.Include(x=>x.UsuarioMedicacions).AsTracking().FirstOrDefaultAsync(x => x.Id == changeUserData.Id);
                if (usuarioUpdate != null)
                {  
					
					usuarioUpdate.Avatar = changeUserData.Avatar;
					usuarioUpdate.Nombre = changeUserData.Nombre;
					usuarioUpdate.PrimerApellido = changeUserData.PrimerApellido;
					usuarioUpdate.SegundoApellido = changeUserData.SegundoApellido;
					usuarioUpdate.Sexo = changeUserData.Sexo;
					usuarioUpdate.Edad = changeUserData.Edad;
					usuarioUpdate.Peso = changeUserData.Peso;
					usuarioUpdate.Altura = changeUserData.Altura;
					usuarioUpdate.Actividad = changeUserData.Actividad;
					usuarioUpdate.TipoDiabetes = changeUserData.TipoDiabetes;
					usuarioUpdate.Insulina = changeUserData.Insulina;

					
				

                  
                   
                    ///Eliminar medicacion usuario
                     _context.UsuarioMedicacions.RemoveRange(usuarioUpdate.UsuarioMedicacions);
                    ///Guardar los cambios en la base de datos
                    await _context.SaveChangesAsync();
                  

                    //Se llama al repository encargado de eliminar la medicacion.
                  
                    
                    //toma los medicamentos asociados al usuario y los pone en una sola linea
                    var medicamentos = changeUserData.Medicacion.SelectMany(m => m.Split(','));
                    foreach (var medicacionNombre in medicamentos)
                    {
                        // Eliminar espacios en blanco alrededor del nombre de la medicación
                        var nombreMedicacion = medicacionNombre.Trim();
                       
                        ///   //Verificar si la medicación ya existe en la base de datos

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
                        //await _userMedicationAssociation.AssociateMedicationWithUser(usuarioUpdate.Id, nombreMedicacion);
						///<summary>
						///Al realizar la separacion de responsabilidad la logica de asociacion se encuentra en otro archivo
						/// este archivo se llama AsociacionDeMedicacionConUsuario.cs el cual tiene una interfaz que dictamina
						/// como tiene que ser. Lo que antes poniamos aqui sin separacion de responsabilidad es esto:
						///  // Obtener el objeto Medicacion correspondiente al nombre de la medicacion
						var medicacion = await _context.Medicaciones.FirstOrDefaultAsync(m => m.Nombre == nombreMedicacion);

						var usuarioMedicacion = new UsuarioMedicacion
						{
						    // Asignar el ID del nuevo usuario
						    IdUsuario = usuarioUpdate.Id,
						   // Asignar el ID de la medicación
						   IdMedicacion = medicacion.IdMedicacion
						};
						/// Agregar la relación a la tabla UsuarioMedicaciones
						_context.UsuarioMedicacions.Add(usuarioMedicacion);
						
						/// </summary>

					}
                    // Guardar los cambios en la base de datos
                    await _context.SaveChangesAsync();
                    // Eliminar las medicaciones que ya no están asociadas a ningún usuario
                    var medicacionesViejas = await _context.Medicaciones
                    .Where(m => !m.UsuarioMedicacions.Any())
                     .ToListAsync();
                    //var medicacionesViejas = await _deleteOldMedication.GetMedicationsWithoutUsers();
                    //await _medicationRemoval.RemoveMedications(medicacionesViejas);
                    _context.Medicaciones.RemoveRange(medicacionesViejas);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
               
            }
        }
    }
}
