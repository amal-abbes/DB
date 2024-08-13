using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace projet.Pages.Employee
{
    public class createModel : PageModel
    {
        public Employee employee = new();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {

        }

        public void OnPost()
        {
            string nom = Request.Form["nom"].FirstOrDefault();
            string prenom = Request.Form["prenom"].FirstOrDefault();
            string telIntern = Request.Form["telIntern"].FirstOrDefault();
            string email = Request.Form["email"].FirstOrDefault();
            string niveau = Request.Form["niveau"].FirstOrDefault();

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

            if (!IsValidEmail(email))
            {
                errorMessage = "Invalid email.";
                return;
            }

            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=agenda-projet;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO EMPLOYE (numEmploye, nom, prenom, telIntern, email, niveau, numDept) VALUES (NEXT VALUE FOR seqEMPLOYE, @nom, @prenom, @telIntern, @email, @niveau, 2)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@nom", nom);
                        command.Parameters.AddWithValue("@prenom", prenom);
                        command.Parameters.AddWithValue("@telIntern", telIntern);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@niveau", niveau);
                        command.ExecuteNonQuery();
                    }
                }

                employee.nom = "";
                employee.prenom = "";
                employee.telIntern = "";
                employee.email = "";
                employee.niveau = "";

                successMessage = "New Employee added correctly.";
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

        private bool IsValidEmail(string input)
        {
            return Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}