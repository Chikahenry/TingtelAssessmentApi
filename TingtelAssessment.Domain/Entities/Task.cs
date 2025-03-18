using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskStatus = TingtelAssessment.Domain.Enums.TaskStatus;

namespace TingtelAssessment.Domain.Entities
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime DueDate { get; set; }
    }
}
