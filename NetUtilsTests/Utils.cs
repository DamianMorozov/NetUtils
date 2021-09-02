using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace NetUtilsTests
{
    /// <summary>
    /// Utilites.
    /// </summary>
    public static class Utils
    {
        internal static void MethodStart([CallerMemberName] string memberName = "")
        {
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
            TestContext.WriteLine($@"{memberName} start.");
            TestContext.WriteLine();
        }

        internal static void MethodComplete([CallerMemberName] string memberName = "")
        {
            TestContext.WriteLine();
            TestContext.WriteLine($@"{memberName} complete.");
        }
    }
}
