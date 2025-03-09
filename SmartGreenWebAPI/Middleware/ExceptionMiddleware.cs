using System.Net;
using System.Text.Json;
using MongoDB.Driver;

namespace SmartGreenWebAPI.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try //Intentar ejecutar el código 
            {
                await next(context); 
            }
            catch (MongoException ex)
            {
                await HandleExceptionAsync(context, ex); //Se captura en un objeto en un exception
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json"; //Respuesta en formato JSON
            context.Response.StatusCode = (int)HttpStatusCode.Conflict; //Convertir el código de estado en un entero

            var respose = new
            {
                status = context.Response.StatusCode,
                message = "Ocurrio un error inesperado.",
                error = ex.Message,
            }; //Mensaje de error

            var jsonResponse = JsonSerializer.Serialize(respose); //Serializar el mensaje de error
            return context.Response.WriteAsync(jsonResponse); //Enviar el mensaje de error
        }
    }
}
