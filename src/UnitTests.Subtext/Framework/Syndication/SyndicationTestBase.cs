namespace UnitTests.Subtext.Framework.Syndication
{
    public class SyndicationTestBase
    {
        protected string indent()
        {
            return "    ";
        }

        protected string indent(int count)
        {
            string returnval = string.Empty;
            for(int i = 0; i < count; i++)
            {
                returnval += indent();
            }
            return returnval;
        }
    }
}