namespace NUnit.Framework;

using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

public sealed class DebugOnlyAttribute : NUnitAttribute, IApplyToTest
{
#if !DEBUG
    private const string Reason = "This test should be run in Debug configuration only";
#endif

    public void ApplyToTest(Test test)
    {
#if !DEBUG
        test.RunState = RunState.Ignored;
        test.Properties.Set(PropertyNames.SkipReason, Reason);
#endif
    }
}
