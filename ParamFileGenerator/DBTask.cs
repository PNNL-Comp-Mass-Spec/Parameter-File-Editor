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
            string connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(connectionString, "ParamFileGenerator");
            mDBTools = DbToolsFactory.GetDBTools(connectionStringToUse);
        }

#pragma warning disable CS3001 // Type of parameter is not CLS-compliant
        public DBTask(IDBTools existingDbTools)
#pragma warning restore CS3001 // Type of parameter is not CLS-compliant
        {
            mDBTools = existingDbTools;
        }

        [Obsolete("Use the constructor that only has a connection string")]
        public DBTask(string connectionString, bool persistConnection)
        {
            string connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(connectionString, "ParamFileGenerator");
            mDBTools = DbToolsFactory.GetDBTools(connectionStringToUse);
        }

        [Obsolete("Use the constructor that accepts a connection string", true)]
        public DBTask(bool persistConnection = false)
        {
            throw new NotImplementedException();
        }

        protected DataTable GetTable(string selectSQL)
        {
            int retryCount = 3;
            int retryDelaySeconds = 5;
            int timeoutSeconds = 120;

            DataTable queryResults = null;
            bool success = mDBTools.GetQueryResultsDataTable(selectSQL, out queryResults, retryCount, retryDelaySeconds, timeoutSeconds);

            if (!success)
            {
                throw new Exception("Could not get records after three tries; query: " + selectSQL);
            }

            return queryResults;
        }
    }
}
