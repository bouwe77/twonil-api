using System;
using Dolores.Http;
using Dolores.Requests;
using Dolores.Responses;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;
using TwoNil.Logic.Exceptions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Controllers
{
   //[BasicAuthenticationFilter(false)]
   public class AuthenticationController : ControllerBase
   {
      public Response Post()
      {
         const string invalidRequestBodyError = "Invalid request body";

         //TODO Klopt deze check en/of is deze verstandig op deze manier?
         if (Request?.MessageBody == null || Request.MessageBody.Length == 0)
         {
            throw ResponseHelper.Get400BadRequest(invalidRequestBodyError);
         }

         AuthenticateResource authenticateResource;
         try
         {
            authenticateResource = Request.MessageBody.DeserializeJson<AuthenticateResource>();
         }
         catch (Exception)
         {
            throw ResponseHelper.Get400BadRequest(invalidRequestBodyError);
         }

         var userService = ServiceFactory.CreateUserService();

         User user;
         try
         {
            user = userService.GetUser(authenticateResource.Username, authenticateResource.Password);
         }
         catch (ValidationException validationException)
         {
            throw ResponseHelper.Get400BadRequest(validationException.Message);
         }

         if (user == null)
         {
            throw ResponseHelper.Get404NotFound("Username/password combination not found");
         }

         var response = new Response(HttpStatusCode.Ok);
         return response;
      }
   }
}