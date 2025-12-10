using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamChat.Domain.Entities;

namespace TeamChat.Application.DTOs.CompanyUser
{
    public record CompanyUserResponse(Domain.Entities.CompanyUser CompanyUser);
}
