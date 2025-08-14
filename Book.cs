namespace Librarian
{
    public class Book
    {
        public string Title { get; private set; }
        public string Author { get; private set; }
        public int PubYear { get; private set; }
        public string Genre { get; private set; }

        public Book(string name, string author, int pubYear, string genre)
        {
            this.Title = name;
            this.Author = author;
            this.PubYear = pubYear;
            this.Genre = genre;
        }
        public override string ToString()
        {
            return $"{this.Title} by {this.Author}, {this.PubYear}, {this.Genre}";
        }
    }

}