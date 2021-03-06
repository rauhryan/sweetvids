using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.View;
using SweetVids.Core.Domain;
using SweetVids.Core.Persistence;
using SweetVids.Web.Conventions;

namespace SweetVids.Web.Controllers.Videos
{
    public class VideosController
    {
        private readonly IRepository<Video> _repository;

        public VideosController(IRepository<Video> repository)
        {
            _repository = repository;
        }

        public ListVideosViewModel List(ListVideosRequest request)
        {
            var videos = _repository
                .Query()
                .Skip(10*request.Page)
                .Take(10)
                .OrderByDescending(x => x.Created);

            var count = _repository.Query().Count();


            return new ListVideosViewModel(){Videos = videos, Total = count, Page = request.Page};
        }

        public VideoViewModel Details(GetVideoRequest request)
        {
            return new VideoViewModel(){Video = _repository.Get(request.Id)};
        }

        [UrlForNew(typeof(Video))]
        public AjaxResponse Post(AddVideoRequest request)
        {
            _repository.Save(request.Video);

            return new AjaxResponse()
                       {
                           Success = true, 
                           Payload = request.Video
                       };
        }

        public AjaxResponse Delete(DeleteVideoRequest request)
        {
            var video = _repository.Get(request.Id);
            _repository.Delete(video);

            return new AjaxResponse()
                       {
                           Success = true
                       };
        }
    }

    public class DeleteVideoRequest : IRequestById
    {
        public Guid Id { get; set; }
    }

    public class AddVideoRequest
    {
        public Video Video { get; set; }
    }

    public class GetVideoRequest : IRequestById 
    {
        public Guid Id { get; set; }
    }

    public class VideoViewModel : IReturnJson
    {
        public Video Video { get; set; }
        public object Flatten()
        {
            return Video;
        }
    }

    public class ListVideosRequest
    {
        public int Page { get; set; }
    }

    public class ListVideosViewModel : IEnumerable
    {
        public IEnumerable<Video> Videos { get; set; }

        public int Total { get; set; }

        public int Page { get; set; }

        public Video Video { get; set; }
        public IEnumerator GetEnumerator()
        {
            return Videos.GetEnumerator();
        }
    }

    public class Videos : FubuPage<ListVideosViewModel> { }
    public class Details : FubuPage<VideoViewModel> { }
}