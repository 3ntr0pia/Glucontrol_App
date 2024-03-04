using Microsoft.AspNetCore.Mvc.Filters;

namespace DiabetesNoteBook.Application.Filters
{
    //Hacemos que herede de la clase  ExceptionFilterAttribute para crear el filtro de excepcion

    public class FiltroExcepcion : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _env;

        public FiltroExcepcion(IWebHostEnvironment env)
        {
            _env = env;
        }
        //Metodo interno de ExceptionFilterAttribute

        public override void OnException(ExceptionContext context)
        {
            //Donde se va a almacenar el log de errores en caso de haber una excepcion

            var path = $@"{_env.ContentRootPath}\wwwroot\logErrores.txt";
            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                //Obtine la ip
                writer.WriteLine(context.HttpContext.Connection.RemoteIpAddress);
                //Obtiene la fecha
                writer.WriteLine(DateTime.Now);
                //Obtiene la ruta
                writer.WriteLine(context.HttpContext.Request.Path);
                //Obtiene el metodo (Get, post o put)

                writer.WriteLine(context.HttpContext.Request.Method);
                //Devuelve que causo el error
                writer.WriteLine(context.Exception.Source);
                //Devuelve el mensaje de error
                writer.WriteLine(context.Exception.Message);
            }
            //En caso de haber mas filtros continuaria hacia delante

            base.OnException(context);
        }
    }
}
