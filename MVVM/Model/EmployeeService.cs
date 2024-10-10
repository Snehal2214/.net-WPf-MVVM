using MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace MVVM.Model
{
    public class EmployeeService
    {
        SqlConnection ObjSqlConnection;
        SqlCommand ObjSqlCommand;

        private static List<Employee> ObjEmployeeList;
        public EmployeeService()
        {
            ObjSqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EMSConnection"].ConnectionString);
            ObjSqlCommand = new SqlCommand();
            ObjSqlCommand.Connection = ObjSqlConnection;
            ObjSqlCommand.CommandType = CommandType.StoredProcedure;
            //ObjEmployeeList = new List<Employee>()
            //{
            //    new Employee { Id = 101, Name = "om", Age = 23 }
            //};
        }

        public List<Employee> GetAll()
        {
            List<Employee> ObjEmployeeList = new List<Employee>();
            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "udp_SelectAllEmployee";
                ObjSqlConnection.Open();

                var ObjSqlDataReader = ObjSqlCommand.ExecuteReader();
                if (ObjSqlDataReader.HasRows)
                {
                    Employee ObjEmployee = null;
                    while (ObjSqlDataReader.Read()) 
                    {
                        ObjEmployee = new Employee();
                        ObjEmployee.Id = ObjSqlDataReader.GetInt32(0);
                        ObjEmployee.Name = ObjSqlDataReader.GetString(1);
                        ObjEmployee.Age = ObjSqlDataReader.GetInt32(2);

                        ObjEmployeeList.Add(ObjEmployee);
                    }
                }
                ObjSqlDataReader.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally 
            {
                ObjSqlConnection.Close();
            }
            return ObjEmployeeList;
        }

        public bool Add(Employee ObjNewemployee)
        {
            bool IsAdded = false;
            // For selected age group
            if (ObjNewemployee.Age < 21 || ObjNewemployee.Age > 60)
                throw new ArgumentException("Invalid Age Limit for Employee");
            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "udp_InsertEmployee";

                ObjSqlCommand.Parameters.AddWithValue("@Id", ObjNewemployee.Id);
                ObjSqlCommand.Parameters.AddWithValue("@Name", ObjNewemployee.Name);
                ObjSqlCommand.Parameters.AddWithValue("@Age", ObjNewemployee.Age);

                ObjSqlConnection.Open();

                int NoOfRowsAffected = ObjSqlCommand.ExecuteNonQuery();
                IsAdded = NoOfRowsAffected > 0;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally 
            {
                ObjSqlConnection.Close();
            }

            //ObjEmployeeList.Add(objNewemployee);
            return IsAdded;
        }

        public bool Update(Employee ObjEmployeeUpdate)
        {
            bool IsUpdated = false;

            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "udp_UpdateEmployee";

                ObjSqlCommand.Parameters.AddWithValue("@Id", ObjEmployeeUpdate.Id);
                ObjSqlCommand.Parameters.AddWithValue("@Name", ObjEmployeeUpdate.Name);
                ObjSqlCommand.Parameters.AddWithValue("@Age", ObjEmployeeUpdate.Age);

                ObjSqlConnection.Open();

                int NoOfRowsAffected = ObjSqlCommand.ExecuteNonQuery();
                IsUpdated = NoOfRowsAffected > 0;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }

            //for (int index = 0; index < ObjEmployeeList.Count; index++)
            //{
            //    if (ObjEmployeeList[index].Id == objEmployeeUpdate.Id)
            //    {
            //        ObjEmployeeList[index].Name = objEmployeeUpdate.Name;
            //        ObjEmployeeList[index].Age = objEmployeeUpdate.Age;
            //        IsUpdated = true;
            //        break;
            //    }
            //}
            return IsUpdated;
        }

        public bool Delete(int id)
        {
            bool IsDeleted = false;

            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "udp_DeleteEmployee";

                ObjSqlCommand.Parameters.AddWithValue("@Id", id);
                
                ObjSqlConnection.Open();

                int NoOfRowsAffected = ObjSqlCommand.ExecuteNonQuery();
                IsDeleted = NoOfRowsAffected > 0;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }

            //for (int index = 0; index < ObjEmployeeList.Count; index++)
            //{
            //    if (ObjEmployeeList[index].Id == id)
            //    {
            //        ObjEmployeeList.RemoveAt(index);
            //        IsDeleted = true;
            //        break;
            //    }
            //}
            return IsDeleted;
        }

        public Employee Search(int id)
        {
            Employee ObjEmployee = null;

            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "udp_SelectAllEmployeebyId";
                ObjSqlCommand.Parameters.AddWithValue("@Id",id);

                ObjSqlConnection.Open();
                var ObjSqlDataReader = ObjSqlCommand.ExecuteReader();
                if (ObjSqlDataReader.HasRows) 
                {
                    ObjSqlDataReader.Read();
                    ObjEmployee= new Employee();
                    ObjEmployee.Id = ObjSqlDataReader.GetInt32(0);
                    ObjEmployee.Name = ObjSqlDataReader.GetString(1);
                    ObjEmployee.Age = ObjSqlDataReader.GetInt32(2);
                }
                ObjSqlDataReader.Close();

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally 
            { 
                ObjSqlConnection.Close();
            
            }

            return ObjEmployee;
            //return ObjEmployeeList.FirstOrDefault(e => e.Id == id);
        }

    }
}
