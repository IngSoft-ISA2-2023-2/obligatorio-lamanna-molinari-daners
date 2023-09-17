using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using PharmaGo.Exceptions;
using Microsoft.Data.SqlClient.Server;

namespace PharmaGo.WebApi.Filters;
public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        try
        {
            throw context.Exception;
        }
        catch (ResourceNotFoundException e)
        {
            context.Result = new JsonResult(new {Message = e.Message }) { StatusCode = 404 };
        }
        catch (InvalidResourceException e)
        {
            context.Result = new JsonResult(new { Message = e.Message }) { StatusCode = 400 };
        }
        catch (FormatException e)
        {
            context.Result = new JsonResult(new { Message = "Invalid token format" }){ StatusCode = 400 };
        }
        catch (Exception e)
        {
            context.Result = new JsonResult(new { Message = e.Message }) { StatusCode = 500 };
        }
    }
}
