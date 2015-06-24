﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using OneManBlog.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OneManBlog.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        static readonly List<TodoItem> _items = new List<TodoItem>()
        {
            new TodoItem { Id = 1, Title = "Do Laundry" }
        };
        
        [HttpGet]
        public IEnumerable<TodoItem> GetAllToDoItems()
        {
            return _items;
        }

        [HttpGet("{id}", Name = "GetByTodoItemById")]
        public IActionResult Get(int id)
        {
            var item = _items.FirstOrDefault(todoItem => todoItem.Id == id);

            if (item == null)
            {
                return HttpNotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public void CreateTodoItem([FromBody] TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                Context.Response.StatusCode = 400;
            }
            else
            {
                item.Id = 1 + _items.Max(x => (int?)x.Id) ?? 0;
                _items.Add(item);

                string url = Url.RouteUrl("GetByTodoItemById", new { id = item.Id },
                    Request.Scheme, Request.Host.ToUriComponent());

                Context.Response.StatusCode = 201;
                Context.Response.Headers["Location"] = url;
            }
        }

        [HttpPost]
        public void UpdateTodoItem([FromBody] TodoItem item)
        {
            var indexOfTodoItemToUpdate = _items.FindIndex(todoItem => todoItem.Id == item.Id);

            if(indexOfTodoItemToUpdate != -1)
            {
                _items[indexOfTodoItemToUpdate] = item;
                Context.Response.StatusCode = 200;
            }

            Context.Response.StatusCode = 400;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodoItem(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            _items.Remove(item);
            return new HttpStatusCodeResult(204);
        }
    }
}