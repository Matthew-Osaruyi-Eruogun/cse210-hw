using System;
using System.Collections.Generic;

// Comment class to track commenter name and comment text
class Comment
{
    public string CommenterName { get; set; }
    public string Text { get; set; }

    public Comment(string commenterName, string text)
    {
        CommenterName = commenterName;
        Text = text;
    }
}

// Video class tracks title, author, length, and stores list of comments
class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthSeconds { get; set; }
    private List<Comment> Comments { get; }

    public Video(string title, string author, int lengthSeconds)
    {
        Title = title;
        Author = author;
        LengthSeconds = lengthSeconds;
        Comments = new List<Comment>();
    }

    // Method to add a comment to this video
    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }

    // Method to return the number of comments
    public int GetNumberOfComments()
    {
        return Comments.Count;
    }

    // Method to return the list of comments
    public List<Comment> GetComments()
    {
        return Comments;
    }
}

// Main Program class to create videos, add comments, store in list, and display details
class Program
{
    static void Main()
    {
        // Create videos
        var video1 = new Video("Unboxing Latest Smartphone", "TechGuru", 540);
        var video2 = new Video("Top 10 Travel Destinations", "Wanderlust", 720);
        var video3 = new Video("DIY Home Improvement Tips", "HandyMan", 660);
        var video4 = new Video("Cooking 101: Easy Recipes", "Foodie", 480);

        // Add comments for video1
        video1.AddComment(new Comment("Alice", "Great insight on the phone features!"));
        video1.AddComment(new Comment("Bob", "I love the camera quality review."));
        video1.AddComment(new Comment("Charlie", "Thanks for the detailed unboxing."));

        // Add comments for video2
        video2.AddComment(new Comment("Diana", "These places are amazing!"));
        video2.AddComment(new Comment("Ethan", "Adding these to my bucket list!"));
        video2.AddComment(new Comment("Fiona", "Beautiful shots and commentary."));
        video2.AddComment(new Comment("George", "I visited 3 of these spots, amazing experience."));

        // Add comments for video3
        video3.AddComment(new Comment("Helen", "Very helpful tips, thanks!"));
        video3.AddComment(new Comment("Ian", "Just tried some of these, easy to follow."));
        video3.AddComment(new Comment("Jane", "I love this channel!"));

        // Add comments for video4
        video4.AddComment(new Comment("Kevin", "Yummy recipes, can’t wait to try!"));
        video4.AddComment(new Comment("Laura", "Perfect for beginners."));
        video4.AddComment(new Comment("Mike", "Could you make a vegetarian version?"));
        video4.AddComment(new Comment("Nina", "Simple and delicious!"));

        // Store videos in a list
        List<Video> videos = new List<Video> { video1, video2, video3, video4 };

        // Iterate videos and display details
        foreach (var video in videos)
        {
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length (seconds): {video.LengthSeconds}");
            Console.WriteLine($"Number of comments: {video.GetNumberOfComments()}");
            Console.WriteLine("Comments:");
            foreach (var comment in video.GetComments())
            {
                Console.WriteLine($" - {comment.CommenterName}: {comment.Text}");
            }
            Console.WriteLine(); // Blank line to separate videos
        }
    }
}
