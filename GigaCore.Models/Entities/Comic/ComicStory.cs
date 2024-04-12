using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Models.Entities.Comic
{
    public class ComicStory
    {
        [ForeignKey(nameof(AbstractId))]
        public ComicAbstract Abstract { get; set; }
        public long AbstractId { get; set; }
    }
}
