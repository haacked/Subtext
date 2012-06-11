using System.IO;
using System.Text;

namespace UnitTests.Subtext
{
    /// <summary>
    /// When simulating a blog request, this class 
    /// contains some useful context for the setup.
    /// </summary>
    internal class SimulatedRequestContext
    {
        public string HostName;
        public StringBuilder ResponseStringBuilder;
        public TextWriter ResponseTextWriter;
        public SimulatedHttpRequest SimulatedRequest;

        public SimulatedRequestContext(SimulatedHttpRequest request, StringBuilder responseText,
                                       TextWriter responseWriter, string host)
        {
            ResponseStringBuilder = responseText;
            ResponseTextWriter = responseWriter;
            SimulatedRequest = request;
            HostName = host;
        }
    }
}