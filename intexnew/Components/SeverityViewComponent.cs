using System;
using System.Linq;
using intexnew.Models;
using Microsoft.AspNetCore.Mvc;


namespace intexnew.Components
{
    public class SeverityViewComponent :ViewComponent
    {
        private ICrashRepository repo { get; set; }

        public SeverityViewComponent(ICrashRepository temp)
        {
            repo = temp;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectCategory = RouteData?.Values["Severity"];
            var types = repo.Crashes
                .Select(x => x.CRASH_SEVERITY_ID)
                .Distinct()
                .OrderBy(x => x);

            return View(types);
        }

    }
}
