using Assimp.Configs;
using CruZ.Components;
using MonoGame.Extended.Entities;

namespace CruZ.Test
{
    [TestClass]
    public class UnitTest1
    {
        class Template1 : EntityTemplate
        {
            public override void GetInstruction(IBuildInstruction buildInstruction)
            {
                buildInstruction.RequireComponent(typeof(SpriteComponent));
            }
        }

        class TemplateParent : EntityTemplate
        {
            public override void GetInstruction(IBuildInstruction buildInstruction)
            {
                buildInstruction.AddChildTemplate(new Template1());
                buildInstruction.AddChildTemplate(new Template1());
            }
        }

        [TestMethod]
        public void TemplateBuiderTest()
        {
            World world = new WorldBuilder().Build();

            var builder = new EntityBuilder(world);

            var root = new Template1();
            var d = builder.BuildFrom(root);

            Assert.IsTrue(d[root].HasComponent(typeof(SpriteComponent)));
        }

        [TestMethod]
        public void TemplateBuiderParentTest()
        {
            World world = new WorldBuilder().Build();

            var builder = new EntityBuilder(world);

            var root = new TemplateParent();
            var d = builder.BuildFrom(root);

            Assert.IsTrue(d.Count == 3);

            Assert.IsTrue(d[root].Parent == null);

            var rootEntity = d[root];
            d.Remove(root);

            var child = d.First().Value;
            Assert.IsTrue(child.Parent == rootEntity);
            Assert.IsTrue(child.HasComponent(typeof(SpriteComponent)));
        }
    }
}