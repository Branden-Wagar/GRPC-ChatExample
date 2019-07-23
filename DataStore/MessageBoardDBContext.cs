using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore
{
    public class MessageBoardDBContext :DbContext
    {
        public DbSet<Message> MessageBoard { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (localdb)\\ProjectsV13; Database = MessageBoard; Trusted_Connection=true");
        }
    }
}
