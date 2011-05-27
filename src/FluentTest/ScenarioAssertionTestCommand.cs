using System;
using System.Xml;
using Xunit.Sdk;

namespace FluentTest
{
    class ScenarioAssertionTestCommand : ITestCommand
    {
        private Func<MethodResult> execute;
        private string displayName;

        public ScenarioAssertionTestCommand(string displayName, Func<MethodResult> execute)
        {
            this.displayName = displayName;
            this.execute = execute;
        }

        public string DisplayName
        {
            get { return displayName; }
        }

        public MethodResult Execute(object testClass)
        {
            return execute();
        }

        public bool ShouldCreateInstance
        {
            get { return false; }
        }

        public int Timeout
        {
            get { return 0; }
        }

        public XmlNode ToStartXml()
        {
            return null;
        }
    }
}
