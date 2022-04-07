using System;
using System.Linq;

namespace intexnew.Models
{
    public interface ICrashRepository
    {
        IQueryable<Crash> Crashes { get; }

        //included to allow CRUD from administration end
        public void SaveCrash(Crash C);
        public void AddCrash(Crash C);
        public void DeleteCrash(Crash C);
        public void UpdateCrash(Crash C);
    }
}
