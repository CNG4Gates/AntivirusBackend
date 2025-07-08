using System.ComponentModel.DataAnnotations;

namespace Antivirus.DTOs
{
    public class UsersReadDTO
    {
        public long Id { get; set; }
        public string? DateBirth { get; set; }
        public string? Email { get; set; }
        public string? LastName { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UsersCreateDTO
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(255)]
        public string? Name { get; set; }
        [StringLength(255)]
        public string? LastName { get; set; }
        [StringLength(255)]
        public string? DateBirth { get; set; }
        [StringLength(255)]
        public string? ImageUrl { get; set; }
    }

    public class UsersUpdateDTO
{
    [StringLength(255)]
    public string? Password { get; set; }
    [StringLength(255)]
    public string? Name { get; set; }
    [StringLength(255)]
    public string? LastName { get; set; }
    [StringLength(255)]
    public string? DateBirth { get; set; }
    [StringLength(255)]
    public string? ImageUrl { get; set; }
}

}
