using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FFXIVSB.Models
{
    public class SBContext : DbContext
    {
        public DbSet<SquadMember> SquadMembers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=FFXIVSB.db");
        }
    }
}
