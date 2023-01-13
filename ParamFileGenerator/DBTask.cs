using System;
using System.Data;
using PRISMDatabaseUtils;

namespace ParamFileGenerator
{
    public class DBTask
    {
        // DB access

#pragma warning disable CS3003 // Type of member is not CLS-compliant
        protected readonly IDBTools mDBTools;
#pragma warning restore CS3003 // Type of member is not CLS-compliant

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public DBTask(string connectionString)
        {
            var connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(connectionString, "ParamFileGenerator");
            mDBTools = DbToolsFactory.GetDBTools(connectionStringToUse);
        }

#pragma warning disable CS3001 // Type of parameter is not CLS-compliant
        public DBTask(IDBTools existingDbTools)
#pragma warning restore CS3001 // Type of parameter is not CLS-compliant
        {
            mDBTools = existingDbTools;
        }

        protected DataTable GetTable(string sqlSelect)
        {
            const int retryCount = 3;
            const int retryDelaySeconds = 5;
            const int timeoutSeconds = 120;

            var success = mDBTools.GetQueryResultsDataTable(sqlSelect, out var queryResults, retryCount, retryDelaySeconds, timeoutSeconds);

            if (!success)
            {
                throw new Exception("Could not get records after three tries; query: " + sqlSelect);
            }

            return queryResults;
        }
    }
}
