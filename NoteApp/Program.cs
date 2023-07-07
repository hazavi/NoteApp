using System;
using System.Collections.Generic;
using System.IO;

public class Note
{
    public string Title { get; set; }
    public string Content { get; set; }
}

public class NoteManager
{
    private List<Note> notes;
    private string filePath;

    public NoteManager(string filePath)
    {
        this.filePath = filePath;
        notes = new List<Note>();
        LoadNotes();
    }

    public List<Note> GetNotes()
    {
        return notes;
    }

    public void AddOrUpdateNote(Note note)
    {
        Note existingNote = notes.Find(n => n.Title == note.Title);

        if (existingNote != null)
        {
            existingNote.Content = note.Content;
        }
        else
        {
            notes.Add(note);
        }

        SaveNotes();
    }

    public void DeleteNote(Note note)
    {
        notes.Remove(note);
        SaveNotes();
    }

    private void LoadNotes()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 2)
                {
                    Note note = new Note
                    {
                        Title = parts[0],
                        Content = parts[1]
                    };
                    notes.Add(note);
                }
            }
        }
    }

    private void SaveNotes()
    {
        List<string> lines = new List<string>();

        foreach (Note note in notes)
        {
            string line = $"{note.Title}|{note.Content}";
            lines.Add(line);
        }

        File.WriteAllLines(filePath, lines);
    }
}

public class Program
{
    private static NoteManager noteManager;
    private static string filePath = "notes.txt";

    public static void Main()
    {
        noteManager = new NoteManager(filePath);
        Console.WriteLine("Welcome to the Note App!");

        while (true)
        {
            Console.WriteLine("\nSelect an option:");
            Console.WriteLine("1. View Notes");
            Console.WriteLine("2. Add Note");
            Console.WriteLine("3. Edit Note");
            Console.WriteLine("4. Delete Note");
            Console.WriteLine("5. Exit");

            Console.Write("Choice: ");
            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ViewNotes();
                    break;
                case "2":
                    AddNote();
                    break;
                case "3":
                    EditNote();
                    break;
                case "4":
                    DeleteNote();
                    break;
                case "5":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void ViewNotes()
    {
        List<Note> notes = noteManager.GetNotes();

        if (notes.Count > 0)
        {
            Console.WriteLine("Notes:");
            foreach (Note note in notes)
            {
                Console.WriteLine($"- {note.Title}: {note.Content}");
            }
        }
        else
        {
            Console.WriteLine("No notes found.");
        }
    }

    private static void AddNote()
    {
        Console.Write("Enter note title: ");
        string title = Console.ReadLine();
        Console.Write("Enter note content: ");
        string content = Console.ReadLine();

        Note note = new Note
        {
            Title = title,
            Content = content
        };

        noteManager.AddOrUpdateNote(note);
        Console.WriteLine("Note added successfully.");
    }

    private static void EditNote()
    {
        Console.Write("Enter note title to edit: ");
        string title = Console.ReadLine();

        Note note = noteManager.GetNotes().Find(n => n.Title == title);

        if (note != null)
        {
            Console.WriteLine($"Current content: {note.Content}");
            Console.Write("Enter new content: ");
            string newContent = Console.ReadLine();

            note.Content = newContent;
            noteManager.AddOrUpdateNote(note);
            Console.WriteLine("Note updated successfully.");
        }
        else
        {
            Console.WriteLine("Note not found.");
        }
    }

    private static void DeleteNote()
    {
        Console.Write("Enter note title to delete: ");
        string title = Console.ReadLine();

        Note note = noteManager.GetNotes().Find(n => n.Title == title);

        if (note != null)
        {
            noteManager.DeleteNote(note);
            Console.WriteLine("Note deleted successfully.");
        }
        else
        {
            Console.WriteLine("Note not found.");
        }
    }
}
