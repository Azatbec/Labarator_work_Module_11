using System;
using System.Collections.Generic;

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public bool IsAvailable { get; set; }

    public Book(string title, string author, string isbn)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        IsAvailable = true;
    }

    public void MarkAsLoaned()
    {
        IsAvailable = false;
    }

    public void MarkAsAvailable()
    {
        IsAvailable = true;
    }
}

public class Reader
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public Reader(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public void BorrowBook(Book book)
    {
        if (book.IsAvailable)
        {
            book.MarkAsLoaned();
            Console.WriteLine($"{Name} borrowed the book '{book.Title}'.");
        }
        else
        {
            Console.WriteLine($"The book '{book.Title}' is not available.");
        }
    }

    public void ReturnBook(Book book)
    {
        book.MarkAsAvailable();
        Console.WriteLine($"{Name} returned the book '{book.Title}'.");
    }
}

public class Librarian
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }

    private List<Book> books = new List<Book>();
    private List<Reader> readers = new List<Reader>();

    public Librarian(int id, string name, string position)
    {
        Id = id;
        Name = name;
        Position = position;
    }

    public void AddBook(Book book)
    {
        books.Add(book);
        Console.WriteLine($"Book '{book.Title}' added to the library.");
    }

    public void RemoveBook(Book book)
    {
        books.Remove(book);
        Console.WriteLine($"Book '{book.Title}' removed from the library.");
    }

    public void AddReader(Reader reader)
    {
        readers.Add(reader);
        Console.WriteLine($"Reader '{reader.Name}' added.");
    }

    public void RemoveReader(Reader reader)
    {
        readers.Remove(reader);
        Console.WriteLine($"Reader '{reader.Name}' removed.");
    }
}

public class Loan
{
    public Book Book { get; set; }
    public Reader Reader { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime ReturnDate { get; set; }

    public Loan(Book book, Reader reader)
    {
        Book = book;
        Reader = reader;
        LoanDate = DateTime.Now;
    }

    public void IssueLoan()
    {
        Console.WriteLine($"Loan issued for book '{Book.Title}' to reader '{Reader.Name}' on {LoanDate}.");
    }

    public void CompleteLoan()
    {
        ReturnDate = DateTime.Now;
        Book.MarkAsAvailable();
        Console.WriteLine($"Loan completed for book '{Book.Title}' returned by reader '{Reader.Name}' on {ReturnDate}.");
    }
}
class Program
{
    static void Main(string[] args)
    {
        // Создание объектов
        Book book1 = new Book("1984", "George Orwell", "12345");
        Book book2 = new Book("To Kill a Mockingbird", "Harper Lee", "67890");

        Reader reader1 = new Reader(1, "John Doe", "john.doe@example.com");
        Reader reader2 = new Reader(2, "Jane Smith", "jane.smith@example.com");

        Librarian librarian = new Librarian(1, "Alice Johnson", "Head Librarian");

        // Добавление книг и читателей
        librarian.AddBook(book1);
        librarian.AddBook(book2);
        librarian.AddReader(reader1);
        librarian.AddReader(reader2);

        // Выдача книги
        reader1.BorrowBook(book1);

        // Возврат книги
        reader1.ReturnBook(book1);

        // Завершение выдачи
        Loan loan = new Loan(book1, reader1);
        loan.IssueLoan();
        loan.CompleteLoan();

        Console.ReadKey();
    }
}
