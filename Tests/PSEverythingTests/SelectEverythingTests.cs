using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using PSEverything;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSEverythingTests
{

    [TestClass]
    public class SelectEverythingTests
    {
        [TestMethod]
        public void TestSelectEverythingString()
        {
            var iss = InitialSessionState.CreateDefault2();
            iss.ImportPSModule(new[] { (typeof(SelectEverythingStringCommand).Assembly.Location) });
            using (var ps = PowerShell.Create(iss))
            {
                ps.Commands.AddCommand("Select-EverythingString")
                  .AddParameter("-Extension", "psd1")
                  .AddParameter("-Global", true)
                  .AddParameter("-Pattern", "RootModule")
                  .AddParameter("-Exclude", "$env:SystemRoot");


                var res = ps.Invoke();
                Assert.IsTrue(res.Count > 0);
            }
        }
    }
}
