using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
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

        public virtual IEnumerable<VideoComment> Comments
        {
            get { return _comments; }
        }

        public virtual string YouTubeUrl
        {
            get
            {
                var url =
                    "http://www.youtube.com/v/{0}&amp;hl=en_US&amp;fs=1";
                var uri = new Uri(Link);

                var vidId = HttpUtility.ParseQueryString(uri.Query).Get("v");

                return string.Format(url, vidId);
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append("<div><h3>");
            builder.Append(Title);
            builder.Append("</h3><div>");
            builder.Append(string.Format(
                "<object width=\"960\" height=\"745\"><param name=\"movie\" value=\"{0}\"></param><param name=\"allowFullScreen\" value=\"true\"></param><param name=\"allowscriptaccess\" value=\"always\"></param><embed src=\"{0}\" type=\"application/x-shockwave-flash\"allowscriptaccess=\"always\" allowfullscreen=\"true\" width=\"660\" height=\"525\"></embed></object>",
                YouTubeUrl));
            builder.Append("</div><p>");
            builder.Append(Description);
            builder.Append("</p></div>");
            return builder.ToString();
        }
       
    }
}