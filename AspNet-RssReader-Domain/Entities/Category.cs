using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet_RssReader_Domain.Entities
{
    public class Category
    {
        public Category()
        {
            Sources = new HashSet<Source>();
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Source> Sources { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
