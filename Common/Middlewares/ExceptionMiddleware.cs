﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebApiDeveloperChallenge.Common.Classes;
using WebApiDeveloperChallenge.Common.Exceptions;
using WebApiDeveloperChallenge.Common.Extensions;

namespace WebApiDeveloperChallenge.Common.Middlewares
{
  public class ExceptionMiddleware
  {

    private readonly RequestDelegate _next;
    private readonly ILogger<Exception> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<Exception> logger)
    {
      _logger = logger;
      _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Something went wrong: {ex}");
        await HandleExceptionAsync(httpContext, ex);
      }
    }

    /// <summary>
    /// Return formatted exception 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

      var errorDetails = new ErrorDetails
      {
        StatusCode = context.Response.StatusCode,
        Message = GetMessageFromException(exception)
      }.ToString();
      return context.Response.WriteAsync(errorDetails);
    }

    /// <summary>
    /// Get specific message from exception
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    private string GetMessageFromException(Exception exception)
    {
      switch (exception)
      {
        case UserIdRelatedDataException _:
        case UserIdRelatedDataUsedException _:
          return exception.Message;
      }
      return "Unable to perform this operation.";
    }
  }
}
