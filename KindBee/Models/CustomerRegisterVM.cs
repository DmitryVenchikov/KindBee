using KindBee.DB.DBModels;
using System.ComponentModel.DataAnnotations;

namespace KindBee.Models
{
    public class CustomerRegisterVM
    {
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }
        public string? Middlename { get; set; }
        [Required(ErrorMessage = "Не указана Фамилия")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Не указан номер")]
        public string PhoneNumber { get; set; }
        public string? Mail { get; set; }

        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Длина строки должна быть от 8 до 50 символов")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }
}
