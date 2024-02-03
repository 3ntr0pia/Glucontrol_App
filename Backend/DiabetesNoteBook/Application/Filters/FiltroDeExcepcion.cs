using Microsoft.AspNetCore.Mvc.Filters;

namespace DiabetesNoteBook.Application.Filters
{
    public class FiltroExcepcion : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _env;

        public FiltroExcepcion(IWebHostEnvironment env)
        {
            _env = env;
        }

        public override void OnException(ExceptionContext context)
        {

            var path = $@"{_env.ContentRootPath}\wwwroot\logErrores.txt";
            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(context.HttpContext.Connection.RemoteIpAddress);
                writer.WriteLine(DateTime.Now);
                writer.WriteLine(context.HttpContext.Request.Path);
                writer.WriteLine(context.HttpContext.Request.Method);
                writer.WriteLine(context.Exception.Source);
                writer.WriteLine(context.Exception.Message);
            }

            base.OnException(context);
        }
    }
}
