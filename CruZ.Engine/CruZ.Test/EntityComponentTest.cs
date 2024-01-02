using CruZ.Components;
using Mono = MonoGame.Extended.Entities;

namespace CruZ.Test
{
    public class TestComponent : EntityScript
    {

    }

    [TestClass]
	public class EntityComponentTest
	{
		[TestInitialize]
		public void Initialize()
		{
            _world = new Mono.WorldBuilder().Build();
        }

		[TestMethod]
		public void TestAddCopmonent()
		{
			var te = _world.CreateTransformEntity();

			te.AddComponent(new TestComponent());

			Assert.IsTrue(te.HasComponent(typeof(TestComponent)));
			Assert.IsTrue(te.HasComponent(typeof(EntityScript)));
		}

		[TestMethod]
		public void TestRequireComponent()
		{
            var te = _world.CreateTransformEntity();
			te.RequireComponent(typeof(TestComponent));

            Assert.IsTrue(te.HasComponent(typeof(TestComponent)));
            Assert.IsTrue(te.HasComponent(typeof(EntityScript)));
        }

        [TestMethod]
        public void IsComponentTest()
		{
			Assert.IsTrue(ComponentManager.IsComponent(typeof(TestComponent)));
		}

        [TestMethod]
        public void TestGetComponent()
        {
            var te = _world.CreateTransformEntity();
			var com = new TestComponent();
            te.AddComponent(com);

			Assert.IsTrue(te.GetComponent(typeof(TestComponent)) == com);
			Assert.IsTrue(te.GetComponent<TestComponent>() == com);
        }

        Mono.World _world;
	}
}