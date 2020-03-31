<<<<<<< HEAD
using NUnit.Framework.Interfaces;

namespace UnityEngine.TestRunner
{
    public interface ITestRunCallback
    {
        void RunStarted(ITest testsToRun);
        void RunFinished(ITestResult testResults);
        void TestStarted(ITest test);
        void TestFinished(ITestResult result);
    }
}
=======
using NUnit.Framework.Interfaces;

namespace UnityEngine.TestRunner
{
    public interface ITestRunCallback
    {
        void RunStarted(ITest testsToRun);
        void RunFinished(ITestResult testResults);
        void TestStarted(ITest test);
        void TestFinished(ITestResult result);
    }
}
>>>>>>> master
