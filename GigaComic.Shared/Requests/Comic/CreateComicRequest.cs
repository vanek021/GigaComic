using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Shared.Requests.Comic
{
    public class CreateComicRequest : IRequest
    {
        [Required(ErrorMessage = "Введите название темы")]
        public string Theme { get; set; }
    }
}
