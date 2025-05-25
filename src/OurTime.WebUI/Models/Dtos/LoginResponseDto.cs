// Services/Dtos/LoginResponseDto.cs
namespace OurTime.WebUI.Services.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = default!;
        public string Message { get; set; } = default!;
    }
}
