namespace Librarian
{
    public class Book : IComparable<Book>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int PubYear { get; set; }
        public string Genre { get; set; }

        public int CompareTo(Book book)
        {
            int comparison = this.Genre.CompareTo(book.Genre);
            return comparison;
        }
        
        public Book(string title, string author, int pubYear, string genre)
        {
            this.Title = title;
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