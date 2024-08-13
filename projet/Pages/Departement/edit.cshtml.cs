using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace projet.Pages.Departement
{
    public class EditModel : PageModel
    {
        public Department departement = new Department();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {

        }

        public void OnPost(int num, string nom, string numChef, int numAgendaDept)
        {
            if (!IsValidName(nom))
            {
                errorMessage = "Invalid nom.";
                return;
            }

            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=agenda-projet;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Departement SET nom = @nom, numChef = @numChef, numAgendaDept = @numAgendaDept WHERE num = @num";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@num", num);
                        command.Parameters.AddWithValue("@nom", nom);
                        command.Parameters.AddWithValue("@numChef", numChef);
                        command.Parameters.AddWithValue("@numAgendaDept", numAgendaDept);
                        command.ExecuteNonQuery();
                    }
                }

                successMessage = "Department updated successfully.";
                Response.Redirect("/Departement/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        private bool IsValidName(string input)
        {
            return !Regex.IsMatch(input, @"\d");
        }
    }
}
