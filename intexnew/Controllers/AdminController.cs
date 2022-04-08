using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using intexnew.Models;
using intexnew.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace intexnew.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private ICrashRepository repo { get; set; }

        public AdminController(ICrashRepository temp)
        {
            repo = temp;
        }

        //********************
        //CRUD****************
        //********************

        [HttpGet]
        public IActionResult Add()
        {
            return View("Form");
        }

        [HttpPost]
        public IActionResult Add(Crash crash)
        {
            repo.AddCrash(crash);
            return RedirectToAction("CrashList");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var form = repo.Crashes.Single(x => x.CRASH_ID == id);
            return View("Form", form);
        }

        [HttpPost]
        public IActionResult Edit(Crash crash)
        {
            repo.UpdateCrash(crash);

            return RedirectToAction("CrashList");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var delete = repo.Crashes.Single(x => x.CRASH_ID == id);
            repo.DeleteCrash(delete);

            return RedirectToAction("CrashList");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            return RedirectToAction("CrashList");
        }

        //CRASHLIST

        [HttpGet]
        public IActionResult CrashList()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CrashList(int severity = 0, int pageNum = 1)
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

            return View("CrashList", x);
        }
    }
}
