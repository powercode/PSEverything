using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using PSEverything;
using Xunit;

namespace PSEverythingTests
{

    public class SelectEverythingTests
    {
        [Fact]
        public void TestSelectEverythingString()
        {
            var iss = InitialSessionState.CreateDefault2();
            iss.ImportPSModule(new []{(typeof(SelectEverythingStringCommand).Assembly.Location)});
            using (var ps = PowerShell.Create(iss))
            {
                ps.Commands.AddCommand("Select-EverythingString")
                  .AddParameter("-Extension", "ps1")
                  .AddParameter("-Global", true)
                  .AddParameter("-Pattern", "function (\\S+)");


                var res = ps.Invoke();
                Assert.True(res.Count > 0);
            }
        }
    }
}
