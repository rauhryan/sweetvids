using System.Collections.Generic;
using SweetVids.Core.Validation;

namespace SweetVids.Core.Domain
{
    public class Video : Entity
    {
        private IList<VideoComment> _comments = new List<VideoComment>();

        [Required]
        public virtual string Link { get; set; }
        [Required]
        public virtual string Description { get; set; }
        [Required]
        public virtual string Title { get; set; }
        [Required]
        public virtual string VideoType { get; set; }

        public virtual IEnumerable<VideoComment> GetComments()
        {
            return _comments;
        }
    }
}