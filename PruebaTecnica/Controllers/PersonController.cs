﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PruebaTecnica.Models;

namespace PruebaTecnica.Controllers
{
    public class PersonController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private static IList<Person> _Person;

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persons"></param>
        [NonAction]
        public static void setPersons(IList<Person> persons)
        {
            _Person = persons;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public static IList<Person> getPerson()
        {
            return _Person;
        }

        /// <summary>
        /// Load Index
        /// </summary>
        /// <param name="n"></param>
        /// <param name="c"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IActionResult Index(string n = "", string c = "", int page = 1)
        {
            try
            {
                // Get Persons
                var data = _Person;

                var result = new PagedResult<Person>();
                if (data == null)
                {
                    result.CurrentPage = 0;
                    result.PageSize = 0;
                    result.RowCount = 0;
                    result.PageCount = 0;
                    result.Results = new List<Person>();
                    return View(result);
                }

                // Contains Name
                if (!string.IsNullOrEmpty(n))
                {
                    data = data.Where(x => x.Name.Contains(n, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Contains City
                if (!string.IsNullOrEmpty(c))
                {
                    data = data.Where(x => x.City.Contains(c, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                int pageSize = 15;

                // Apply the pagination
                result.CurrentPage = page;
                result.PageSize = pageSize;
                result.RowCount = data.Count();

                var pageCount = (double)result.RowCount / pageSize;
                result.PageCount = (int)Math.Ceiling(pageCount);

                var skip = (page - 1) * pageSize;
                result.Results = data.Skip(skip).Take(pageSize).ToList();

                return View(result);
            }
            catch (Exception)
            {
                return Error();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
