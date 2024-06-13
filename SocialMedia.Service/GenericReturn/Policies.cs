
namespace SocialMedia.Service.GenericReturn
{
    public class Policies
    {

        public List<string> PostPolicies { get; private set; } = new List<string> 
        {
            "PUBLIC", "PRIVATE", "FRIENDS ONLY", "FRIENDS OF FRIENDS"
        };

        public List<string> ReactPolicies { get; private set; } = new List<string>
        {
            "PUBLIC", "PRIVATE", "FRIENDS ONLY", "FRIENDS OF FRIENDS"
        };

        public List<string> CommentPolicies { get; private set; } = new List<string>
        {
            "PUBLIC", "PRIVATE", "FRIENDS ONLY", "FRIENDS OF FRIENDS"
        };

        public List<string> AccountPolicies { get; private set; } = new List<string>
        {
            "PUBLIC", "PRIVATE"
        };

        public List<string> SarehneMessagePolicies { get; private set; } = new List<string>
        {
            "PUBLIC", "PRIVATE"
        };

        public List<string> AccountPostPolicies { get; private set; } = new List<string>
        {
            "PUBLIC", "PRIVATE", "FRIENDS ONLY", "FRIENDS OF FRIENDS"
        };

        public List<string> FriendListPolicies { get; private set; } = new List<string>
        {
            "PUBLIC", "PRIVATE", "FRIENDS ONLY", "FRIENDS OF FRIENDS"
        };

    }
}
