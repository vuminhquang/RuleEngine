using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RuleEngine.Abstraction.Entities;

namespace RuleEngine.Infrastructure.EF;

public class RuleDbContext(DbContextOptions<RuleDbContext> options) : DbContext(options)
{
    public DbSet<Rule> Rules { get; set; }
    public DbSet<Operation> Operations { get; set; }
    public DbSet<Expression> Expressions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expression>()
            .HasDiscriminator<ExpressionType>(e => e.Type)
            .HasValue<ConstantExpression>(ExpressionType.Constant)
            .HasValue<FieldExpression>(ExpressionType.Field)
            .HasValue<ArithmeticExpression>(ExpressionType.Arithmetic)
            .HasValue<ComparisonExpression>(ExpressionType.Comparison)
            .HasValue<LogicalExpression>(ExpressionType.Logical);

        modelBuilder.Entity<ArithmeticExpression>()
            .HasOne(e => e.LeftExpression)
            .WithMany()
            .HasForeignKey(e => e.LeftExpressionId);

        modelBuilder.Entity<ArithmeticExpression>()
            .HasOne(e => e.RightExpression)
            .WithMany()
            .HasForeignKey(e => e.RightExpressionId);

        modelBuilder.Entity<ComparisonExpression>()
            .HasOne(e => e.LeftExpression)
            .WithMany()
            .HasForeignKey(e => e.LeftExpressionId);

        modelBuilder.Entity<ComparisonExpression>()
            .HasOne(e => e.RightExpression)
            .WithMany()
            .HasForeignKey(e => e.RightExpressionId);

        modelBuilder.Entity<LogicalExpression>()
            .HasMany(e => e.OperandExpressions)
            .WithOne()
            .HasForeignKey(e => e.RuleId);
        
        
        // Value Conversions
        modelBuilder.Entity<ConstantExpressionValue>()
            .HasKey(v => v.Id);
        
        modelBuilder.Entity<ConstantExpressionValue>()
            .Property(v => v.Value)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject(v, typeof(object))
            );

        modelBuilder.Entity<ConstantExpressionValue>()
            .Property(v => v.ValueType)
            .HasConversion(
                v => v.AssemblyQualifiedName,
                v => Type.GetType(v)
            );
        
        modelBuilder.Entity<OperationValue>()
            .Property(v => v.Value)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject(v, typeof(object))
            );

        modelBuilder.Entity<OperationValue>()
            .Property(v => v.ValueType)
            .HasConversion(
                v => v.AssemblyQualifiedName,
                v => Type.GetType(v)
            );
    }
}