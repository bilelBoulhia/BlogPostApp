namespace ArtcilesServer.DTO
{
    public class UserDTO
    {

        public string UserName { get; set; } = null!;

        public string UserFamilyName { get; set; } = null!;

        public string? UserImage { get; set; }

        public int UserPhoneNumber { get; set; }

        public DateTime UserBirthDay { get; set; }

        public string UserEmail { get; set; } = null!;

        public string UserHash { get; set; } = null!;

    }
}
