using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using StructureMap.AutoMocking;
using SweetVids.Core.Domain;
using SweetVids.Core.Persistence;
using SweetVids.Web.Actions;
using SweetVids.Web.Actions.Videos;

namespace SweetVids.Tests.Actions
{
    public class VideosActionTester : ContextSpecification
    {
        protected RhinoAutoMocker<VideosController> _mocks;
        protected VideosController Controller;
        protected ListVideosViewModel _outModel;
        protected IList<Video> _vidList;


        protected override void SetupFixtureContext()
        {
            _mocks = new RhinoAutoMocker<VideosController>();
            Controller = _mocks.ClassUnderTest;

            _vidList = new List<Video>() { new Video(), new Video(), new Video(), new Video() };
        }

        protected override void BecauseOnce()
        {
            base.BecauseOnce();
            _mocks.Get<IRepository<Video>>().Stub(c => c.Query()).Return(_vidList.AsQueryable());
            
        }
    }
    public class VideoAction_NoPagingTester : VideosActionTester
    {
        protected override void BecauseOnce()
        {
            base.BecauseOnce();

            _outModel = Controller.Get(new ListVideosRequest { Page = 0 });
        }

        [Test]
        public void Get_should_ask_the_repository_for_videos()
        {
            _mocks.Get<IRepository<Video>>().AssertWasCalled(c => c.Query());

        }

        [Test]
        public void The_outmodel_should_not_be_null()
        {
            _outModel.ShouldNotBeNull();
        }

        [Test]
        public void OutModel_should_have_same_number_of_videos_as_provided_list()
        {
            _outModel.Videos.ShouldHaveCount(_vidList.Count);
        }


    }

    public class VideoActionPagingTester : VideosActionTester
    {
        protected override void BecauseOnce()
        {
            _vidList.AddRange(new[]
                                  {
                                      new Video(), new Video(), new Video(), new Video(), new Video(), new Video(),
                                      new Video(), new Video(), new Video(), new Video(), new Video(), new Video(),
                                      new Video(), new Video(), new Video(), new Video(), new Video(), new Video(),
                                      new Video(), new Video(), new Video(),
                                  });
            base.BecauseOnce();

            _outModel = Controller.Get(new ListVideosRequest { Page = 1 });
            
        }

        [Test]
        public void The_outmodel_should_not_be_null()
        {
            _outModel.ShouldNotBeNull();
        }

        [Test]
        public void Outmodel_should_only_contain_10_videos()
        {
            _outModel.Videos.ShouldHaveCount(10);
        }
    }
}