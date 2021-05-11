using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebHooks.DAL.EF
{
    class DbContextFactory : IDesignTimeDbContextFactory<WebHooksContext>
    {
        public WebHooksContext CreateDbContext(string[] args)
        {
            var contextOptions = new DbContextOptionsBuilder<WebHooksContext>()
            .UseSqlServer("")
            .Options;
            WebHooksContext context = new WebHooksContext(contextOptions);
            return context;
        }

    }
}
