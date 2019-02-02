using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KeyStoreService.Models
{
    public class KeyInfo
    {

        public int Id { get; set; }

        [MaxLength(25)]
        public string Alias { get; set; }

        [Required]
        public string Key { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }
}
