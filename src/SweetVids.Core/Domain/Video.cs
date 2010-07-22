using System;
using System.Collections.Generic;
using System.Web;
using SweetVids.Core.Validation;

namespace SweetVids.Core.Domain
{
    public class Video : Entity
    {
        private IList<VideoComment> _videoComments = new List<VideoComment>();

        [Required]
        public virtual string Link { get; set; }
        [Required]
        public virtual string Description { get; set; }
        [Required]
        public virtual string Title { get; set; }
        [Required]
        public virtual string VideoType { get; set; }

        public virtual IEnumerable<VideoComment> GetVideoComments()
        {
            return _videoComments;
        }

        public virtual string GetYouTubeUrl()
        {
            var url =
                "http://www.youtube.com/v/{0}&amp;hl=en_US&amp;fs=1";
            var uri = new Uri(Link);

            var vidId = HttpUtility.ParseQueryString(uri.Query).Get("v");

            return string.Format(url, vidId);
           
        }
    }
}