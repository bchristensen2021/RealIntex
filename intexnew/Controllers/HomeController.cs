using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using intexnew.Models;
using intexnew.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace intexnew.Controllers
{
    public class HomeController : Controller
    {
        private ICrashRepository repo { get; set; }

        public HomeController(ICrashRepository temp)
        {
            repo = temp;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CrashCardsIndex(int severity = 0, int pageNum = 1)
        {

            int pageSize = 25;

            var x = new CrashesViewModel
            {
                Crashes = repo.Crashes
                .Where(r => r.CRASH_SEVERITY_ID == severity || severity == 0)
                .OrderBy(r => r.CRASH_SEVERITY_ID)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                PageInfo = new PageInfo
                {
                    TotalNumCrashes =
                    (severity == 0
                        ? repo.Crashes.Count()
                        : repo.Crashes.Where(x => x.CRASH_SEVERITY_ID == severity).Count()),
                    //TotalNumCrashes = repo.Crashes.Where(x => x.CRASH_SEVERITY_ID == severity).Count(),
                    //TotalNumCrashes = repo.Crashes.Count(),
                    CrashesPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };

            return View(x);
        }
    }
}
