namespace DiabetesNoteBook.Application.DTOs
{
    public class DTOLoginResponse
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Rol { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Avatar { get; set; }
    }
}
