using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using Cumulative_3.Models;

namespace Cumulative_3.Controllers
{
    public class TeachersController : ApiController
    {
        
        private CumulativeProjectDb Teachers = new CumulativeProjectDb();
        
        [HttpGet]
        [Route("api/Teachers/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            
            MySqlConnection Conn = Teachers.AccessDatabase();

            
            Conn.Open();

            
            MySqlCommand cmd = Conn.CreateCommand();

            
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat (teacherfname, ' ', teacherlname)) like lower(@key)";

           
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            
            MySqlDataReader Results = cmd.ExecuteReader();

            
            List<Teacher> TeachersInfo = new List<Teacher> { };


            
            while (Results.Read())
            {
                
                DateTime HireDate = (DateTime)Results["hiredate"];
                decimal Salary = (decimal)Results["salary"];
                string EmployeeNumber = (string)Results["employeenumber"];
                int TeacherId = (int)Results["teacherid"];
                string TeacherFname = (string)Results["teacherfname"];
                string TeacherLname = (string)Results["teacherlname"];

                
                Teacher NewTeacher = new Teacher();

               
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

                TeachersInfo.Add(NewTeacher);
            }

            
            Conn.Close();


            
            return TeachersInfo;


        }


        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            
            MySqlConnection Conn = Teachers.AccessDatabase();

           
            Conn.Open();

            
            MySqlCommand cmd = Conn.CreateCommand();

            
            cmd.CommandText = "Select * from teachers where teacherid = " + id;

            
            MySqlDataReader Results = cmd.ExecuteReader();

           
            while (Results.Read())
            {
                
                DateTime HireDate = (DateTime)Results["hiredate"];
                decimal Salary = (decimal)Results["salary"];
                string EmployeeNumber = (string)Results["employeenumber"];
                int TeacherId = (int)Results["teacherid"];
                string TeacherFname = (string)Results["teacherfname"];
                string TeacherLname = (string)Results["teacherlname"];

                
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;
            }

            
            return NewTeacher;
        }

        
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            MySqlConnection Conn = Teachers.AccessDatabase();

            
            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            
            cmd.CommandText = "Delete from teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        
        [HttpPost]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            MySqlConnection Conn = Teachers.AccessDatabase();

            Conn.Open();

            
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate,salary) values (@TeacherFname,@TeacherLname,@EmployeeNumber,CURRENT_DATE(),@Salary)";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();


            cmd.ExecuteNonQuery();

            Conn.Close();
        }

       
        public void UpdateTeacher(int id, [FromBody] Teacher TeacherInfo)
        {
            MySqlConnection Conn = Teachers.AccessDatabase();

            
            Conn.Open();

            
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "update teachers set teacherfname=@TeacherFname, teacherlname=@TeacherLname, employeenumber=@EmployeeNumber,salary=@Salary where teacherid=@TeacherId";
            cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
            cmd.Parameters.AddWithValue("@Salary", TeacherInfo.Salary);
            cmd.Parameters.AddWithValue("@TeacherId", id);
            cmd.Prepare();


            cmd.ExecuteNonQuery();

            Conn.Close();

        }

    }
}