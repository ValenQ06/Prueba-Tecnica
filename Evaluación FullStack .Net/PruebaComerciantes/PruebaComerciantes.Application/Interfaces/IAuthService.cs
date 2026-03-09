using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PruebaComerciantes.Application.DTOs;

namespace PruebaComerciantes.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginRequestDto request);
    }
}
