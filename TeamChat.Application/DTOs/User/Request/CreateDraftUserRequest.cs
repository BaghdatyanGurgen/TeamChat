using System;
using System.ComponentModel.DataAnnotations;

namespace TeamChat.Application.DTOs.User.Request
{
    public class CreateDraftUserRequest
    {
        public required string Email { get; set; }
    }
}
