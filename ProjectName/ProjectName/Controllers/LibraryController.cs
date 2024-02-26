using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace MyNamespace.Controllers
{
    public class LibraryController : Controller
    {
        private readonly string booksConfigPath = "BooksConfig.json";
        private readonly string usersConfigPath = "UsersConfig.json";

        public IActionResult Welcome()
        {
            return Content("Welcome to the Library!");
        }

        public IActionResult Books(string genre = null)
        {
            try
            {
                // Чтение списка книг из файла конфигурации
                var booksConfigContent = System.IO.File.ReadAllText(booksConfigPath);
                var booksConfig = JObject.Parse(booksConfigContent);

                // Если жанр не указан или указан пустой, возвращаем все книги
                if (string.IsNullOrWhiteSpace(genre))
                {
                    var allBooks = booksConfig["Books"].Select(b => b["Title"].ToString());
                    return Content("List of books: " + string.Join(", ", allBooks));
                }
                else
                {
                    // Фильтруем книги по указанному жанру
                    var filteredBooks = booksConfig["Books"]
                        .Where(b => b["Genre"].ToString().Equals(genre, StringComparison.OrdinalIgnoreCase))
                        .Select(b => b["Title"].ToString());

                    if (filteredBooks.Any())
                    {
                        return Content($"List of {genre} books: " + string.Join(", ", filteredBooks));
                    }
                    else
                    {
                        return Content($"No {genre} books found.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        public IActionResult Profile(int? id)
        {
            try
            {
                if (id.HasValue && id >= 0 && id <= 5)
                {
                    // Чтение информации о пользователях из файла конфигурации
                    var usersConfigContent = System.IO.File.ReadAllText(usersConfigPath);
                    var usersConfig = JObject.Parse(usersConfigContent);

                    // Поиск пользователя по id
                    var user = usersConfig.SelectToken($"$.Users[?(@.Id == {id})]");

                    if (user != null)
                    {
                        return Content($"User ID: {user["Id"]}\nName: {user["Name"]}\nAge: {user["Age"]}");
                    }
                    else
                    {
                        return Content("User not found");
                    }
                }
                else
                {
                    // Чтение информации о пользователе с id = 0
                    var defaultUser = GetUserById(0);

                    if (defaultUser != null)
                    {
                        return Content($"User ID: {defaultUser["Id"]}\nName: {defaultUser["Name"]}\nAge: {defaultUser["Age"]}");
                    }
                    else
                    {
                        return Content("Default user not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Метод для получения пользователя по id из файла конфигурации
        private JObject GetUserById(int id)
        {
            var usersConfigContent = System.IO.File.ReadAllText(usersConfigPath);
            var usersConfig = JObject.Parse(usersConfigContent);
            var user = usersConfig.SelectToken($"$.Users[?(@.Id == {id})]");
            return user as JObject;
        }
    }
}
