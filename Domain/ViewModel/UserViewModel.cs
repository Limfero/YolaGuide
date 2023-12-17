using YolaGuide.Domain.Enums;

namespace YolaGuide.Domain.ViewModel
{
    public class UserViewModel
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public State State { get; set; }

        public Substate Substate { get; set; }
    }
}
