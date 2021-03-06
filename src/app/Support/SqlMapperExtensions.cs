using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using GTDPad.Domain;
using MDSC = Microsoft.Data.SqlClient.Server;

namespace GTDPad.Support
{
    public static class SqlMapperExtensions
    {
        public static Task<T> QuerySingle<T>(this SqlConnection conn, string sql, object param) =>
            conn.QuerySingleAsync<T>(sql, param);

        public static Task<IEnumerable<T>> QuerySp<T>(this SqlConnection conn, string sql, object param) =>
            conn.QueryAsync<T>(sql, param, commandType: CommandType.StoredProcedure);

        public static Task<T> QuerySingleSp<T>(this SqlConnection conn, string sql, object param) =>
            conn.QuerySingleAsync<T>(sql, param, commandType: CommandType.StoredProcedure);

        public static Task<T> QuerySingleOrDefaultSp<T>(this SqlConnection conn, string sql, object param) =>
            conn.QuerySingleOrDefaultAsync<T>(sql, param, commandType: CommandType.StoredProcedure);

        public static Task ExecuteSp(this SqlConnection conn, string sql, object param) =>
            conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);

        public static IEnumerable<IDataRecord> AsDataRecords(this IEnumerable<Guid> ids)
        {
            Guard.AgainstNull(ids, nameof(ids));

            if (ids is null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            var records = new List<MDSC.SqlDataRecord>();

            var definition = new MDSC.SqlMetaData("ID", SqlDbType.UniqueIdentifier);

            foreach (var id in ids)
            {
                var record = new MDSC.SqlDataRecord(definition);

                record.SetGuid(0, id);

                records.Add(record);
            }

            return records;
        }
    }
}
