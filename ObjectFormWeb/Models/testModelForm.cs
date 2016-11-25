using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ObjectForm.Attribute;

namespace ObjectFormWeb.Models
{
    public class TestModelForm
    {
        public string Name { get; set; }

        [GridColumn("Age")]
        public int Age { get; set; }

        public float Salary { get; set; }
        [Required]
        public long? Nid { get; set; }


        [IsSelect]
        [GridColumn("Gender")]
        public int? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime? JoinDate { get; set; }

        public virtual IList<Responcibility> Responcibility { get; set; }
    }


    public class Responcibility
    {
        public string JobName { get; set; }
        public int Count { get; set; }
    }
}