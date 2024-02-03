using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class NewRegisterRepository : INewRegisterRepository
    {
        private readonly DiabetesNoteBookContext _context;

        public NewRegisterRepository(DiabetesNoteBookContext context)
        {
            _context = context;
        }

        public async Task SaveNewRegisterUser(Usuario newUsuario)
        {
            await _context.Usuarios.AddAsync(newUsuario);
            await _context.SaveChangesAsync();
        }

        public async Task SaveNewRegisterPerson(Persona newPerson)
        {
            await _context.Personas.AddAsync(newPerson);
            await _context.SaveChangesAsync();

        }
    }
}
