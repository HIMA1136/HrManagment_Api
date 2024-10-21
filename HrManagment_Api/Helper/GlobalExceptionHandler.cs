using Application.Model.Common;
using FluentValidation;
using Newtonsoft.Json;
using System.Net;
namespace HrManagment_Api.Helper;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context, IWebHostEnvironment environment)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception e)
        {
            UseGlobalExceptionHandler(context, e, environment);
        }
    }

    public void UseGlobalExceptionHandler(HttpContext context, Exception ex, IWebHostEnvironment environment)
    {
        var response = context.Response;
        var result = new ResponseResult()
        {
            IsSuccssed = false,
            Message = "server-error"
        };

        if (ex == null) return;
        switch (ex)
        {
            case ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result.Message = validationException.Errors.Select(e => e.ErrorMessage).ToString();
                break;

            case Exception baseException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result.Message = baseException.Message;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Message = environment.IsProduction() == false ? ex.Message.ToString() : null;
                break;
        }
        response.ContentType = "application/json";
        response.WriteAsync(JsonConvert.SerializeObject(result));
    }
}
