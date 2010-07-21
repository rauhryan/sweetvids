using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.View;
using SweetVids.Core.Domain;
using SweetVids.Core.Persistence;
using SweetVids.Web.Conventions;

namespace SweetVids.Web.Actions.Videos
{
    public class VideosAction
    {
        private readonly IRepository<Video> _repository;

        public VideosAction(IRepository<Video> repository)
        {
            _repository = repository;
        }

        public ListVideosViewModel Get(ListVideosRequest request)
        {
            var videos = _repository
                .Query()
                .Skip(10*request.Page)
                .Take(10);

            return new ListVideosViewModel(){Videos = videos};
        }

        public VideoViewModel Get(GetVideoRequest request)
        {
            return new VideoViewModel(){Video = _repository.Get(request.Id)};
        }

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
    }

    public class Videos : FubuPage<ListVideosViewModel> { }
    public class Details : FubuPage<VideoViewModel> { }
}