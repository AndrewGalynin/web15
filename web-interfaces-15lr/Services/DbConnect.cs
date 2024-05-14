using Microsoft.EntityFrameworkCore;

public class DbConnect : DbContext
{
    public DbSet<clients2> clients2 { get; set; }
    public DbConnect(DbContextOptions<DbConnect> options) : base(options)
    {
    }
}

public class clients2
{
    public int Id { get; set; }
    public string brand { get; set; }
    public string model { get; set; }
    public int price { get; set; }
    public string order_code { get; set; }
    public bool Check { get; set; }
}
