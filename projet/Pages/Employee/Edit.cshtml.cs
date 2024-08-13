using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace projet.Pages.Employee
{
    public class EditModel : PageModel
    {
        public Employee employee = new Employee();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {

        }

        public void OnPost(int numEmploye, string nom, string prenom, string telIntern, string email, string niveau, int numDept)
        {
            if (!IsValidNumber(telIntern))
            {
                errorMessage = "Invalid telephone number.";
                return;
            }

            if (!IsValidInteger(niveau))
            {
                errorMessage = "Invalid niveau.";
                return;
            }

            if (!IsValidName(nom))
            {
                errorMessage = "Invalid nom.";
                return;
            }

            if (!IsValidName(prenom))
            {
                errorMessage = "Invalid prenom.";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=agenda-projet;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE EMPLOYE SET nom = @nom, prenom = @prenom, telIntern = @telIntern, email = @email, niveau = @niveau , numDept=@numDept WHERE numEmploye = @numEmploye";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@numEmploye", numEmploye);
                        command.Parameters.AddWithValue("@nom", nom);
                        command.Parameters.AddWithValue("@prenom", prenom);
                        command.Parameters.AddWithValue("@telIntern", telIntern);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@niveau", niveau);
                        command.Parameters.AddWithValue("@numDept", numDept);
                        command.ExecuteNonQuery();
                    }
                }

                successMessage = "Employee updated successfully.";
                Response.Redirect("/Employee/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        private bool IsValidNumber(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }

        private bool IsValidInteger(string input)
        {
            return int.TryParse(input, out _);
        }

        private bool IsValidName(string input)
        {
            return !Regex.IsMatch(input, @"\d");
        }
    }
}
