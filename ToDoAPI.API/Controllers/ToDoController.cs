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
        public IHttpActionResult GetToDoItems()
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



    }
}
