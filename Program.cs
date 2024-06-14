using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dapper_2
{
    public class Book
    {
        public int id { get; set; }
        public string name {  get; set; }   
        public string author { get; set; }
        public override string ToString()
        {
            return $"Id: {id}, Name: {name}, Technology: {author}";
        }
    }

    internal class Program
    {
        static public void PrintDB(SqlConnection conn)
        {
            var sqlQuery = $"Select * from Library3";
            var data = conn.Query<Book>(sqlQuery);
            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
        }

        static public void AddBooks(SqlConnection conn, List<Book> books)
        {
            var sqlQuery = $@"insert into Library3 (name, author) values (@{nameof(Book.name)}, @{nameof(Book.author)})";
            conn.Execute(sqlQuery, books);
        }

        static public void UpdateBook(SqlConnection conn, int id, string newAuthor)
        {
            var sqlQuery = $@"Update Library3 set author = @Author where id = @Id";
            conn.Execute(sqlQuery, new { Author = newAuthor, Id = id });
        }

        static public void DeleteBooks(SqlConnection conn)
        {
            var sqlQuery = "delete from Library3";
            conn.Execute(sqlQuery);
        }
       
        static void Main(string[] args)
        {
            string connectionString = @"Server=DESKTOP-0BAM1G2\SQLEXPRESS;Database=Library3;Integrated Security=True;";
            using (var db = new SqlConnection(connectionString))
            {
                db.Open();

                Console.WriteLine("---------");
                Console.WriteLine("Initial Database State:");
                Console.WriteLine("---------");
                PrintDB(db);

                List<Book> books = new List<Book>
            {
                new Book { name = "book1", author = "author1" },
                new Book { name = "book2", author = "author1" },
                new Book { name = "book3", author = "author1" },
                new Book { name = "book4", author = "author1" }
            };

                Console.WriteLine("---------");
                Console.WriteLine("After Insert");
                Console.WriteLine("---------");
                AddBooks(db, books);
                PrintDB(db);

                Console.WriteLine("---------");
                Console.WriteLine("After Update");
                Console.WriteLine("---------");
                UpdateBook(db, 4, "Vanya");
                PrintDB(db);

                Console.WriteLine("---------");
                Console.WriteLine("After Delete");
                Console.WriteLine("---------");
                DeleteBooks(db);
                PrintDB(db);

                db.Close();
            }
    }
}
