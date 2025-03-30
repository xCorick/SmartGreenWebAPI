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
            catch (Exception ex)
            {
                Action? accion = ex switch
                {
                    var x when x is TimeoutException => async () => await HandleExceptionAsync(context, new Exception("Compruebe su conexión de internet. Vuelva a intentarlo más tarde.")),
                    var x when x is MongoConnectionException => async() => await HandleExceptionAsync(context, new Exception("Error del servidor. Intende de nuevo más tarde.")),
                    _ => null
                    
                };
                if (accion != null)  accion();
                if(ex is InvalidOperationException) await HandleExceptionAsync(context, new  Exception("Favor de verficar los datos ingresar."));
                await HandleExceptionAsync(context, ex); //Se captura en un objeto en un exception
            }
        }
    
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
          
            context.Response.ContentType = "application/json"; //Respuesta en formato JSON
            context.Response.StatusCode = ex switch
            {
                var x when ex is TimeoutException or MongoConnectionException => 500
            };  

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
