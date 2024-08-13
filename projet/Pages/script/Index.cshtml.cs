using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace projet.Pages.script
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string SqlQuery { get; set; }

        public List<string> SqlQueryColumns { get; set; }
        public List<List<string>> SqlQueryRows { get; set; }
        public string SqlQueryResult { get; set; }

        public IndexModel()
        {
            SqlQueryColumns = new List<string>();
            SqlQueryRows = new List<List<string>>();
        }

        public void OnPost()
        {
            if (!string.IsNullOrEmpty(SqlQuery))
            {
                // Ex�cuter la requ�te SQL et r�cup�rer les r�sultats
                using (SqlConnection connection = new SqlConnection("Data Source=.\\sqlexpress;Initial Catalog=agenda-projet;Integrated Security=True"))
                {
                    try
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(SqlQuery, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        bool hasRows = reader.HasRows;

                        if (hasRows)
                        {
                            SqlQueryColumns = GetColumnNames(reader);
                            SqlQueryRows = GetRows(reader);

                            SqlQueryResult = "Requ�te ex�cut�e avec succ�s !";
                        }
                        else
                        {
                            SqlQueryResult = "La requ�te n'a retourn� aucun r�sultat.";
                        }
                    }
                    catch (SqlException ex)
                    {
                        // G�rer l'erreur de requ�te SQL
                        SqlQueryResult = FormatSqlErrorMessage(ex);
                    }
                }
            }
        }

        private List<string> GetColumnNames(SqlDataReader reader)
        {
            List<string> columnNames = new List<string>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                columnNames.Add(reader.GetName(i));
            }

            return columnNames;
        }

        private List<List<string>> GetRows(SqlDataReader reader)
        {
            List<List<string>> rows = new List<List<string>>();

            while (reader.Read())
            {
                List<string> row = new List<string>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader[i].ToString());
                }

                rows.Add(row);
            }

            return rows;
        }

        private string FormatSqlErrorMessage(SqlException ex)
        {
            string errorMessage = "Erreur de requ�te SQL : \n";

            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorMessage += $"Num�ro d'erreur : {ex.Errors[i].Number}\n";
                errorMessage += $"Gravit� : {ex.Errors[i].Class}\n";
                errorMessage += $"Message : {ex.Errors[i].Message}\n";
                errorMessage += $"Nom de la proc�dure : {ex.Errors[i].Procedure}\n";
                errorMessage += $"Ligne : {ex.Errors[i].LineNumber}\n";
                errorMessage += "\n";
            }

            return errorMessage;
        }
    }
}
