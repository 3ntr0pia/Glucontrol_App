using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Application.Services
{
    public class OperationsService : IOperationsService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IAddOperation _addOperation;

        public OperationsService(IHttpContextAccessor accessor, IAddOperation addOperation)
        {
            _accessor = accessor;
            _addOperation = addOperation;
        }

        public async Task AddOperacion(DTOOperation operation)
        {

            Operacione newOperation = new Operacione()
            {
                FechaAccion = DateTime.Now,
                Operacion = operation.Operacion,
                Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                IdUsuario = operation.UserId
            };

            await _addOperation.SaveAddOpertion(newOperation);

        }

    }
}
