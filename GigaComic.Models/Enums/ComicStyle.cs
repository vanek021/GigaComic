using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Models.Enums
{
    public enum ComicStyle
    {
        [Display(Name = "Свой стиль")]
        Default,

        [Display(Name = "Kandinsky")]
        Kandinsky,

        [Display(Name = "Детальное фото")]
        Uhd,

        [Display(Name = "Аниме")]
        Anime
    }
}
