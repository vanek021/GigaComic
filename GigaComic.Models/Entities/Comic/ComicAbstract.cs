using GigaComic.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Models.Entities.Comic
{
    public class ComicAbstract : BaseRecord
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string? Plot { get; set; }

        [ForeignKey(nameof(ComicId))]
        public Comic Comic { get; set; }
        public long ComicId { get; set; }
    }
}
