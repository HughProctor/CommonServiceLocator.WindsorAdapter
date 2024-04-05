namespace CommonServiceLocator.WindsorAdapter.Tests
{
	using System.Collections;
	using System.Collections.Generic;
	using Components;
	using NUnit.Framework;

	public abstract class ServiceLocatorTestCase
	{
		private IServiceLocator locator;

		[SetUp]
		public void SetUp()
		{
			locator = CreateServiceLocator();
		}

		protected abstract IServiceLocator CreateServiceLocator();

		[Test]
		public void GetInstance()
		{
			ILogger instance = locator.GetInstance<ILogger>();
			Assert.That(instance, Is.Not.Null, "instance should not be null");
		}

		[Test]
		public void AskingForInvalidComponentShouldRaiseActivationException()
		{
            Assert.Throws<ActivationException>(() => locator.GetInstance<IDictionary>());
		}

		[Test]
		public void GetNamedInstance()
		{
			ILogger instance = locator.GetInstance<ILogger>(typeof(AdvancedLogger).FullName);
			Assert.That(instance, Is.InstanceOf(typeof(AdvancedLogger)), "Should be an advanced logger");
		}

		[Test]
		public void GetNamedInstance2()
		{
			ILogger instance = locator.GetInstance<ILogger>(typeof(SimpleLogger).FullName);
			Assert.That(instance, Is.InstanceOf(typeof(SimpleLogger)), "Should be a simple logger");
		}

		[Test]
		public void GetNamedInstance_WithZeroLenName()
		{
            Assert.Throws<ActivationException>(() => locator.GetInstance<ILogger>(""));
		}

		[Test]
		public void GetUnknownInstance2()
		{
            Assert.Throws<ActivationException>(() => locator.GetInstance<ILogger>("test"));
		}

		[Test]
		public void GetAllInstances()
		{
			IEnumerable<ILogger> instances = locator.GetAllInstances<ILogger>();
			IList<ILogger> list = new List<ILogger>(instances);
			Assert.That(2, Is.EqualTo(list.Count));
		}

		[Test]
		public void GetlAllInstance_ForUnknownType_ReturnEmptyEnumerable()
		{
			IEnumerable<IDictionary> instances = locator.GetAllInstances<IDictionary>();
			IList<IDictionary> list = new List<IDictionary>(instances);
			Assert.That(0, Is.EqualTo(list.Count));
		}

		[Test]
		public void GenericOverload_GetInstance()
		{
			Assert.That(
				locator.GetInstance<ILogger>().GetType(), Is.EqualTo(
				locator.GetInstance(typeof(ILogger), null).GetType()),
				"should get the same type"
				);
		}

		[Test]
		public void GenericOverload_GetInstance_WithName()
		{
			Assert.That(
				locator.GetInstance<ILogger>(typeof(AdvancedLogger).FullName).GetType(), Is.EqualTo(
				locator.GetInstance(typeof(ILogger), typeof(AdvancedLogger).FullName).GetType()),
				"should get the same type"
				);
		}

		[Test]
		public void Overload_GetInstance_NoName_And_NullName()
		{
			Assert.That(
				locator.GetInstance<ILogger>().GetType(), Is.EqualTo(
				locator.GetInstance<ILogger>(null).GetType()),
				"should get the same type"
				);
		}

		[Test]
		public void GenericOverload_GetAllInstances()
		{
			List<ILogger> genericLoggers = new List<ILogger>(locator.GetAllInstances<ILogger>());
			List<object> plainLoggers = new List<object>(locator.GetAllInstances(typeof(ILogger)));
			Assert.That(genericLoggers.Count, Is.EqualTo(plainLoggers.Count));
			for (int i = 0; i < genericLoggers.Count; i++)
			{
				Assert.That(
					genericLoggers[i].GetType(), Is.EqualTo(
					plainLoggers[i].GetType()),
					"instances (" + i + ") should give the same type");
			}
		}

	}
}