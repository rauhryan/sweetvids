using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using StructureMap.AutoMocking;
using SweetVids.Core.Domain;
using SweetVids.Core.Persistence;
using SweetVids.Web.Actions;
using SweetVids.Web.Controllers.Videos;

namespace SweetVids.Tests.Actions
{
    public class VideoActionGetTester : ContextSpecification
    {
        protected RhinoAutoMocker<VideosController> _mocks;
        protected VideosController Controller;
        protected VideoViewModel _outModel;
        private Video _video;


        protected override void SetupFixtureContext()
        {
            _mocks = new RhinoAutoMocker<VideosController>();
            Controller = _mocks.ClassUnderTest;

            _video = new Video() {Id = Guid.NewGuid()};
        }

        protected override void BecauseOnce()
        {
            base.BecauseOnce();
            _mocks.Get<IRepository<Video>>().Stub(c => c.Get(_video.Id)).Return(_video);

            _outModel = Controller.Get(new GetVideoRequest() {Id = _video.Id});
        }

        [Test]
        public void OutModel_Should_Not_Be_Null()
        {
            _outModel.ShouldNotBeNull();
        }

        [Test]
        public void OutModel_Should_Have_the_Video()
        {
            _outModel.Video.ShouldBeTheSameAs(_video);
        }
    }


}