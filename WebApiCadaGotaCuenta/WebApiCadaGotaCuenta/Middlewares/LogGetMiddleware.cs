namespace WebApiCadaGotaCuenta.Middlewares
{
    public class LogGetMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment env;

        public LogGetMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            this.next = next;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            if (httpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {

                var IP = httpContext.Connection.RemoteIpAddress.ToString();
                var ruta = httpContext.Request.Path;
                var metodo = httpContext.Request.Method;
                var fechaHora = DateTime.Now;

                var path = $@"{env.ContentRootPath}\wwwroot\consultas.txt";
                using (StreamWriter writer = new StreamWriter(path, append: true))
                {
                    writer.WriteLine($@"{IP}-{metodo}-{ruta}-{fechaHora}");
                }

            }

            await next(httpContext);
        }
    }
}
