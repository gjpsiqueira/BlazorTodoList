using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Data
{
    public class TodoItem
    {

        public Guid id { get; set; }
        [Required]
        public string description { get; set; }
        public bool isComplete { get; set; }
        public DateTime dateCreated { get; set; }
    }
}
