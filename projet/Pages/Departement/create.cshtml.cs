using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace projet.Pages.Departement
{
    public class CreateModel : PageModel
    {
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {

        }

        public void OnPost()
        {
            string nom = Request.Form["nom"].FirstOrDefault();
            string numChef = Request.Form["numChef"].FirstOrDefault();
            string numAgendaDept = Request.Form["numAgendaDept"].FirstOrDefault();

            if (!IsValidName(nom))
            {
                errorMessage = "Invalid nom.";
                return;
            }

            if (!IsValidNumber(numAgendaDept))
            {
                errorMessage = "Invalid numAgendaDept.";
                return;
            }

            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=agenda-projet;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Departement (num, nom, numChef, numAgendaDept) VALUES (NEXT VALUE FOR seqDEPARTEMENT, @nom, @numChef, @numAgendaDept)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@nom", nom);
                        command.Parameters.AddWithValue("@numChef", numChef);
                        command.Parameters.AddWithValue("@numAgendaDept", numAgendaDept);
                        command.ExecuteNonQuery();
                    }
                }

                successMessage = "New Department added correctly.";
                Response.Redirect("/Departement/Index");
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

        private bool IsValidName(string input)
        {
            return !Regex.IsMatch(input, @"\d");
        }
    }
}
