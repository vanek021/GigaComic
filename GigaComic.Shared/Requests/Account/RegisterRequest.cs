using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Shared.Requests.Account
{
    public class RegisterRequest : BaseAccountRequest
    {

        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль повторно")]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string RepeatPassword { get; set; }
    }
}
