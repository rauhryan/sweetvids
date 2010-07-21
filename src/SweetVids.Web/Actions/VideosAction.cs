using System;
using System.Collections.Generic;
using System.Linq;
using SweetVids.Core.Domain;
using SweetVids.Core.Persistence;
using SweetVids.Web.Conventions;

namespace SweetVids.Web.Actions
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
}