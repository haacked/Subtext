using System.IO;

namespace SubtextUpgraderTests
{
    public class NonDisposableMemoryStream : MemoryStream
    {
        public override void Close()
        {
            // Do nothing
        }

        protected override void Dispose(bool disposing)
        {
            // Do Nothing
        }
    }

}
