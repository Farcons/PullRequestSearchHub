namespace PipelineSearchHub.ExceptionsFilters
{
    public class GlobalExceptionHandler(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = StatusCodes.Status500InternalServerError;
            var message = exception.InnerException?.Message ?? exception.Message;

            if (message.StartsWith("Erro") && message.Length >= 7)
            {
                var statusCodeText = message.Substring(4, 3);
                if (int.TryParse(statusCodeText, out var parsedStatusCode))
                {
                    statusCode = parsedStatusCode;
                    message = message.Substring(7);
                }
            }

            response.StatusCode = statusCode;

            var result = new
            {
                detail = message,
                statusCode,
                title = GetTitleForStatusCode(statusCode)
            };

            return response.WriteAsJsonAsync(result);
        }

        private static string GetTitleForStatusCode(int statusCode) => statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad Request",
            StatusCodes.Status401Unauthorized => "Unauthorized",
            StatusCodes.Status403Forbidden => "Forbidden",
            StatusCodes.Status404NotFound => "Not Found",
            StatusCodes.Status500InternalServerError => "Internal Server Error",
            _ => "Error"
        };
    }
}