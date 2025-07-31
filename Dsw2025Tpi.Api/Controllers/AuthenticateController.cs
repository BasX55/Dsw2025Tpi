using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtTokenService _jwtTokenService;
        public AuthenticateController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager ,JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid username or password");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var rolAsignado = roles.FirstOrDefault()?.ToUpper(); 

            if (rolAsignado is null)
                return Unauthorized("El usuario no tiene ningún rol asignado.");

            string token = _jwtTokenService.GenerateToken(user.UserName, rolAsignado);


            return Ok(new { Token = token });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (registerModel == null || string.IsNullOrEmpty(registerModel.Username) || string.IsNullOrEmpty(registerModel.Password))
            {
                return BadRequest("Invalid registration request");
            }
            
            
            string[] rolesPermitidos = { "ADMINISTRADOR", "CLIENTE" };

            if (!rolesPermitidos.Contains(registerModel.Role.ToUpper()))
            {
                return BadRequest($"El rol '{registerModel.Role}' no está permitido.");
            }
            if (await _userManager.FindByNameAsync(registerModel.Username) is not null)
                return BadRequest($"El nombre de usuario '{registerModel.Username}' ya está en uso.");

            var user = new IdentityUser { UserName = registerModel.Username, Email = registerModel.Email };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }
            await _userManager.AddToRoleAsync(user, registerModel.Role.ToUpper());
            return Ok("User registered successfully");
        }

        [HttpPost("crear-rol/{nombre}")]
        public async Task<IActionResult> CrearRol(string nombre, [FromServices] RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.RoleExistsAsync(nombre))
                return BadRequest($"El rol '{nombre}' ya existe.");

            var result = await roleManager.CreateAsync(new IdentityRole(nombre));
            if (result.Succeeded)
                return Ok($"Rol '{nombre}' creado correctamente.");

            return StatusCode(500, result.Errors.Select(e => e.Description));
        }
        [HttpGet("mi-rol")]
        public IActionResult VerRol()
        {
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;
            return Ok($"Tu rol es: {rol}");
        }
    }
    
        
    }
