namespace TwoNil.Shared.DomainObjects
{
    public class GameInfo : DomainObjectBase
    {
        public GameInfo()
        {
        }

        public string Name { get; set; }

        private Team _currentTeam;

        public string CurrentTeamId { get; set; }

        public Team CurrentTeam
        {
            get
            {
                return _currentTeam;
            }
            set
            {
                _currentTeam = value;
                CurrentTeamId = value != null ? value.Id : null;
            }
        }
    }
}
