using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Web.Helpers;
using MVCProject.Security;
using MVCProject.Models;
using System.Web.ModelBinding;
using System.Linq;

namespace MVCProject.DAL
{
    public class UserDAL
    {
        private string ConnStr;
        public UserDAL()
        {
            ConnStr = ConfigurationManager.ConnectionStrings["MyDBConnStr"].ConnectionString;
        }

        public List<User> GetUsers()
        {
            List<User> userList = new List<User>();

            // Data Reader
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    string query = "SELECT UserID, UserName, Email FROM Users";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                userid = Convert.ToInt32(reader["UserID"]),
                                username = Convert.ToString(reader["Username"]),
                                email = Convert.ToString(reader["Email"])
                            };
                            userList.Add(user);
                        }
                    }
                }
            }
            /* Data Adapter
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    string query = "SELECT UserID, UserName, Email FROM Users";

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                    DataSet dataset = new DataSet();
                    dataAdapter.Fill(dataset, "Users");

                    DataTable usersTable = dataset.Tables["Users"];

                    foreach (DataRow row in usersTable.Rows)
                    {
                        User user = new User
                        {
                            userid = Convert.ToInt32(row["UserID"]),
                            username = row["UserName"].ToString(),
                            email = row["Email"].ToString()
                        };
                        userList.Add(user);
                    }
                }
            }*/
            catch (SqlException e)
            {
                Console.WriteLine("SQL Error Occurred: " + e.Message);
                throw;
            } catch (Exception e)
            {
                Console.WriteLine("Error Occurred: " + e.Message);
                throw;
            }
            return userList;
        }

        public void NewUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    string query = "INSERT INTO Users (UserName, Password, Email) VALUES (@Username, @HashedPassword, @VerifiedEmail)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        string hashedPassword = PasswordSecurity.HashPassword(user.password);
                        cmd.Parameters.AddWithValue("@Username", user.username);
                        cmd.Parameters.AddWithValue("@HashedPassword", hashedPassword);
                        cmd.Parameters.AddWithValue("@VerifiedEmail", user.email);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                } catch (SqlException e)
                {
                    Console.WriteLine("SQL Error Occurred: " + e.Message);
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error Occurred: " + e.Message);
                    throw;
                }
            }
        }

        public bool IsEmailExists(string email)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                } catch (SqlException e)
                {
                    Console.WriteLine("SQL Error Occurred: " + e.Message);
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error Occurred: " + e.Message);
                    throw;
                }
            }
        }

        public bool IsUsernameExists(string username)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try {
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                } catch (SqlException e)
                    {
                    Console.WriteLine("SQL Error Occurred: " + e.Message);
                    throw;
                }
                    catch (Exception e)
                    {
                    Console.WriteLine("Error Occurred: " + e.Message);
                    throw;
                }
            }
        }
    }
}