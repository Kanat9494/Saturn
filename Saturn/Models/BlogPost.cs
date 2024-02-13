namespace Saturn.Models;

public class BlogPost
{
    public BlogPost(int blogId, string title, string profileImageUrl, string blogImageUrl, string content)
    {
        BlogId = blogId;
        Title = title;
        ProfileImageUrl = profileImageUrl;
        BlogImageUrl = blogImageUrl;
        Content = content;
    }
    public int BlogId { get; set; } 
    public string Title { get; set; }
    public string ProfileImageUrl { get; set; }
    public string BlogImageUrl { get; set; }
    public string Content { get; set; }
}
