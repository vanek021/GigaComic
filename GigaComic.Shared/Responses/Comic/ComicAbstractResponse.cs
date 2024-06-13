using GigaComic.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Shared.Responses.Comic
{
    public class ComicAbstractResponse : IResponse, IDraggable
    {
        public long Id { get; set; }

        public int Order { get; set; }

        [Required(ErrorMessage = "Имя тезиса не может быть пустым")]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string? Content { get; set; }
    }
}
