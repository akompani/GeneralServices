using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralServices.Models
{
    public class CultureResource
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(5)]
        public string CultureName { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Value { get; set; }
    }
}
