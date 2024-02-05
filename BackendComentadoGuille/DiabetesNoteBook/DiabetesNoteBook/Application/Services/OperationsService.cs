using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz.
    public class OperationsService : IOperationsService
    {
        //Se llama a lo necesario para que este servicio cumpla su funcion
        private readonly IHttpContextAccessor _accessor;
        private readonly IAddOperation _addOperation;
        //Creacion del constructor
        public OperationsService(IHttpContextAccessor accessor, IAddOperation addOperation)
        {
            _accessor = accessor;
            _addOperation = addOperation;
        }
        //Agregamos el metodo que hay en la interfaz junto con el DTOOperation
        public async Task AddOperacion(DTOOperation operation)
        {
            //Se crea una nueva operacion
            Operacione newOperation = new Operacione()
            {
                //Se le pone una fecha
                FechaAccion = DateTime.Now,
                //Se le pone un nombre
                Operacion = operation.Operacion,
                //Se pone la ip de donde se ha realizado la operacion
                Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                //Se le pone la id de usuario que ha realizado esa operacion
                IdUsuario = operation.UserId
            };
            //Llamamos al servicio _addOperation para guardar los cambios
            await _addOperation.SaveAddOpertion(newOperation);

        }

    }
}
