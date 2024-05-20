using GigaComic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Models.Entities
{
    public class User : BaseUser
    {
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}
