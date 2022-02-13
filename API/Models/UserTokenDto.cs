using API.Enums;

public class UserTokenDto
{
    public string userName { get; set; }
    public string Token { get; set; }
    public string PhotoUrl { get; set; }
    public GenderEnum Gender { get; set; }
    public string KnownAs { get; set; }
}