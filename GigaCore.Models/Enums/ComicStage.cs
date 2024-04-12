using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Models.Enums
{
    public enum ComicStage
    {
        [Display(Name = "Создание тезисов")]
        AbstractsCreation = 0,

        [Display(Name = "Редактирование историй")]
        StoriesEditing = 1,

        [Display(Name = "Настройка комикса")]
        ComicSetup = 2,

        [Display(Name = "Завершен")]
        Completed = 3
    }
}
