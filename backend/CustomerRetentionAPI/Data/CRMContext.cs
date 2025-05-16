using Microsoft.EntityFrameworkCore;
using CustomerRetentionAPI.Models;

public class CRMContext : DbContext
{
    public CRMContext(DbContextOptions<CRMContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerChurnData> CustomerChurnData { get; set; }
    public DbSet<CustomerChurnPrediction> CustomerChurnPredictions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerChurnData>()
            .ToTable("customerchurndata")
            .HasKey(c => c.CustomerChurnDataId);

        modelBuilder.Entity<CustomerChurnPrediction>()
            .ToTable("customerchurnprediction")
            .HasKey(c => c.CustomerChurnPredictionId);
        
        modelBuilder.Entity<CustomerChurnData>()
            .Property(c => c.CustomerId)
            .HasColumnName("customerid"); // Map to the correct column name

        modelBuilder.Entity<CustomerChurnData>()
            .Property(c => c.Tenure)
            .HasColumnName("tenure");

        modelBuilder.Entity<CustomerChurnData>()
            .Property(c => c.OrderCount)
            .HasColumnName("ordercount");

        modelBuilder.Entity<CustomerChurnData>()
            .Property(c => c.DaysSinceLastOrder)
            .HasColumnName("dayssincelastorder");

        modelBuilder.Entity<CustomerChurnData>()
            .Property(c => c.CashbackAmount)
            .HasColumnName("cashbackamount");

        modelBuilder.Entity<CustomerChurnData>()
            .Property(c => c.SatisfactionScore)
            .HasColumnName("satisfactionscore");

        modelBuilder.Entity<CustomerChurnData>()
            .Property(c => c.Complain)
            .HasColumnName("complain");

        modelBuilder.Entity<CustomerChurnData>()
            .Property(c => c.HourSpendOnApp)
            .HasColumnName("hourspendonapp");

        modelBuilder.Entity<CustomerChurnData>()
            .Property(c => c.NumberOfDeviceRegistered)
            .HasColumnName("numberofdeviceregistered");

        modelBuilder.Entity<CustomerChurnPrediction>()
            .ToTable("customerchurnprediction")
            .HasKey(c => c.CustomerId);
        
        modelBuilder.Entity<CustomerChurnPrediction>()
            .Property(c => c.CustomerId)
            .HasColumnName("customerid"); // If applicable
            
        modelBuilder.Entity<CustomerChurnPrediction>()
            .Property(c => c.Prediction)
            .HasColumnName("prediction");

        modelBuilder.Entity<CustomerChurnPrediction>()
            .Property(c => c.PredictionProbability)
            .HasColumnName("predictionprobability");

        modelBuilder.Entity<CustomerChurnPrediction>()
            .Property(c => c.CreatedAt)
            .HasColumnName("createdat");

        modelBuilder.Entity<CustomerChurnData>()
            .Property(c => c.CreatedAt)
            .HasColumnName("createdat");

        modelBuilder.Entity<CustomerChurnPrediction>()
            .Property(c => c.CustomerChurnPredictionId)
            .HasColumnName("customerchurnpredictionid");

        
        modelBuilder.Entity<CustomerChurnData>()
            .Property(c => c.CustomerChurnDataId)
            .HasColumnName("customerchurndataid");
    }
}
