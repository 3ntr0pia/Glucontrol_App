namespace WebApiCadaGotaCuenta.DTOs
{
    public class DTOUsuarioRegistro
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }

    //public class DTOUsuario
    //{
    //    public int? Id { get; set; }
    //    public string UserName { get; set; }
    //    public string Email { get; set; }
    //    public string Password { get; set; }
    //    public string Rol { get; set; }

    //}

    //public class DTOUsuarioLinkChangePassword
    //{
    //    public string Email { get; set; }
    //}

    public class DTOLoginResponse
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Rol { get; set; }
    }

    //public class DTOUsuarioChangePassword
    //{
    //    public string Email { get; set; }
    //    public string Password { get; set; }
    //    public string Enlace { get; set; }
    //}

}
