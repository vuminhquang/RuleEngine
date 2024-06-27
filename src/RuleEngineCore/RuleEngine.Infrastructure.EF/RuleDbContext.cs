using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RuleEngine.Abstraction.Entities;
using RuleEngine.Domain.Ext.RemoteFieldExpr;

namespace RuleEngine.Infrastructure.EF;

public class RuleDbContext : DbContext
{
    public RuleDbContext(DbContextOptions<RuleDbContext> options) : base(options) { }

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
            .HasValue<LogicalExpression>(ExpressionType.Logical)
            .HasValue<ForEachExpression>(ExpressionType.ForEach)
            .HasValue<FilteredCollectionExpression>(ExpressionType.FilteredCollection)
            .HasValue<SwitchExpression>(ExpressionType.Switch);

        modelBuilder.Entity<ArithmeticExpression>()
            .HasOne<Expression>(e => e.LeftExpression)
            .WithMany()
            .HasForeignKey(e => e.LeftExpressionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ArithmeticExpression>()
            .HasOne<Expression>(e => e.RightExpression)
            .WithMany()
            .HasForeignKey(e => e.RightExpressionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ComparisonExpression>()
            .HasOne<Expression>(e => e.LeftExpression)
            .WithMany()
            .HasForeignKey(e => e.LeftExpressionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ComparisonExpression>()
            .HasOne<Expression>(e => e.RightExpression)
            .WithMany()
            .HasForeignKey(e => e.RightExpressionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LogicalExpression>()
            .HasMany(e => e.OperandExpressions)
            .WithOne()
            .HasForeignKey(e => e.RuleId);

        modelBuilder.Entity<ForEachExpression>()
            .HasOne<Expression>(e => e.CollectionExpression)
            .WithMany()
            .HasForeignKey(e => e.CollectionExpressionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ForEachExpression>()
            .HasOne<Expression>(e => e.ConditionExpression)
            .WithMany()
            .HasForeignKey(e => e.ConditionExpressionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ForEachExpression>()
            .HasOne<Expression>(e => e.ActionExpression)
            .WithMany()
            .HasForeignKey(e => e.ActionExpressionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FilteredCollectionExpression>()
            .HasOne<Expression>(e => e.Condition)
            .WithMany()
            .HasForeignKey(e => e.ConditionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FilteredCollectionExpression>()
            .Ignore(e => e.Collection);

        modelBuilder.Entity<SwitchExpression>()
            .HasOne<Expression>(e => e.Condition)
            .WithMany()
            .HasForeignKey(e => e.ConditionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SwitchExpression>()
            .Ignore(e => e.Cases)
            .Ignore(e => e.DefaultCase);

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