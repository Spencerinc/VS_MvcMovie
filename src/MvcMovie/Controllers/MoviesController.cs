using System.Linq;
using Microsoft.AspNet.Mvc;
using MvcMovie.Models;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Microsoft.AspNet.Mvc.Rendering;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        private List<Movie> movieList = new List<Movie>();

        public Movie movie1 = new Movie();
        private Movie movie2 = new Movie();
        private Movie movie3 = new Movie();
    
        public void fakeData1()
        {
            movie1.ID = 1;
            movie1.Title = "Movie One";
            movie1.Genre = "Comedy";
            movie1.ReleaseDate = new DateTime(1776, 7, 4);
            movie1.Price = 1.00M;

            movie2.ID = 2;
            movie2.Title = "Movie Two";
            movie2.Genre = "Action";
            movie2.ReleaseDate = new DateTime(2000, 1, 1);
            movie2.Price = 23.99M;

            movie3.ID = 3;                  
            movie3.Title = "Movie Three";                       
            movie3.Genre = "Drama";                       
            movie3.ReleaseDate = new DateTime(2016, 1, 13);            
            movie3.Price = 5.00M;

            movieList.Add(movie1);
            movieList.Add(movie2);
            movieList.Add(movie3);
        }

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Movies
        public IActionResult Index(string movieGenre, string searchString)
        {
            fakeData1();

            Debug.WriteLine("\n CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC\n    searchString = >" + searchString + "<\n    movieGenre = >" + movieGenre + "<");

            //return View(_context.Movie.ToList());
            var GenreQry = from m in movieList
                           orderby m.Genre
                           select m.Genre;

            var GenreList = new List<string>();
            GenreList.AddRange(GenreQry.Distinct());
            ViewData["movieGenre"] = new SelectList(GenreList);

            Debug.WriteLine("Genre count is " +GenreList.Count);

            // var movies = from m in movieList select m;
            var movieListShort = from m in movieList select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movieListShort = movieListShort.Where(s => s.Title.Contains(searchString));  // s => s.Title is a Lambda Expression. This is case sensitive.
            }

            if(!string.IsNullOrEmpty(movieGenre))
            {
                movieListShort = movieListShort.Where(x => x.Genre == movieGenre);
            }

            return View(movieListShort);
        }

        // GET: Movies/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Movie movie = _context.Movie.Single(m => m.ID == id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID,Title,ReleaseDate,Genre,Price")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Movie.Add(movie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        // This method is called from a button click within the browser
        public IActionResult Edit(int? id)
        {
            Debug.WriteLine("\n  AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n    id = " + id);
            fakeData1();

            if (id == null)
            {
                return HttpNotFound();
            }

            //Movie movie = _context.Movie.Single(m => m.ID == id);
            Movie movie = new Movie();
            int count = movieList.Count;

            for(int i=0; i<count; i++)
            {
                if(id.Equals(movieList.ElementAt(i).ID))
                {
                    movie = movieList.ElementAt(i);
                    break;
                }
            }
                       
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("ID,Title,ReleaseDate,Genre,Price")] Movie movie)
        {
            Debug.WriteLine("\n BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB\n    movie => " + movie);
            fakeData1();

            if (ModelState.IsValid)
            {
                _context.Update(movie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Movie movie = _context.Movie.Single(m => m.ID == id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed([Bind("ID,Title,ReleaseDate,Genre,Price")] int id)
        {
            Movie movie = _context.Movie.Single(m => m.ID == id);
            _context.Movie.Remove(movie);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
