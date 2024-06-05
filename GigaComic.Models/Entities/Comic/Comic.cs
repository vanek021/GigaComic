using GigaComic.Core.Entities;
using GigaComic.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Models.Entities.Comic
{
    public class Comic : BaseRecord
    {
        public string Title { get; set; }
        public ComicStage Stage { get; set; }
        public ComicStyle Style { get; set; }
        public ComicGrid Grid { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long UserId { get; set; }

        public virtual List<ComicAbstract> ComicAbstracts { get; set; }
    }
}
