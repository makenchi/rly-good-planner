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

        [HttpPatch]
        public async Task<ActionResult<UserResponseDto>> Update(UpdateUserDto request)
        {
            UserResponseDto response = new UserResponseDto();
            try
            {
                var user = await _userservice.getUserById(request.Email);
                if (user == null)
                {
                    response.Code = 404;
                    response.Text = "Usuário não encontrado";

                    return NotFound(response);
                }

                user.Name = request.Name;
                user.LastName = request.LastName;
                await _userservice.updateUser(user);

                response.Code = 200;
                response.Text = "Informações do usuário atualizadas";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = ex.HResult;
                response.Text = ex.Message;
            }

            return BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<UserResponseDto>> Delete(DeleteUserDto request)
        {
            UserResponseDto response = new UserResponseDto();
            try
            {
                var user = await _userservice.getUserById(request.Email);
                if (user == null)
                {
                    response.Code = 404;
                    response.Text = "Usuário não encontrado";

                    return NotFound(response);
                }

                if (!_passwordService.VerifyPasswordHash(request.Password,user.PasswordHash,user.PasswordSalt))
                {
                    response.Code = 500;
                    response.Text = "Não autorizado a excluir usuário";

                    return BadRequest(response);
                }

                await _userservice.removeUser(user);

                response.Code = 200;
                response.Text = "Usuário excluído com sucesso";

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = ex.HResult;
                response.Text = ex.Message;
            }

            return BadRequest(response);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<GetUserDto>> Get(string email)
        {
            GetUserDto response = new GetUserDto();
            
            var user = await _userservice.getUserById(email);
            if (user != null)
            {
                response.Code = 200;
                response.Name = user.Name;
                response.LastName = user.LastName;
                response.Email = user.Email;

                return Ok(response);
            }

            response.Code = 404;
            return NotFound(response);
        }
    }
}
