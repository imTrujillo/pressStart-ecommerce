using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.Request;
using Shop.Application.DTOs.Request.Auth;
using Shop.Application.Interfaces.Services;
using LoginRequest = Shop.Application.DTOs.Request.Auth.LoginRequest;

namespace Shop.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<RegisterCustomer> _customerValidator;
        private readonly IValidator<RegisterEmployee> _employeeValidtor;
        private readonly IValidator<ResetPassword> _resetPasswordValidator;

        public AuthController(
            IAuthService authService, 
            IValidator<LoginRequest> loginValidator,
            IValidator<RegisterCustomer> customerValidator,
            IValidator<RegisterEmployee> employeeValidator,
            IValidator<ResetPassword> resetPasswordValidator)
        {
            _authService = authService;
            _loginValidator = loginValidator;
            _customerValidator = customerValidator;
            _employeeValidtor = employeeValidator;
            _resetPasswordValidator = resetPasswordValidator;
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    Property = e.PropertyName,
                    Error = e.ErrorMessage
                });
                
                return BadRequest(errors);
            }
            
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshTokens(RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokensAsync(request);
            return Ok(result);
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(RefreshTokenRequest request)
        {
            var result = await _authService.LogoutAsync(request.RefreshToken);

            if (result)
            {
                return Ok("Successfully logged out");
            }
            else
            {
                return BadRequest("Token No Encontrado");
            }
        }

        [HttpPost]
        [Route("register/customer")]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomer request)
        {
            var validationResult = await _customerValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    Property = e.PropertyName,
                    Error = e.ErrorMessage
                });
                
                return BadRequest(errors);
            }
            
            var result = await _authService.RegisterCustomerAsync(request);
            return CreatedAtAction(nameof(RegisterCustomer), new { id = result.Id }, result);
        }

        [HttpPost]
        [Route("register/employee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterEmployee(RegisterEmployee request)
        {
            var validationResult = await _employeeValidtor.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    Property = e.PropertyName,
                    Error = e.ErrorMessage
                });
                
                return BadRequest(errors);
            }
            
            var result = await _authService.RegisterEmployeeAsync(request);
            return CreatedAtAction(nameof(RegisterEmployee), new { id = result.Id }, result);
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword request)
        {
            var validationResult = await _resetPasswordValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    Property = e.PropertyName,
                    Error = e.ErrorMessage
                });
                
                return BadRequest(errors);
            }
            
            var result = await _authService.ResetPasswordAsync(request);
            return Ok("Password Reset Successful");
        }
    }
}
