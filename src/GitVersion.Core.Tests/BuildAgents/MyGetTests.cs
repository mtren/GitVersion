using GitVersion.BuildAgents;
using GitVersion.Core.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GitVersion.Core.Tests.BuildAgents
{
    [TestFixture]
    public class MyGetTests : TestBase
    {
        private MyGet buildServer;

        [SetUp]
        public void SetUp()
        {
            var sp = ConfigureServices(services => services.AddSingleton<MyGet>());
            this.buildServer = sp.GetService<MyGet>();
        }

        [Test]
        public void DevelopBranch()
        {
            var vars = new TestableVersionVariables(fullSemVer: "0.0.0-Unstable4");
            var message = this.buildServer.GenerateSetVersionMessage(vars);
            Assert.AreEqual(null, message);
        }

        [Test]
        public void EscapeValues()
        {
            var message = this.buildServer.GenerateSetParameterMessage("Foo", "0.8.0-unstable568 Branch:'develop' Sha:'ee69bff1087ebc95c6b43aa2124bd58f5722e0cb'");
            Assert.AreEqual("##myget[setParameter name='GitVersion.Foo' value='0.8.0-unstable568 Branch:|'develop|' Sha:|'ee69bff1087ebc95c6b43aa2124bd58f5722e0cb|'']", message[0]);
        }

        [Test]
        public void BuildNumber()
        {
            var message = this.buildServer.GenerateSetParameterMessage("LegacySemVerPadded", "0.8.0-unstable568");
            Assert.AreEqual("##myget[buildNumber '0.8.0-unstable568']", message[1]);
        }
    }
}
