using RestHomes.Domain.Entities;
using RestHomes.Domain.Abstract;
using RestHomes.Domain.Concrete;
using Moq;
using System.Linq;
using System.Collections.Generic;

using System;
using System.Web.Routing;
using System.Web.Mvc;
using Ninject;

namespace RestHomes.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBindings()
        {//controller's configuring
            Mock<IFortNightRepository> mock = new Mock<IFortNightRepository>();
            mock.Setup(m => m.FortNights).Returns(new List<FortNight> { new FortNight { firstDay = new DateTime(2015, 05, 12),
                                                                                        Hours = new double[14] { 0.0, 4.5, 1.0, 4.0, 0.0, 0.0, 2.0,
                                                                                                                 5.0, 6.0, 0.0, 0.0, 3.0, 2.0, 1.0 },
                                                                                        Worker = new Personal { Name = "Sarah" } },
                                                                        new FortNight { firstDay = new DateTime(2015, 05, 12),
                                                                                        Hours = new double[14] {3.0, 4.5, 1.0, 4.0, 0.0, 0.0, 0.0,
                                                                                                                0.0, 2.5, 3.0, 4.5, 1.0, 1.0, 1.0},
                                                                                        Worker = new Personal { Name = "Anna" } } 
            }.AsQueryable());
            ninjectKernel.Bind<IFortNightRepository>().ToConstant(mock.Object);
            ninjectKernel.Bind<IDBRepository>().To<EFDBRepository>();
        }
    }
}