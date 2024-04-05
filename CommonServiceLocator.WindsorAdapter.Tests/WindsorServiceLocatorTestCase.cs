namespace CommonServiceLocator.WindsorAdapter.Tests
{
	using Castle.MicroKernel.Registration;
	using Castle.Windsor;
	using Components;
	using NUnit.Framework;

	[TestFixture]
	public class WindsorServiceLocatorTestCase : ServiceLocatorTestCase
	{
		protected override IServiceLocator CreateServiceLocator()
		{
			IWindsorContainer container = new WindsorContainer()
				.Register(Classes
					.FromAssembly(typeof(ILogger).Assembly)
					.Pick().WithServiceFirstInterface()
				);
			return new WindsorServiceLocator(container);
		}

	}
}