using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrtMovie
{
    public class sqliteProtocol
    {
        public SQLiteConnection sqlite_connect = new SQLiteConnection("Data source=db/config.db");
        public SQLiteCommand sqlite_cmd = null;
        public SQLiteDataReader reader = null;
        public List<string> cmds = new List<string>();
        private String m_strErrorDes = "";
        int countsql = 0;
        public sqliteProtocol()
        {
            sqliteconnect();
        }
        public bool sqliteconnect()
        {
            bool result = false;
            try
            {
                this.m_strErrorDes = "";
                sqlite_connect = new SQLiteConnection("Data source=db/config.db");
                sqlite_connect.Open();// Open
                result = true;
            }
            catch (Exception exp)
            {
                this.m_strErrorDes = "SQLite_0000: ERROR - " + exp.Message;
            }
            return result;
        }
        public bool selectcmd(string sql)
        {
            bool result = false;
            try
            {
                this.m_strErrorDes = "";
                sqlite_cmd = sqlite_connect.CreateCommand();//create command
                sqlite_cmd.CommandText = sql;
                sqlite_cmd.ExecuteNonQuery();
                reader = sqlite_cmd.ExecuteReader();
                result = true;
            }
            catch (Exception exp)
            {
                this.m_strErrorDes = "SQLite_0000: ERROR - " + exp.Message;
            }
            return result;
        }
        public bool sqlcmd(string sql)
        {
            bool result = false;
            try
            {
                this.m_strErrorDes = "";
                sqlite_cmd = sqlite_connect.CreateCommand();//create command
                sqlite_cmd.CommandText = sql;
                sqlite_cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception exp)
            {
                this.m_strErrorDes = "SQLite_0000: ERROR - " + exp.Message;
            }
            return result;
        }
        public bool countcmd(string sql)
        {
            bool result = false;
            try
            {
                sqlite_cmd = sqlite_connect.CreateCommand();//create command
                sqlite_cmd.CommandText = sql;
                countsql = Convert.ToInt32((sqlite_cmd.ExecuteScalar()));
                result = true;
            }
            catch (Exception exp)
            {
                m_strErrorDes = "ERROR_COUNTCMD:" + exp.Message;
            }
            return result;
        }
        public bool close()
        {
            bool result = false;
            try
            {
                sqlite_cmd.Dispose();
                sqlite_connect.Close();
                reader = null;
                result = true;
            }
            catch (Exception exp)
            {
                this.m_strErrorDes = "SQLite_0000: ERROR - " + exp.Message;
            }
            return result;
        }
        public void batchcmd()
        {

                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = sqlite_connect;
                SQLiteTransaction tx = sqlite_connect.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    foreach (string command in cmds)
                    {
                        cmd.CommandText = command+";";
                        cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    
                }
                tx.Dispose();
                cmds.Clear();
            
        }
        public SQLiteDataReader getreader()
        {

            return reader;
        }
        public int getcount()
        {
            return countsql;
        }
        public string GetErrorDes()
        {
            return this.m_strErrorDes;
        }
    }
}
