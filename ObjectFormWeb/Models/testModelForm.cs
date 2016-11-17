using System;
using System.Collections.Generic;
using ObjectForm;

namespace ObjectFormWeb.Models
{
    public class TestModelForm
    {
        public string Name { get; set; }

        [GridColumn("Age", false)]
        public int Age { get; set; }

        [GridColumn("Gender", true)]
        public int? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime? JoinDate { get; set; }

        public IList<Responcibility> Responcibility { get; set; }
    }


    public class Responcibility
    {
        public string JobName { get; set; }
        public int Count { get; set; }
    }
}