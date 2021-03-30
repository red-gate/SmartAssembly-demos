using System;

namespace SmartAssembly.DatabaseSample.Data
{
    static class DatabaseExtensions
    {
        public static Guid ParseGuid(this object obj) => obj switch
        {
            Guid => (Guid)obj, // SQL Server
            byte[] bytes => new Guid(bytes), // SQLite
            string str => new Guid(str),
            _ => throw new ArgumentOutOfRangeException(nameof(obj), obj, null),
        };
    }
}
