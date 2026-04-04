using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.Enums;

namespace TaskManager
{
    public static class AppSession
    {
        public static DatabaseProvider CurrentDatabaseProvider { get; set; } = DatabaseProvider.PostgreSql;
    }
}
