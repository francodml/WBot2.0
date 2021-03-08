using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WBot2.Data
{
    public class ReactionRoleContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<RRMessage> Posts { get; set; }

        /*protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=reactionroles.db");*/
    }

    public class Guild
    {
        public ulong ID { get; set; }

        public List<RRMessage> ReactMessages { get; } = new();
    }

    public class RRMessage
    {
        public ulong ID { get; set; }
        [NotMapped]
        public Dictionary<string, ulong> EmojiRoles { get; set; }
        public string EmojiRolesJS
        {
            get => JsonConvert.SerializeObject(EmojiRoles);
            set => EmojiRoles = JsonConvert.DeserializeObject<Dictionary<string, ulong>>(value);
        }

        public ulong GuildID { get; set; }
        public Guild Guild { get; set; }
    }
}
