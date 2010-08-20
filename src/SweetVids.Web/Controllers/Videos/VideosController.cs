using System;
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

        public ListVideosViewModel Get(ListVideosRequest request)
        {
            var videos = _repository
                .Query()
                .Skip(10*request.Page)
                .Take(10)
                .OrderByDescending(x => x.Created);

            var count = _repository.Query().Count();


            return new ListVideosViewModel(){Videos = videos, Total = count, Page = request.Page};
        }

        public VideoViewModel Get(GetVideoRequest request)
        {
            return new VideoViewModel(){Video = _repository.Get(request.Id)};
        }

        [UrlForNew(typeof(Video))]
        public FubuContinuation Post(AddVideoRequest request)
        {
            _repository.Save(request.Video);

            return FubuContinuation.RedirectTo(new ListVideosRequest());
        }

        public FubuContinuation Delete(DeleteVideoRequest request)
        {
            var video = _repository.Get(request.Id);
            _repository.Delete(video);

            return FubuContinuation.RedirectTo(new ListVideosRequest());
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

    public class VideoViewModel
    {
        public Video Video { get; set; }
    }

    public class ListVideosRequest
    {
        public int Page { get; set; }
    }

    public class ListVideosViewModel
    {
        public IEnumerable<Video> Videos { get; set; }

        public int Total { get; set; }

        public int Page { get; set; }

        public Video Video { get; set; }
    }

    public class Videos : FubuPage<ListVideosViewModel> { }
    public class Details : FubuPage<VideoViewModel> { }
}