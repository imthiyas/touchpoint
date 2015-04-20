using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchPointWindows.DataModel
{
    public class MenuModel : NotifyBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string ChildName { get; set; }
        public string Name { get; set; }
        public Double Vat { get; set; }
        public Double Price { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
