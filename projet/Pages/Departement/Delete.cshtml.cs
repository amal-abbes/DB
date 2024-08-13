using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace projet.Pages.Departement
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public int num { get; set; }

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            // Laisser la méthode OnGet vide pour afficher le formulaire de suppression du département
        }

        public void OnPost()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=agenda-projet;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM Departement WHERE num = @num";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@num", num);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            errorMessage = "Department not found.";
                            return;
                        }
                    }
                }

                successMessage = "Department deleted successfully.";
                Response.Redirect("/Departement/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }
    }
}
