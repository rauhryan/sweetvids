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
        private readonly IRepository<VideoComment> _repository;

        public CommentsController(IRepository<VideoComment> repository)
        {
            _repository = repository;
        }

        [UrlForNew(typeof(VideoComment))]
        public FubuContinuation Post(AddCommentRequest request)
        {
            request.Comment.Video = new Video(){Id = request.VideoId};
            request.Comment.Email = request.Comment.Email.ToGravatarHash();
            _repository.Save(request.Comment);


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