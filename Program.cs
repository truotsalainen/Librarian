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
        string? loadAnswer = null;
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

        string? topMenuSelect = null;


        while (topMenuSelect != "Q")
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine($"(A)dd   (R)emove    (S)earch    (B)rowse by genre   (L)ist all books   (Q)uit");
            topMenuSelect = Console.ReadLine().ToUpper();

            if (topMenuSelect == "A")
            {
                AddBook(books);
                SaveLibrary(books);
                Console.WriteLine();
            }
            else if (topMenuSelect == "S")
            {

                List<Book> searchResults = FindBook(books);
                if (searchResults == null)
                {
                    Console.WriteLine("No books match your query!");
                    Console.WriteLine();
                }
                else
                {
                    int bookIndex = 1;
                    Console.WriteLine($"{searchResults.Count} book(s) match your query:");
                    foreach (Book book in searchResults)
                    {
                        Console.WriteLine($"{bookIndex}. {book.ToString()}");
                        bookIndex++;
                    }
                    Console.WriteLine();
                }
            }
            else if (topMenuSelect == "R")
            {
                RemoveBook(books);
                SaveLibrary(books);
                Console.WriteLine();
            }
            else if (topMenuSelect == "B")
            {
                List<Book> booksByGenre = BrowseBooks(books);
                if (booksByGenre == null)
                {
                    Console.WriteLine($"No books in library!");
                    Console.WriteLine();
                }
                else
                {
                    int bookIndex = 1;
                    foreach (Book book in booksByGenre)
                    {
                        Console.WriteLine($"{bookIndex}. {book.ToString()}");
                        bookIndex++;
                    }
                }
            }
            else if (topMenuSelect == "L")
            {
                Console.WriteLine($"{books.Count} items in library:");
                ListAllBooks(books);
                Console.WriteLine();
            }
            else if (topMenuSelect != "Q")
            {
                Console.WriteLine("Choose a valid option!");
                Console.WriteLine();
            }
        }
        Console.WriteLine("See you later!");
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
            string? createAnswer = null;
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
        List<Book>? books = JsonSerializer.Deserialize<List<Book>>(json);
        Console.WriteLine("Library loaded from file.");
        return books;
    }

    static void AddBook(List<Book> books)
    {
        Console.WriteLine("Adding new book");
        Console.WriteLine("Enter title:");
        string title = Console.ReadLine().Trim();
        Console.WriteLine("Enter author:");
        string author = Console.ReadLine().Trim();
        int yearInt = 0;
        bool isNumber = false;
        while (!isNumber)
        {
            Console.WriteLine("Enter year of publication:");
            string yearString = Console.ReadLine().Trim();
            isNumber = int.TryParse(yearString, out yearInt);
            if (!isNumber) Console.WriteLine("Enter a valid year!");
        }
        Console.WriteLine("Enter genre:");
        string genre = Console.ReadLine().ToLower().Trim();

        Book newBook = new Book(title, author, yearInt, genre);

        foreach (Book book in books)
        {
            if (newBook.Title == book.Title && newBook.Author == book.Author)
            {
                Console.WriteLine("This book already exists!");
                return;
            }
        }
        books.Add(newBook);
        Console.WriteLine($"{newBook.Title} by {newBook.Author} successfully added to library!");
    }

    static void RemoveBook(List<Book> books)
    {
        Book bookToRemove;
        Console.WriteLine("Removing book, enter search parameters:");
        List<Book> hits = FindBook(books);
        if (hits == null)
        {
            Console.WriteLine("No books match your query!");
            return;
        }
        else if (hits.Count == 1)
        {
            bookToRemove = hits[0];
            books.Remove(bookToRemove);
            Console.WriteLine($"{hits[0].ToString()} removed from library!");
        }
        else
        {
            int bookIndex = 1;
            Console.WriteLine("Multiple books found:");
            foreach (Book book in hits)
            {
                Console.WriteLine($"{bookIndex}: {book.ToString()}");
                bookIndex++;
            }
            Console.WriteLine($"Choose book to remove: (1-{(bookIndex - 1)}, 0 to cancel)");
            int removeAtIndex = (Convert.ToInt32(Console.ReadLine()) - 1);
            if (removeAtIndex == -1)
            {
                return;
            }
            else if (removeAtIndex > hits.Count)
            {
                Console.WriteLine("Choose a valid option!");
            }
            else
            {
                bookToRemove = hits[removeAtIndex];
                books.Remove(bookToRemove);
                Console.WriteLine($"{hits[removeAtIndex].ToString()} removed from library!");
            }
        }
    }

    //Searches library for a book by title and author, returns a list of books that match the criteria
    static List<Book> FindBook(List<Book> books)
    {
        while (true)
        {
            Console.WriteLine("Enter title:");
            string title = Console.ReadLine().Trim();
            Console.WriteLine("Enter author:");
            string author = Console.ReadLine().Trim();
            List<Book> hits = new List<Book>();
            if (title == null && author == null)
            {
                Console.WriteLine("Enter at least one parameter for your search!");
            }
            else if (title != "" && author != "")
            {
                foreach (Book book in books)
                {

                    if (book.Title == title && book.Author == author)
                    {
                        hits.Add(book);
                    }
                }
                if (hits.Count() == 0)
                {
                    return null;
                }
                else return hits;
            }
            else if (title == "")
            {
                foreach (Book book in books)
                {

                    if (book.Author == author)
                    {
                        hits.Add(book);
                    }
                }
                if (hits.Count() == 0)
                {
                    return null;
                }
                else return hits;
            }
            else
            {
                foreach (Book book in books)
                {

                    if (book.Title == title)
                    {
                        hits.Add(book);
                    }
                }
                if (hits.Count() == 0)
                {
                    return null;
                }
                else return hits;
            }
        }
    }


    static void ListAllBooks(List<Book> books)
    {
        int bookCount = 1;
        foreach (Book book in books)
        {
            Console.WriteLine(bookCount + ". " + book.ToString());
            bookCount++;
        }
    }



    static List<Book> BrowseBooks(List<Book> books)
    {
        List<Book> booksByGenre = books;
        booksByGenre.Sort();
        return booksByGenre;
    }
}
        
