using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using MCS.Common;

namespace MCS.DataAccess
{
    public class DbMigrationBase
    {
        private const string SEEDDATA_SCRIPTS_DIRECTORY = @"SeedData";
        private const string SCHEMA_NAME = "SchemaName";
        private readonly DbContext _context;
        public DbMigrationBase(DbContext context)
        {
            _context = context;
        }
        private string MigrationsRootFolderPath
        {
            get
            {
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                if (HttpContext.Current != null)
                    baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
                return baseDirectory;
            }
        }
        private string InitialCatalog
        {
            get
            {
                if (_context != null)
                {
                    return _context.Database.Connection.Database;
                }
                return string.Empty;
            }
        }
        public void ExecuteSqlFilesWithinSeedFolder(string SeedPathName)
        {
            //If you need to track the code during execute command uncomment the below code.
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
            if (SeedPathName.StartsWith(@"\"))
                SeedPathName = SeedPathName.Substring(1);

            var fullFolderPath = Path.Combine(MigrationsRootFolderPath, SeedPathName, SEEDDATA_SCRIPTS_DIRECTORY);
            ExecuteSqlFilesInFolder(fullFolderPath);
        }
        private void ExecuteSqlFilesInFolder(string fullFolderPath)
        {
            if (!Directory.Exists(fullFolderPath))
                return;

            var files = Directory.EnumerateFiles(fullFolderPath, "*.*", SearchOption.TopDirectoryOnly)
                            .Where(f => f.ToLowerInvariant().EndsWith(".sql")).OrderBy(f => f);

            foreach (var file in files)
            {
                ExecuteSqlScriptInFile(file);
            }
        }
        private void ExecuteSqlScriptInFile(string filePath)
        {
            try
            {
                var query = File.ReadAllText(filePath, Encoding.Default);
                if (string.IsNullOrEmpty(query))
                    return;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                    query = query.Replace(SCHEMA_NAME, SystemConfigurations.OracleSchemaName);
                else
                    query = query.Replace(SCHEMA_NAME, SystemConfigurations.SchemaName);

                ExecuteSqlScript(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ExecuteSqlScript(string sqlScript)
        {
            try
            {

                //Split the scripts by GO statement and execute the scripts. This will be helpful scenarios like scripts with huge seed data. 
                if (_context != null)
                {
                    _context.Database.Connection.Open();
                    if (SystemConfigurations.IsOracleMigrationEnabled == false)
                    {
                        string[] sql = sqlScript.Split(new[] { "\r\nGO\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var sqlCommand in sql)
                        {
                            var sqlToRun = sqlCommand;
                            if (!string.IsNullOrWhiteSpace(sqlToRun))
                            {
                                if (sqlToRun.ToLowerInvariant().EndsWith("go"))
                                    sqlToRun = sqlToRun.Substring(0, sqlCommand.Length - 2);

                                if (sqlToRun.ToLowerInvariant().StartsWith("go"))
                                    sqlToRun = sqlToRun.Substring(2);

                                _context.Database.ExecuteSqlCommand($"USE [{InitialCatalog}]");
                                _context.Database.ExecuteSqlCommand(sqlToRun);
                                _context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        _context.Database.ExecuteSqlCommand(sqlScript);
                    }
                    _context.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
