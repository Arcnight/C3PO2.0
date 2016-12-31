using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleInjector;

using C3PO.Data.Interfaces;
using C3PO.Data.Repositories;

namespace C3PO.Data.Extensions
{
    public static class DataDependencies
    {
        public static void RegisterRespositories(this Container iocContainer)
        {
            iocContainer.Register<IBoardingRepository, BoardingRepository>(Lifestyle.Scoped);
        }
    }
}
