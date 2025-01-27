using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Query_Management_App.Models;

namespace Query_Management_App.Controllers
{
    public class QueryController : Controller
    {
        private readonly QueryDbContext _context;

        public QueryController(QueryDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
          

            //var queries = _context.Queries.ToList();
            List<Query> queries = _context.Queries.ToList();
            // Make sure that the `queries` variable is not null
            if (queries == null)
            {
                queries = new List<Query_Management_App.Models.Query>();// Initialize it to an empty list if null
            }
            return View(queries);
        }

        public IActionResult Raise() => View();

        [HttpPost]
        public IActionResult Raise(string requestor)
        {
            var query = new Query
            {
                Requestor = requestor,
                Status = "Raised",
                CreatedAt = DateTime.Now
            };
            _context.Queries.Add(query);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Perform(int id)
        {
            var query = _context.Queries.Find(id);
            if (query != null)
            {
                query.Status = "In Progress";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Resolve(int id)
        {
            var query = _context.Queries.Find(id);
            if (query != null)
            {
                query.Status = "Resolved";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult OnHold(int id)
        {
            var query = _context.Queries.Find(id);
            if (query != null)
            {
                query.Status = "On Hold";
                query.OnHoldUntil = DateTime.Now.AddDays(3);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Cancel(int id)
        {
            var query = _context.Queries.Find(id);
            if (query != null)
            {
                query.Status = "Cancelled";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult ExpireOnHold()
        {
            var expiredQueries = _context.Queries
                .Where(q => q.Status == "On Hold" && q.OnHoldUntil <= DateTime.Now)
                .ToList();

            foreach (var query in expiredQueries)
            {
                query.Status = "Raised";
                query.OnHoldUntil = null;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
