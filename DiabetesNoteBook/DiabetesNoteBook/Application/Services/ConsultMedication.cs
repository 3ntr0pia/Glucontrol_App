﻿using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    public class ConsultMedication : IConsultMedication
    {
        private readonly DiabetesNoteBookContext _context;

        public ConsultMedication(DiabetesNoteBookContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetMedication(DTOMedicacion userData)
        {
            var medicationNames = await _context.UsuarioMedicacions
                .Where(x => x.IdUsuario == userData.Id)
                .Join(_context.Medicaciones,
                    usuarioMedicacion => usuarioMedicacion.IdMedicacion,
                    medicacion => medicacion.IdMedicacion,
                    (usuarioMedicacion, medicacion) => medicacion.Nombre)
                .ToListAsync();

            return medicationNames;
        }


    }
}

