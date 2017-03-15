using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace MailSender_Core
{
    public class DBHelper
    {
        string connectionString;
        
       public DBHelper(string conn)
        {
            connectionString = conn;            
        }

        public List<QRecruitmentUser> FetchClients()
        {
            try
            {
                List<QRecruitmentUser> clients = new List<QRecruitmentUser>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("exec GetClients", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        QRecruitmentUser a = new QRecruitmentUser();
                        string Email = dr[0].ToString();
                        string Name = dr[1].ToString();
                        a.Email = Email;
                        a.UserName = Name;                     
                        clients.Add(a);
                    }
                    con.Close();
                    return clients;
                }
            }
            catch(Exception e)
            {
                throw e;
            }        
        }

        public bool UpdateUser(string email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("exec Updateuser '"+email+"'", con);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }


        public List<Candidate> FetchTestMailClients()
        {
            try
            {
                List<Candidate> clients = new List<Candidate>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("exec GetTestMailClients", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Candidate a = new Candidate();
                        string Email = dr[0].ToString();
                        string Name = dr[1].ToString();
                        a.Email = Email;
                        a.Name = Name;
                        clients.Add(a);
                    }
                    con.Close();
                    return clients;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool UpdateTestMailClient(string email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("exec UpdateTestMailClients '" + email + "'", con);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
