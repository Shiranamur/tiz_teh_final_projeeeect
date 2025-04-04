﻿namespace BlazorApp1.Models;

public class Session
{
    public string SessionId { get; set; } = null!;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    
    public virtual User? User { get; set; }
}