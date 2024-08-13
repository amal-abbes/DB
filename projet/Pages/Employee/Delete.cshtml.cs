using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace projet.Pages.Employee
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public int numEmploye { get; set; }

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            // Laisser la méthode OnGet vide pour afficher le formulaire de suppression de l'employé
        }

        public void OnPost()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=agenda-projet;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "DELETE FROM EMPLOYE WHERE numEmploye = @numEmploye";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@numEmploye", numEmploye);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            errorMessage = "Employee not found.";
                            return;
                        }
                    }
                }

                successMessage = "Employee deleted successfully.";
                Response.Redirect("/Employee/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }
    }
}