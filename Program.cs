using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Librarian;

class Program
{
    static void Main(string[] args)
    {
        List<Book> books;
        Console.WriteLine("Welcome to Librarian!");
        string loadAnswer = null;
        while (loadAnswer != "Y" && loadAnswer != "N")
        {
            Console.WriteLine("Would you like to load a library? (Y/N)");
            loadAnswer = Console.ReadLine().ToUpper();

            if (loadAnswer != "Y" && loadAnswer != "N")
            {
                Console.WriteLine("Answer Y(es) or N(o) !");
            }
        }

        if (loadAnswer == "Y")
        {
            books = LoadLibrary();
        }
        else
        {
            Console.WriteLine("Creating new library");
            books = new List<Book>();
        }

        string topMenuSelect = null;


        while (topMenuSelect != "Q")
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine($"(A)dd   (R)emove    (B)rowse   (Q)uit");
            topMenuSelect = Console.ReadLine().ToUpper();

            if (topMenuSelect == "A")
            {
                AddBook(books);
                SaveLibrary(books);
            }
            else if (topMenuSelect == "R")
            {
                RemoveBook(books);
                SaveLibrary(books);
            }
            else if (topMenuSelect == "B")
            {
                BrowseBooks();
            }
            else if (topMenuSelect != "Q")
            {
                Console.WriteLine("Choose a valid option!");
            }
        }

        static void SaveLibrary(List<Book> books)
        {
            string json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("library.json", json);
            Console.WriteLine("Library saved to file!");
        }

        static List<Book> LoadLibrary()
        {
            if (!File.Exists("library.json"))
            {
                string createAnswer = null;
                while (createAnswer != "Y" && createAnswer != "N")
                {
                    Console.WriteLine("No saved library found. Start a new one? (Y/N)");
                    createAnswer = Console.ReadLine().ToUpper();

                    if (createAnswer != "Y" && createAnswer != "N")
                    {
                        Console.WriteLine("Answer Y(es) or N(o) !");
                    }
                }

                if (createAnswer == "Y")
                {
                    Console.WriteLine("Creating new library");
                    return new List<Book>();
                }
                else if (createAnswer == "N")
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                    return null;
                }
            }
            string json = File.ReadAllText("library.json");
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(json);
            Console.WriteLine("Library loaded from file.");
            return books;
        }

        static void AddBook(List<Book> books)
        {
            Console.WriteLine("Adding new book");
            Console.WriteLine("Enter title:");
            string title = Console.ReadLine();
            Console.WriteLine("Enter author:");
            string author = Console.ReadLine();
            int yearInt = 0;
            bool isNumber = false;
            while (!isNumber)
            {
                Console.WriteLine("Enter year of publication:");
                string yearString = Console.ReadLine();
                isNumber = int.TryParse(yearString, out yearInt);
                if (!isNumber) Console.WriteLine("Enter a valid year!");
            }
            Console.WriteLine("Enter genre:");
            string genre = Console.ReadLine().ToLower();
            
            Book newBook = new Book(title, author, yearInt, genre);
            books.Add(newBook);
        }

        static void RemoveBook(List<Book> books)
        {
            Console.WriteLine("Removing book, enter search parameters. (Blank space skips search criteria)");
            Console.WriteLine("Enter title:");
            string title = Console.ReadLine();
            Console.WriteLine("Enter author:");
            string author = Console.ReadLine();
            int yearInt = 0;
            string yearString;
            bool isNumber = false;
            while (!isNumber)
            {
                Console.WriteLine("Enter year of publication:");
                yearString = Console.ReadLine();
                if (yearString == "") break;
                isNumber = int.TryParse(yearString, out yearInt);
                if (!isNumber) Console.WriteLine("Enter a valid year!");
            }
            Console.WriteLine("Enter genre:");
            string genre = Console.ReadLine().ToLower();
        }

        static Book FindBook(string name, List<Book> books)
        {
            foreach (Book book in books)
            {
                if (book.Title == name) return book;
            }
                Console.WriteLine("No books match your search!");
                return null; 
        }

        static void BrowseBooks()
        {

        }
    }
}
