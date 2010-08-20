using FubuMVC.Core;
using FubuMVC.Core.View;
using SweetVids.Core.Domain;

namespace SweetVids.Web.Actions.Videos
{
    public class VideoFormController
    {
        [FubuPartial]
        public VideoFormViewModel Form(AddVideoFormRequest request)
        {
            return new VideoFormViewModel(){Video = new Video()};
        }
    }

    public class AddVideoFormRequest
    {
    }

    public class VideoFormViewModel : VideoViewModel
    {
    }

    public class AddVideoForm : FubuPage<VideoFormViewModel> { }
}