using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace MailSender_Core
{
    public class DBHelper
    {
        string connectionString;
        
       public DBHelper(string conn)
        {
            connectionString = conn;            
        }

        public List<ApplicationUser> FetchClients()
        {
            try
            {
                List<ApplicationUser> clients = new List<ApplicationUser>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("exec GetClients", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ApplicationUser a = new ApplicationUser();
                        string Email = dr[0].ToString();
                        string Name = dr[1].ToString();
                        a.Email = Email;
                        a.UserName = Email;                     
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
