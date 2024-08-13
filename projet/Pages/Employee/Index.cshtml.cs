using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace projet.Pages.Employee
{
	public class IndexModel : PageModel
	{
		public List<Employee> ListEmployee = new ();
		public void OnGet()
		{
			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=agenda-projet;Integrated Security=True";
				using (SqlConnection connection = new(connectionString))
					try
					{
						connection.Open();
						String sql = "SELECT * FROM EMPLOYE";
						using (SqlCommand command = new(sql, connection))
						{ 
							
								using (SqlDataReader reader = command.ExecuteReader())
								{
									while (reader.Read())
									{
										Employee employee = new();
										employee.numEmploye = reader.GetInt32(0);
										employee.nom = reader.GetString(1);
										employee.prenom = reader.GetString(2);
										employee.telIntern = reader.GetString(3);
										employee.email = reader.GetString(4);
										employee.niveau = reader.GetString(5);
										employee.numDept = reader.GetInt32(6);
									    ListEmployee.Add(employee);


									}

								}

							
					     }

					}
					catch (Exception ) 
					{ Console.WriteLine("connection :" ); }
				

			}
	
			catch (Exception ex)
			{
				Console.WriteLine("Exception :" + ex.ToString());

			}
		}
	}
	public class Employee
	{
		public int numEmploye ;
		public string nom ;
		public string prenom;
		public string telIntern;
		public string email;
		public string niveau;
		public int numDept;


	}
}
