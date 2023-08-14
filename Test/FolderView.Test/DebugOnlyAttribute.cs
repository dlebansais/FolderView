namespace NUnit.Framework;

using System.Diagnostics;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

public class DebugOnlyAttribute : NUnitAttribute, IApplyToTest
{
    private const string Reason = "This test should be run in Debug configuration only";

    public void ApplyToTest(Test test)
    {
        if (!Debugger.IsAttached)
        {
            test.RunState = RunState.Ignored;
            test.Properties.Set(PropertyNames.SkipReason, Reason);
        }
    }
}
