using SweetVids.Core.Validation;

namespace SweetVids.Core.Domain
{
    public class VideoComment : Entity
    {
        [Required]
        public virtual string Name { get; set; }
        [Required, ValidEmail]
        public virtual string Email { get; set; }

        [Required]
        public virtual string Comment { get; set; }

        public virtual Video Video { get; set; }
    }
}