using System;
using System.Linq;
using Shally.Forms;
using Shally.Hal;
using TwoNil.Logic.Calendar;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
    public class GameDateTimeMapper : IResourceMapper<GameDateTime>
    {
        private readonly UriHelper _uriHelper;
        public static string Date = "date";
        public static string DateTime = "datetime";
        public static string NavigationForm = "navigate-to-next-game-datetime";

        public GameDateTimeMapper(UriHelper uriHelper)
        {
            _uriHelper = uriHelper;
        }

        public Resource Map(GameDateTime gameDateTime, params string[] properties)
        {
            var resource = new Resource(new Link(_uriHelper.GetPresentGameDateTimeUri(gameDateTime.GameId, gameDateTime.Id)));

            if (!properties.Any() || properties.Contains(Date))
            {
                resource.AddProperty(Date, gameDateTime.Date);
            }

            if (!properties.Any() || properties.Contains(DateTime))
            {
                resource.AddProperty(DateTime, gameDateTime.DateTime);
            }

            if (!properties.Any() || properties.Contains(NavigationForm))
            {
                AddForm(resource, gameDateTime.GameId, gameDateTime.CanNavigateToNext());
            }

            return resource;
        }

        private void AddForm(Resource resource, string gameId, bool isEnabled)
        {
            var form = new Form(NavigationForm)
            {
                Action = _uriHelper.GetPastGameDateTimesUri(gameId),
                Method = "post",
                IsEnabled = isEnabled
            };

            resource.AddForm(form);
        }
    }
}
