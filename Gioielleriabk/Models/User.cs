﻿namespace Gioielleriabk.Models
{
    public class User
    {
        public int Id { get; set; } 

        public string Name { get; set; }
        public string Email { get; set; } 
        public string PasswordHash { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Role Role { get; set; }
    }
}
