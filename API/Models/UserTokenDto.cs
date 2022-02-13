using System.Collections.Generic;
using API.Enums;

public class UserTokenDto
{
    public int Id { get; set; }
    public string userName { get; set; }
    public string Token { get; set; }
    public string PhotoUrl { get; set; }
    public GenderEnum Gender { get; set; }
    public string KnownAs { get; set; }
    public List<string> Roles { get; set; }
}