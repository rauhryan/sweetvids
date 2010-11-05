using System;
using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using SweetVids.Core;
using SweetVids.Core.Domain;
using SweetVids.Core.Persistence;
using SweetVids.Web.Controllers.Videos;

namespace SweetVids.Web.Controllers.Comments
{
    public class CommentsController
    {
        private readonly IRepository<Video> _repository;

        public CommentsController(IRepository<Video> repository)
        {
            _repository = repository;
        }

        [UrlForNew(typeof(VideoComment))]
        public FubuContinuation Post(AddCommentRequest request)
        {
            var video = _repository.Get(request.VideoId);

            video.AddComment(request.Comment);

            _repository.Save(video);
    
            return FubuContinuation.RedirectTo(new GetVideoRequest{Id = request.VideoId});}
        }
    }

    public class AddCommentRequest
    {
        public Guid VideoId { get; set; }
        public VideoComment Comment
        {
            get; set;
        }
}