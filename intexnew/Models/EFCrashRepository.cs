using System;
using System.Linq;

namespace intexnew.Models
{
    public class EFCrashRepository : ICrashRepository
    {
        private CrashesDbContext Context { get; set; }

        public EFCrashRepository(CrashesDbContext temp)
        {
            Context = temp;
        }

        public IQueryable<Crash> Crashes => Context.Crashes;


        //adding functionality from ICrashRepository
        public void SaveCrash(Crash c)
        {
            Context.SaveChanges();
        }

        public void AddCrash(Crash c)
        {
            Context.Add(c);
            Context.SaveChanges();
        }

        public void DeleteCrash(Crash c)
        {
            Context.Remove(c);
            Context.SaveChanges();
        }
        
        public void UpdateCrash(Crash c)
        {
            Context.Update(c);
            Context.SaveChanges();
        }
    }
}
