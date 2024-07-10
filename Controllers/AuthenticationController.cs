using System;
using System.Net.Mail;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjektTabAPI.Entities.Domain;
using ProjektTabAPI.Entities.Dtos.Client;
using ProjektTabAPI.Repositories;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ProjektTabAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;
        private readonly IVerificationCodeRepository _verificationCodeRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;
        private static readonly bool _is2FAEnabled = true;
        private readonly ILoginRepository _loginRepository;

        public AuthenticationController(IMapper mapper, IClientRepository clientRepository, ILoginRepository loginRepository, IVerificationCodeRepository verificationCodeRepository, IConfiguration configuration, ILogger<AuthenticationController> logger)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
            _verificationCodeRepository = verificationCodeRepository;
            _configuration = configuration;
            _logger = logger;
            _loginRepository = loginRepository;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentialsDto loginCredentials)
        {
            var foundClient = await _clientRepository.GetClientByLogin(loginCredentials.Login);

            if (foundClient is null)
            {
                return NotFound("Nie znaleziono użytkownika z podanym loginem");
            }

            if (foundClient.NumberOfTries == 5)
            {
                foundClient.Blocked = true;
                await _clientRepository.SaveChangesAsync();
            }

            if (foundClient.Blocked)
            {
                return Unauthorized("Twoje konto zostało zablokowane");
            }

            if (foundClient.Password == loginCredentials.Password)
            {
                if (_is2FAEnabled)
                {
                    // Generate 2FA code
                    var verificationCode = GenerateVerificationCode();
                    await _verificationCodeRepository.SaveVerificationCode(foundClient.Id, verificationCode);

                    // Send email with the code
                    SendVerificationCodeByEmail(foundClient.Email, verificationCode);
                    Login login = new Login();
                    login.Id_Client = foundClient.Id;
                    login.DateTime = DateTime.Now;
                    login.Successful = true;
                    login.Client = foundClient;
                    await _loginRepository.Create(login);
                    // Return message for 2FA
                    return Ok($"Wysłano na twoją skrzynkę pocztową {foundClient.Email} wiadomość. Przepisz otrzymane cyfry aby kontynuować.");
                }
                else
                {

                    var foundClientDto = _mapper.Map<ClientSimpleDto>(foundClient);
                    foundClient.NumberOfTries = 0;
                    await _clientRepository.SaveChangesAsync();
                    Login login = new Login();
                    login.Id_Client = foundClient.Id;
                    login.DateTime = DateTime.Now;
                    login.Successful = true;
                    login.Client = foundClient;
                    await _loginRepository.Create(login);
                    return Ok(foundClientDto);
                }
            }
            else
            {
                foundClient.NumberOfTries++;
                await _clientRepository.SaveChangesAsync();
                Login login = new Login();
                login.Id_Client = foundClient.Id;
                login.DateTime = DateTime.Now;
                login.Successful = false;
                login.Client = foundClient;
                await _loginRepository.Create(login);
                return BadRequest($"Niepoprawne hasło pozostało {5 - foundClient.NumberOfTries} prób logowania.");
            }
        }

        private string GenerateVerificationCode()
        {
            var random = new Random();
            var code = new StringBuilder(6);
            for (int i = 0; i < 6; i++)
            {
                code.Append(random.Next(0, 10));
            }
            return code.ToString();
        }

        private void SendVerificationCodeByEmail(string email, string code)
        {
            var smtpSettings = _configuration.GetSection("Smtp");

            using (var smtpClient = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
            {
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                smtpClient.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["FromEmail"]),
                    Subject = "Your verification code",
                    Body = $"Your verification code is {code}",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
            }
        }
        [HttpPost]
        [Route("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto verifyCodeDto)
        {
            var foundClient = await _clientRepository.GetClientByLogin(verifyCodeDto.Login);
            if (foundClient == null)
            {
                return NotFound("Client not found with the provided login.");
            }

            var storedCode = await _verificationCodeRepository.GetVerificationCode(foundClient.Id);
            _logger.LogInformation($"Stored Code: {storedCode}");

            if (storedCode == verifyCodeDto.Code)
            {
                var foundClientDto = _mapper.Map<ClientSimpleDto>(foundClient);
                foundClient.NumberOfTries = 0;
                await _clientRepository.SaveChangesAsync();
                return Ok(foundClientDto);
            }
            else
            {
                return BadRequest("Invalid verification code.");
            }
        }

        [HttpGet]
        [Route("is-2fa-enabled")]
        public IActionResult Is2FAEnabled()
        {
            return Ok(new { is2FAEnabled = _is2FAEnabled });
        }
    }
}
