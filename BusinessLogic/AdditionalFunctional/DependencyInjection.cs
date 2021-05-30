using System;
using System.Collections.Generic;
using System.Text;
using Ninject;
using Ninject.Modules;
using ServerLayer.Interfaces;
using ServerLayer.DataObtaining;
using ServerLayer.Repositories;

namespace BusinessLogic.AdditionalFunctional
{
    class DependencyInjection : NinjectModule
    {
        private string ConnectionString;
        public DependencyInjection(string connectionStr)
        {
            ConnectionString = connectionStr;
        }
        public override void Load()
        {
            Bind<IDbAccess>().To<DbAccessRepository>().WithConstructorArgument(ConnectionString);
        }
    }
}
