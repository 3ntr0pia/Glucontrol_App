namespace WebApiCadaGotaCuenta.DTOs
{
    public class DTOLoginUsuario
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class DTOLoginResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Rol { get; set; }
    }
}
