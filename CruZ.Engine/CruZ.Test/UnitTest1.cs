//using Assimp.Configs;
//using CruZ.Components;
//using MonoGame.Extended.Entities;

//namespace CruZ.Test
//{
//    [TestClass]
//    public class UnitTest1
//    {
//        class Template1 : EntityTemplate
//        {
//            public Template1(string name) : base(name) { }

//            public override void GetInstruction(IBuildInstruction buildInstruction)
//            {
//                buildInstruction.RequireComponent(typeof(SpriteComponent));
//            }
//        }

//        class TemplateParent : EntityTemplate
//        {
//            public TemplateParent(string name) : base(name) { }

//            public override void GetInstruction(IBuildInstruction buildInstruction)
//            {
//                buildInstruction.AddChildTemplate(new Template1("child1"));

//                var c2 = new Template1("child2");
//                buildInstruction.AddChildTemplate(c2);

//                buildInstruction.SetTarget(c2);
//                buildInstruction.RequireComponent(typeof(AnimatedSpriteComponent));

//                buildInstruction.AddChildTemplate(new Template1("child3"));
//            }
//        }

//        [TestMethod]
//        public void TestRequireComponentInstruction()
//        {
//            World world = new WorldBuilder().Build();

//            var builder = new EntityBuilder(world);

//            var root = new Template1("root");
//            var d = builder.BuildFrom(root);

//            Assert.IsTrue(d[root].HasComponent(typeof(SpriteComponent)));
//        }

//        [TestMethod]
//        public void TestParentAndChild()
//        {
//            World world = new WorldBuilder().Build();

//            var builder = new EntityBuilder(world);

//            var root = new TemplateParent("root");
//            var d = builder.BuildFrom(root);

//            Assert.IsTrue(d.Count == 4);

//            foreach (var template in d.Keys)
//            {
//                var e = d[template];

//                if(template.AttachedEntity.NameId == "root")
//                {
//                    Assert.IsTrue(e.Parent == null);
//                }

//                List<string> c = ["child1", "child2"];
//                if (c.Contains(template.AttachedEntity.NameId))
//                {
//                    Assert.IsTrue(e.Parent.NameId == "root");
//                }

//                if(template.AttachedEntity.NameId == "child2")
//                {
//                    Assert.IsTrue(e.HasComponent(typeof(AnimatedSpriteComponent)));
//                }

//                if(template.AttachedEntity.NameId == "child3")
//                {
//                    Assert.IsTrue(e.Parent.NameId == "child2");
//                }
//            }
//        }
//    }
//}