namespace WebApiCadaGotaCuenta.Middlewares
{
    public class LogFileBodyHttpResponseMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment env;

        public LogFileBodyHttpResponseMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            this.next = next;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            using (var ms = new MemoryStream())
            {
                var bodyOriginalRespuesta = httpContext.Response.Body;
                httpContext.Response.Body = ms;

                await next(httpContext);

                ms.Seek(0, SeekOrigin.Begin);
                string respuesta = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(bodyOriginalRespuesta);
                httpContext.Response.Body = bodyOriginalRespuesta;

                var path = $@"{env.ContentRootPath}\wwwroot\log.txt";
                using (StreamWriter writer = new StreamWriter(path, append: true))
                {
                    writer.WriteLine(respuesta);
                }
            }
        }
    }
}
