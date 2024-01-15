namespace DiabetesNoteBook.Application.DTOs
{
    public class DTOCambioPassPorId
    {
        public int Id { get; set; }
        public string NewPass { get; set; }
    }

    public class DTOUsuarioChangePasswordMail
    {
        public string Email { get; set; }
    }

    public class DTOUsuarioChangePasswordMailConEnlace
    {
        public string Email { get; set; }
        public string NewPass { get; set; }
    }
}
