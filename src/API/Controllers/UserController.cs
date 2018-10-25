using System;
using Dolores.Http;
using Dolores.Requests;
using Dolores.Responses;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;
using TwoNil.Logic.Exceptions;
using TwoNil.Services;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Controllers
{
    public class UserController : ControllerBase
    {
        public UserController(ServiceFactory serviceFactory, UriHelper uriHelper)
            : base(serviceFactory, uriHelper)
        {
        }

        public Response PostAddUser()
        {
            const string invalidRequestBodyError = "Invalid request body";

            // Check the body is not empty.
            //TODO Klopt deze check en/of is deze verstandig op deze manier?
            if (Request?.MessageBody == null || Request.MessageBody.Length == 0)
            {
                throw ResponseHelper.Get400BadRequest(invalidRequestBodyError);
            }

            // Try to deserialize the body to an AddUserResource.
            AddUserResource addUserResource;
            try
            {
                addUserResource = Request.MessageBody.DeserializeJson<AddUserResource>();
            }
            catch (Exception)
            {
                throw ResponseHelper.Get400BadRequest(invalidRequestBodyError);
            }

            var userService = ServiceFactory.CreateUserService();

            User user;
            try
            {
                user = userService.CreateUser(addUserResource.Firstname, addUserResource.Lastname, addUserResource.Username, addUserResource.Email, addUserResource.Password);
            }
            catch (ConflictException conflictException)
            {
                throw ResponseHelper.Get409Conflict(conflictException.Message);
            }
            catch (ValidationException validationException)
            {
                throw ResponseHelper.Get400BadRequest(validationException.Message);
            }

            var response = new Response(HttpStatusCode.Created);
            response.Headers.Add("Location", UriHelper.GetUserUri(user.Id));

            return response;
        }

        public Response GetItem(string userId)
        {
            RequestHelper.ValidateId(userId);

            // The logged in user is only allowed to request its own profile.
            //TODO Let MODDERVOKKIN op
            bool allowed = "17eqhq" == userId;
            if (!allowed)
            {
                throw ResponseHelper.Get404NotFound("");
            }

            var userService = ServiceFactory.CreateUserService();
            var user = userService.GetUser(userId);
            if (user == null)
            {
                throw ResponseHelper.Get404NotFound("");
            }

            var userResource = new UserMapper(UriHelper).Map(user, UserMapper.Firstname, UserMapper.Lastname, UserMapper.Username, UserMapper.Email);

            var halDocument = CreateHalDocument(UriHelper.GetUserUri(userId));
            halDocument.AddResource("rel:user", userResource);

            var response = GetResponse(halDocument);
            return response;
        }
    }
}