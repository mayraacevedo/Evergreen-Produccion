using EG.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.DAL
{
    public partial class EGDbContext : EGDbContextBase
    {
        public EGDbContext()
        {
        }

        public EGDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=eg-db-server.database.windows.net,1433;Initial Catalog=Evergreen;Persist Security Info=True;User ID=egadmin;Password=4dm1n*2023*;Timeout=600", x => x.MigrationsHistoryTable("__MyMigrationsHistory", "EG"));
            }
        }

    }
}
