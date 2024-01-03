using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using ToDoList.Models;
using ToDoList.Models.ViewModels;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(GetAllTodos());
        }

        internal TodoViewModel GetAllTodos()
        {
            List<Todo> todos = [];
            using SQLiteConnection con = new("Data Source=db.sqlite");
            using var tableCmd = con.CreateCommand();

            con.Open();
            tableCmd.CommandText = "SELECT * FROM TODO";

            try
            {
                using SQLiteDataReader rdr = tableCmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        todos.Add(
                            new Todo
                            {
                                Id = rdr.GetInt32(0),
                                Name = rdr.GetString(1)
                            });
                    }
                }
                else
                {
                    return new TodoViewModel { Todos = todos };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new TodoViewModel { Todos = todos };
        }

        [HttpPost]
        public RedirectResult Insert(Todo todo)
        {
            using SQLiteConnection con = new("Data Source=db.sqlite");
            using var tableCmd = con.CreateCommand();

            con.Open();
            tableCmd.CommandText = $"INSERT INTO TODO (Name) VALUES (@Name)";
            tableCmd.Parameters.AddWithValue("@Name", todo.Name);

            try
            {
                tableCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Redirect("http://localhost:5222/");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using SQLiteConnection con = new("Data Source=db.sqlite");
            using var tableCmd = con.CreateCommand();

            con.Open();
            tableCmd.CommandText = $"DELETE FROM TODO WHERE Id = @Id";
            tableCmd.Parameters.AddWithValue("@Id", id);

            try
            {
                tableCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Json(new {});
        }

        [HttpGet]
        public JsonResult PopulateForm(int id)
        {
            return Json(GetById(id));
        }

        internal Todo GetById(int id)
        {
            Todo todo = new();
            using SQLiteConnection con = new("Data Source=db.sqlite");
            using var tableCmd = con.CreateCommand();

            con.Open();
            tableCmd.CommandText = $"SELECT * FROM TODO WHERE Id = @Id";
            tableCmd.Parameters.AddWithValue("@Id", id);

            try
            {
                using SQLiteDataReader rdr = tableCmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        todo = new Todo
                        {
                            Id = rdr.GetInt32(0),
                            Name = rdr.GetString(1)
                        };
                    }
                }
                else
                {
                    return todo;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return todo;
        }

        public RedirectResult Update(Todo todo)
        {
            using SQLiteConnection con = new("Data Source=db.sqlite");
            using var tableCmd = con.CreateCommand();

            con.Open();
            tableCmd.CommandText = $"UPDATE TODO SET Name = @Name WHERE Id = @Id";
            tableCmd.Parameters.AddWithValue("@Id", todo.Id);
            tableCmd.Parameters.AddWithValue("@Name", todo.Name);

            try
            {
                tableCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Redirect("http://localhost:5222/");
        }
    }
}
