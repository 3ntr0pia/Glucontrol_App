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
        private readonly INewRegisterRepository _newRegisterRepository;
        //Creamos el contructos
        public NewRegisterService(DiabetesNoteBookContext context, HashService hashService, INewRegisterRepository newRegisterRepository)
        {
            _context = context;
            _hashService = hashService;
            _newRegisterRepository = newRegisterRepository;
        }
        //Agregamos el metodo que esta en la interfaz al cual se le pasa un DTORegister
        //que tiene los datos necesarios para registrar un usuario
        public async Task NewRegister(DTORegister userData)
        {
            //Este método inicia una nueva transacción de base de datos.
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //Llamamos al servicio _hashService y usamos el metodo Hash para cifrar la 
                    //contraseña que el usuario nos pase
                    var resultadoHash = _hashService.Hash(userData.Password);
                    //Creamos el nuevo usuario
                    var newUsuario = new Usuario
                    {
                        Avatar = userData.Avatar,
                        UserName = userData.UserName,
                        Email = userData.Email,
                        Password = resultadoHash.Hash,
                        Salt = resultadoHash.Salt,
                        Rol = "user"
                    };

                    // Guardar el usuario, llamando a _newRegisterRepository
                    await _newRegisterRepository.SaveNewRegisterUser(newUsuario);

                    // Obtener el usuario después de guardarlo
                    var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.UserName == userData.UserName);
                    //Creamos la persona para ese usuario
                    var nuevaPersona = new Persona
                    {
                        Nombre = userData.Nombre,
                        PrimerApellido = userData.PrimerApellido,
                        SegundoApellido = userData.SegundoApellido,
                        Sexo = userData.Sexo,
                        Edad = userData.Edad,
                        Peso = userData.Peso,
                        Altura = userData.Altura,
                        Actividad = userData.Actividad,
                        Medicacion = String.Join(",",userData.Medicacion),//esto hace que en front se devuelva un array separado por comas
                        TipoDiabetes = userData.TipoDiabetes,
                        Insulina = userData.Insulina,
                        UserId = usuario.Id
                    };
                    

                    // Guardar la persona
                    await _newRegisterRepository.SaveNewRegisterPerson(nuevaPersona);
                    //Si todas las operaciones dentro del bloque try tienen éxito
                    //sin ninguna excepción, se llama al método Commit(). Esto confirma
                    //todos los cambios realizados dentro de la transacción en la base de datos,
                    //haciendo que los cambios sean permanentes.
                    transaction.Commit();
                }
                catch
                {
                    //Bloque catch: Si ocurre una excepción durante alguna de las operaciones
                    //dentro del bloque try, se llama al método Rollback(). Esto deshace cualquier
                    //cambio realizado dentro de la transacción, asegurando que la base de datos
                    //permanezca en un estado consistente.
                    transaction.Rollback();

                }
            }
        }
    }
}