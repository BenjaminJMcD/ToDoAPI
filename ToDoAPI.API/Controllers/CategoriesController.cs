﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.API.Models;
using ToDoAPI.Data.EF;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class CategoriesController : ApiController
    {
        ToDoEntities db = new ToDoEntities();

        //GET - api/categories
        public IHttpActionResult GetCagetories()
        {
            List<CategoryViewModel> cats = db.Categories.Select(c => new CategoryViewModel()
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
            }).ToList<CategoryViewModel>();

            if (cats.Count == 0)
                return NotFound();

            return Ok(cats);

        }//end GETCategories

        public IHttpActionResult GetCategory(int id)
        {
            CategoryViewModel cat = db.Categories.Where(c => c.CategoryId == id).Select(c => new CategoryViewModel()
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
            }).FirstOrDefault();

            if (cat == null)
            {
                return NotFound();
            }

            return Ok(cat);
        }//end GETCATEGORY

        //POST api/categories
        public IHttpActionResult PostCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            db.Categories.Add(new Category()
            {
                Name = cat.Name,
                Description = cat.Description
            });

            db.SaveChanges();
            return Ok();
        }//end POSTCATEGORY

        //PUT - api/categories
        public IHttpActionResult PutCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            Category existingCat = db.Categories.Where(c => c.CategoryId == cat.CategoryId).FirstOrDefault();

            if (existingCat != null)
            {
                existingCat.Name = cat.Name;
                existingCat.Description = cat.Description;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//End PUTCATEGORY

        //DELETE /api/categories
        public IHttpActionResult DeleteCategory(int id)
        {
            Category cat = db.Categories.Where(c => c.CategoryId == id).FirstOrDefault();

            if (cat != null)
            {
                db.Categories.Remove(cat);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//END DELETE

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



    }//end CLASS
}//end NAMESPACE
