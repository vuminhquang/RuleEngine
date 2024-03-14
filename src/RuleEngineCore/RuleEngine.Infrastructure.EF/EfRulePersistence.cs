using System.Data;
using Microsoft.EntityFrameworkCore;
using RuleEngine.Domain.Services;

namespace RuleEngine.Infrastructure.EF;

public class EfRulePersistence(RuleDbContext dbContext) : IRulePersistence
{
    public IEnumerable<Abstraction.Entities.Rule> GetRules()
    {
        // return dbContext.Rules
        //     .Include(r => r.Condition)
        //     .Include(r => r.Operations)
        //     // .ThenInclude(o => o.Rule)
        //     .ToList();
        return dbContext.Rules.ToList();
    }

    public void SaveRule(Abstraction.Entities.Rule rule)
    {
        if (rule.Id == 0)
        {
            dbContext.Rules.Add(rule);
        }
        else
        {
            dbContext.Entry(rule).State = EntityState.Modified;
        }

        dbContext.SaveChanges();
    }

    public void DeleteRule(int ruleId)
    {
        var rule = dbContext.Rules.Find(ruleId);
        if (rule == null) return;
        dbContext.Rules.Remove(rule);
        dbContext.SaveChanges();
    }
}