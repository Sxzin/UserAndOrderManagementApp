﻿using UserAndOrderManagement.Models.Dto;

namespace UserAndOrderManagementApp.Models.Dto
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}