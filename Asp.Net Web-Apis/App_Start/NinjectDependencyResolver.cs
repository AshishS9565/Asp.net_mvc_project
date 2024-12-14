using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using Asp.Net_Web_Apis.Repository;
using Ninject;

public class NinjectDependencyResolver : IDependencyResolver
{
    private IKernel kernel;

    public NinjectDependencyResolver()
    {
        kernel = new StandardKernel();
        AddBindings();
    }

    public object GetService(Type serviceType)
    {
        return kernel.TryGet(serviceType);
    }

    public IEnumerable<object> GetServices(Type serviceType)
    {
        return kernel.GetAll(serviceType);
    }

    private void AddBindings()
    {
        // Register IRepository to the Repository class
        kernel.Bind<IRepository>().To<Repository>();
    }
}

//----->>>>>>IV. Configure Dependency Injection for ASP.NET MVC we also use unity mvc dependencies injection

//This class implements the IDependencyResolver interface, which is part of ASP.NET MVC's dependency injection system. 
// The purpose of this class is to provide a mechanism to resolve dependencies, 
//  so that ASP.NET MVC can automatically inject the required services (like IRepository) into controllers or other parts of the application without manually creating instances.