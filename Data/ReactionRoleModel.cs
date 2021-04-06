using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WBot2.Data
{
    public class ReactionRoleContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<RRMessage> WatchedMessages { get; set; }

        public ReactionRoleContext( DbContextOptions<ReactionRoleContext> options ) : base(options)
        {
        }

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
    }
}
