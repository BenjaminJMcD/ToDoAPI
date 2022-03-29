using System;
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
    [EnableCors(origins:"*", headers:"*", methods:"*")]
    public class ToDoController : ApiController
    {
        ToDoEntities db = new ToDoEntities();

        //GET - api/ToDo
        public IHttpActionResult GetToDos()
        {
            List<ToDoViewModel> todo = db.ToDoItems.Include("Category").Select(t => new ToDoViewModel()
            {
                ToDoId = t.ToDoId,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.Category.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }
            }).ToList<ToDoViewModel>();

            if (todo.Count == 0)
            {
                return NotFound();
            }

            return Ok(todo);
        }//end GetToDo

        //GET = api/todo/id
        public IHttpActionResult GetToDo(int id)
        {
            ToDoViewModel todo = db.ToDoItems.Include("Category").Where(t => t.ToDoId == id).Select(t => new ToDoViewModel()
            {
                ToDoId = t.ToDoId,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.Category.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }
            }).FirstOrDefault();

            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        //POST - api/ToDo
        public IHttpActionResult PostToDo(ToDoViewModel todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            ToDoItem newtodo = new ToDoItem()
            {
                Action = todo.Action,
                Done = todo.Done,
                CategoryId = todo.CategoryId
            };

            db.ToDoItems.Add(newtodo);
            db.SaveChanges();

            return Ok(newtodo);

        }//End PostToDo

        //PUT - api/todo
        public IHttpActionResult PutToDo(ToDoViewModel todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            ToDoItem oldToDo = db.ToDoItems.Where(t => t.ToDoId == todo.ToDoId).FirstOrDefault();

            if (oldToDo != null)
            {
                oldToDo.ToDoId = todo.ToDoId;
                oldToDo.Action = todo.Action;
                oldToDo.CategoryId = todo.CategoryId;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }//End PUTToDo

        public IHttpActionResult DeleteToDo(int id)
        {
            ToDoItem todo = db.ToDoItems.Where(t => t.ToDoId == id).FirstOrDefault();

            if (todo != null)
            {
                db.ToDoItems.Remove(todo);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }// end DELETE

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }//end class
}//end namespace
