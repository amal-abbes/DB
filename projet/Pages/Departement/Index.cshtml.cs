using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace projet.Pages.Departement
{
    public class IndexModel : PageModel
    {
        public List<Department> ListDepartement { get; set; }

        public void OnGet()
        {
            ListDepartement = GetDepartments();
        }

        private List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();

            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=agenda-projet;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Departement";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Department department = new Department();
                                department.num = Convert.ToInt32(reader["num"]);
                                department.nom = Convert.ToString(reader["nom"]);
                                department.numChef = Convert.ToString(reader["numChef"]);
                                department.numAgendaDept = Convert.ToInt32(reader["numAgendaDept"]);
                                departments.Add(department);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérer l'erreur
            }

            return departments;
        }
    }

    public class Department
    {
        public int num;
        public string nom;
        public string numChef;
        public int numAgendaDept;
    }
}
