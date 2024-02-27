using DiabetesNoteBook.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IConsultMedication
    {
        Task<List<string>> GetMedication(DTOMedicacion userData);
    }
}


