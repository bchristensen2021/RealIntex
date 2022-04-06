using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using intexnew.Models;
using intexnew.Models.ViewModels;

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
            //var blah = repo.Crashes
            //    .ToList();
            return View();
        }


        //public IActionResult CrashCardsIndex()
        //{
        //    return View();
        //}

        



        [HttpGet]
        public IActionResult Add()
        {
            return View("");
        }
        [HttpPost]



        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult CrashCardsIndex(string road, int pageNum = 1)
        {

            int pageSize = 10;

            var x = new CrashesViewModel
            {
                Crashes = repo.Crashes
                .Where(r => r.MAIN_ROAD_NAME == road || road == null)
                .OrderBy(r => r.MAIN_ROAD_NAME)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),
                PageInfo = new PageInfo
                {
                    TotalNumCrashes =
                    (road == null
                        ? repo.Crashes.Count()
                        : repo.Crashes.Where(x => x.MAIN_ROAD_NAME == road).Count()),
                    CrashesPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };

            return View(x);
        }
    }
}
