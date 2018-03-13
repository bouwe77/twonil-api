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
   public class LoginController : ControllerBase
   {
      public Response Post()
      {
         const string invalidRequestBodyError = "Invalid request body";

         LoginResource loginResource;
         try
         {
            loginResource = Request.MessageBody.DeserializeJson<LoginResource>();
         }
         catch (Exception)
         {
            throw ResponseHelper.Get400BadRequest(invalidRequestBodyError);
         }

         var userService = ServiceFactory.CreateUserService();

         User user;
         try
         {
            user = userService.GetUser(loginResource.Username, loginResource.Password);
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