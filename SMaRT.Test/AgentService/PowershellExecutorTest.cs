// ReSharper disable StyleCop.SA1202
namespace SMaRT.Test.AgentService
{
    using System;
    using System.Collections;

    using NUnit.Framework;

    using SMaRT.AgentService;

    [TestFixture]
    public class PowershellExecutorTest
    {
        private CheckExecutor executor;

        [SetUp]
        protected void SetUp()
        {
            this.executor = new CheckExecutor();
        }

        [Test]
        public void AddParamBeforeSet()
        {
            var call = new TestDelegate(() => this.executor.AddParameter("test", null));
            
            Assert.That(
                call,
                Throws.InvalidOperationException.With.Message.EqualTo("No script set. Set 'Script' first."));
        }

        [Test]
        public void AddParamWithNameNull()
        {
            this.executor.Script = "test";

            var call = new TestDelegate(() => this.executor.AddParameter(null, 1));

            Assert.That(
                call,
                Throws.ArgumentNullException.With.Message.Contains("name"));
        }

        [Test]
        public void ExecuteBeforeSet()
        {
            var call = new TestDelegate(() => this.executor.ExecuteScript());

            Assert.That(
                call, 
                Throws.InvalidOperationException.With.Message.EqualTo("No script set. Set 'Script' first."));
        }

        [Test][TestCaseSource(typeof(SimpleCommandData), nameof(SimpleCommandData.ValidScripts))]
        public string ExecuteValidSimpleScript(string command)
        {
            this.executor.Script = command;
            this.executor.ExecuteScript();

            Assert.That(this.executor.ReturnCode, Is.EqualTo(1));
            Assert.That(this.executor.Error, Is.EqualTo(string.Empty));
            return this.executor.Output;
        }

        [Test]
        public void ExecuteInvalidSimpleScript()
        {
            this.executor.Script = "zzzz";

            this.executor.ExecuteScript();

            Assert.That(this.executor.ReturnCode, Is.EqualTo(3));
            Assert.That(this.executor.Error, Does.StartWith("The term 'zzzz' is not recognized"));
        }

        [Test]
        public void ExecuteCommandWithArgument()
        {
            this.executor.Script =
                "param (" + Environment.NewLine +
                "  [string]$out" + Environment.NewLine +
                ")" + Environment.NewLine +
                "write-output $out";
            this.executor.AddParameter("out", "output");

            this.executor.ExecuteScript();

            Assert.That(
                this.executor.Output,
                Is.EqualTo(@"output"));
        }

        [Test]
        public void ExecuteCommandWithMissingArgument()
        {
            this.executor.Script = 
                "param (" + Environment.NewLine +
                "  [string]$out" + Environment.NewLine +
                ")" + Environment.NewLine +
                "write-output $out";
            this.executor.ExecuteScript();

            Assert.That(
                this.executor.Output,
                Is.EqualTo(string.Empty));
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class SimpleCommandData
    {
        public static IEnumerable ValidScripts
        {
            get
            {
                yield return new TestCaseData(@"cd C:\" + Environment.NewLine + "pwd")
                    .Returns(@"C:\");
                yield return new TestCaseData(@"echo test")
                    .Returns(@"test");
                yield return new TestCaseData(string.Empty)
                    .Returns(string.Empty);
            }
        }
    }
}