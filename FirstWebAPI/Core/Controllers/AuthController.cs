using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FirstWebAPI.Core.Contracts;
using FirstWebAPI.Core.Dto.UserDto;
using FirstWebAPI.Core.Entities;

namespace FirstWebAPI.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register (UserRegisterDto request)
        {
            var response = await _authRepository.Register(
                new User { Username= request.Username ,
                FirstName=request.FirstName,
                LastName=request.LastName,
                Address=request.Address,
                Email=request.Email,
                Phone=request.Phone} , request.Password );
            if( !response.Success )
            {
                return BadRequest( response );
            }
            return Ok( response );
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login (UserLoginDto request)
        {
            var response = await _authRepository.Login(request.Username, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
