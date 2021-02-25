using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;

namespace BoService.Models
{
    public class Agencies
    {
        public int Agcy_SeqNo { get; set; }
        public DateTime Agcy_Date { get; set; }
        public string Agcy_Name { get; set; }
        public string Agcy_Lic_User_Name { get; set; }


        internal BoAppDB Db { get; set; }

        public Agencies()
        {
        }

        internal Agencies(BoAppDB db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();

            cmd.CommandText = @"INSERT INTO `AGENCIES` (`AGCY_SEQNO`, `AGCY_DATE`, 'AGCY_NAME', 'AGCY_LIC_USERNAME') VALUES (@Agcy_SeqNo, @Agcy_Date, @Agcy_Name, @Agcy_Lic_User_Name);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Agcy_SeqNo = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `AGENCIES` SET `AGCY_SEQNO` = @Agcy_SeqNo, `AGCY_DATE` = @Agcy_Date, 'AGCY_NAME' = @Agcy_Name, 'AGCY_LIC_USERNAME' = @Agcy_Lic_User_Name  WHERE `AGCY_SEQNO` = @Agcy_SeqNo;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `AGENCIES` WHERE `AGCY_SEQNO` = @Agcy_SeqNo;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Agcy_SeqNo",
                DbType = System.Data.DbType.Int32,
                Value = Agcy_SeqNo,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Agcy_SeqNo",
                DbType = System.Data.DbType.String,
                Value = Agcy_SeqNo,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@AGCY_DATE",
                DbType = System.Data.DbType.Date,
                Value = Agcy_Date,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@AGCY_NAME",
                DbType = System.Data.DbType.String,
                Value = Agcy_Name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@AGCY_LIC_USERNAME",
                DbType = System.Data.DbType.String,
                Value = Agcy_Lic_User_Name,
            });
        }
    }
}
