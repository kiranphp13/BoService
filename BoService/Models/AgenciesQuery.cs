using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;

namespace BoService.Models
{
    public class AgenciesQuery
    {
        public BoAppDB Db { get; }

        public AgenciesQuery(BoAppDB db)
        {
            Db = db;
        }

        public async Task<Agencies> FindOneAsync(int Agcy_SeqNo)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `AGCY_SEQNO`, `AGCY_DATE`, `AGCY_NAME`, 'AGCY_LIC_USERNAME' FROM `AGENCIES` WHERE `AGCY_SEQNO` = @Agcy_SeqNo";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Agcy_SeqNo",
                DbType = System.Data.DbType.Int32,
                Value = Agcy_SeqNo,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Agencies>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `AGCY_SEQNO`, `AGCY_DATE`, `AGCY_NAME`, 'AGCY_LIC_USERNAME' FROM `AGENCIES` ORDER BY `AGCY_SEQNO` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `BlogPost`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Agencies>> ReadAllAsync(System.Data.Common.DbDataReader reader)
        {

            var posts = new List<Agencies>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Agencies(Db)
                    {

                        Agcy_SeqNo = reader.GetInt32(0),
                        Agcy_Date = reader.GetDateTime(1),
                        Agcy_Name = reader.GetString(2),
                        Agcy_Lic_User_Name = reader.GetString(3),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
