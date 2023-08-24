using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthApi.Dtos;
using UserAuthApi.Entities;
using UserAuthApi.Services.Interfaces;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userservice;
        private readonly IPasswordService _passwordService;
        public UserController(IUserService userservice)
        {
            _userservice = userservice;
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserDto request)
        {
            UserResponseDto response = new UserResponseDto();
            try
            {
                _passwordService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                User newUser = new User()
                {
                    Name = request.Name,
                    LastName = request.LastName,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                await _userservice.createUser(newUser);

                response.Code = 200;
                response.Text = "Usuário criado com sucesso, verifique seu email para a confirmação do seu usuário.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = ex.HResult;
                response.Text = ex.Message;
            }

            return BadRequest(response);
        }

        
    }
}
