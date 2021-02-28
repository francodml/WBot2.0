using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WBot2.Data
{
    public class DiscordOptions
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string CommandPrefix { get; set; }

        [Required]
        public ulong Owner { get; set; }
    }
}
