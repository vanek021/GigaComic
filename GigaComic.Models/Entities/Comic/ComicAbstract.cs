using GigaComic.Core.Entities;
using System;
using System.Collections.Generic;
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
    }
}
