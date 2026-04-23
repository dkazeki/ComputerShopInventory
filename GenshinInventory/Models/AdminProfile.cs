using System;
using System.Collections.Generic;
using System.Text;

namespace GenshinInventory.Models
{
    public class AdminProfile
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
