using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Subtext.Scripting.Exceptions;
using Subtext.Scripting.Properties;

namespace Subtext.Scripting
{
    public class ScriptSplitter : IEnumerable<string>
    {
        private readonly TextReader reader;
        private StringBuilder builder = new StringBuilder();
        private char current;
        private char lastChar;
        private ScriptReader scriptReader = null;

        public ScriptSplitter(string script)
        {
            reader = new StringReader(script);
            scriptReader = new SeparatorLineReader(this);
        }

        internal bool HasNext
        {
            get { return reader.Peek() != -1; }
        }

        internal char Current
        {
            get { return current; }
        }

        internal char LastChar
        {
            get { return lastChar; }
        }

        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator()
        {
            while(Next())
            {
                if(Split())
                {
                    string script = builder.ToString().Trim();
                    if(script.Length > 0)
                    {
                        yield return (script);
                    }
                    Reset();
                }
            }
            if(builder.Length > 0)
            {
                string scriptRemains = builder.ToString().Trim();
                if(scriptRemains.Length > 0)
                {
                    yield return (scriptRemains);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        internal bool Next()
        {
            if(!HasNext)
            {
                return false;
            }

            lastChar = current;
            current = (char)reader.Read();
            return true;
        }

        internal int Peek()
        {
            return reader.Peek();
        }

        private bool Split()
        {
            return scriptReader.ReadNextSection();
        }

        internal void SetParser(ScriptReader newReader)
        {
            scriptReader = newReader;
        }

        internal void Append(string text)
        {
            builder.Append(text);
        }

        internal void Append(char c)
        {
            builder.Append(c);
        }

        void Reset()
        {
            current = lastChar = char.MinValue;
            builder = new StringBuilder();
        }
    }

    abstract class ScriptReader
    {
        protected readonly ScriptSplitter splitter;

        public ScriptReader(ScriptSplitter splitter)
        {
            this.splitter = splitter;
        }

        /// <summary>
        /// This acts as a template method. Specific Reader instances 
        /// override the component methods.
        /// </summary>
        public bool ReadNextSection()
        {
            if(IsQuote)
            {
                ReadQuotedString();
                return false;
            }

            if(BeginDashDashComment)
            {
                return ReadDashDashComment();
            }

            if(BeginSlashStarComment)
            {
                ReadSlashStarComment();
                return false;
            }

            return ReadNext();
        }

        protected virtual bool ReadDashDashComment()
        {
            splitter.Append(Current);
            while(splitter.Next())
            {
                splitter.Append(Current);
                if(EndOfLine)
                {
                    break;
                }
            }
            //We should be EndOfLine or EndOfScript here.
            splitter.SetParser(new SeparatorLineReader(splitter));
            return false;
        }

        protected virtual void ReadSlashStarComment()
        {
            if(ReadSlashStarCommentWithResult())
            {
                splitter.SetParser(new SeparatorLineReader(splitter));
                return;
            }
        }

        private bool ReadSlashStarCommentWithResult()
        {
            splitter.Append(Current);
            while(splitter.Next())
            {
                if(BeginSlashStarComment)
                {
                    ReadSlashStarCommentWithResult();
                    continue;
                }
                splitter.Append(Current);

                if(EndSlashStarComment)
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual void ReadQuotedString()
        {
            splitter.Append(Current);
            while(splitter.Next())
            {
                splitter.Append(Current);
                if(IsQuote)
                {
                    return;
                }
            }
        }

        protected abstract bool ReadNext();

        #region Helper methods and properties

        protected bool HasNext
        {
            get { return splitter.HasNext; }
        }

        protected bool WhiteSpace
        {
            get { return char.IsWhiteSpace(splitter.Current); }
        }

        protected bool EndOfLine
        {
            get { return '\n' == splitter.Current; }
        }

        protected bool IsQuote
        {
            get { return '\'' == splitter.Current; }
        }

        protected char Current
        {
            get { return splitter.Current; }
        }

        protected char LastChar
        {
            get { return splitter.LastChar; }
        }

        bool BeginDashDashComment
        {
            get { return Current == '-' && Peek() == '-'; }
        }

        bool BeginSlashStarComment
        {
            get { return Current == '/' && Peek() == '*'; }
        }

        bool EndSlashStarComment
        {
            get { return LastChar == '*' && Current == '/'; }
        }

        protected static bool CharEquals(char expected, char actual)
        {
            return Char.ToLowerInvariant(expected) == Char.ToLowerInvariant(actual);
        }

        protected bool CharEquals(char compare)
        {
            return CharEquals(Current, compare);
        }

        protected char Peek()
        {
            if(!HasNext)
            {
                return char.MinValue;
            }
            return (char)splitter.Peek();
        }

        #endregion
    }

    class SeparatorLineReader : ScriptReader
    {
        private StringBuilder builder = new StringBuilder();
        private bool foundGo;
        private bool gFound;

        public SeparatorLineReader(ScriptSplitter splitter)
            : base(splitter)
        {
        }

        void Reset()
        {
            foundGo = false;
            gFound = false;
            builder = new StringBuilder();
        }

        protected override bool ReadDashDashComment()
        {
            if(!foundGo)
            {
                base.ReadDashDashComment();
                return false;
            }
            base.ReadDashDashComment();
            return true;
        }

        protected override void ReadSlashStarComment()
        {
            if(foundGo)
            {
                throw new SqlParseException(Resources.SqlParseException_IncorrectSyntaxNearGo);
            }
            base.ReadSlashStarComment();
        }

        protected override bool ReadNext()
        {
            if(EndOfLine) //End of line or script
            {
                if(!foundGo)
                {
                    builder.Append(Current);
                    splitter.Append(builder.ToString());
                    splitter.SetParser(new SeparatorLineReader(splitter));
                    return false;
                }
                else
                {
                    Reset();
                    return true;
                }
            }

            if(WhiteSpace)
            {
                builder.Append(Current);
                return false;
            }

            if(!CharEquals('g') && !CharEquals('o'))
            {
                FoundNonEmptyCharacter(Current);
                return false;
            }

            if(CharEquals('o'))
            {
                if(CharEquals('g', LastChar) && !foundGo)
                {
                    foundGo = true;
                }
                else
                {
                    FoundNonEmptyCharacter(Current);
                }
            }

            if(CharEquals('g', Current))
            {
                if(gFound || (!Char.IsWhiteSpace(LastChar) && LastChar != char.MinValue))
                {
                    FoundNonEmptyCharacter(Current);
                    return false;
                }

                gFound = true;
            }

            if(!HasNext && foundGo)
            {
                Reset();
                return true;
            }

            builder.Append(Current);
            return false;
        }

        void FoundNonEmptyCharacter(char c)
        {
            builder.Append(c);
            splitter.Append(builder.ToString());
            splitter.SetParser(new SqlScriptReader(splitter));
        }
    }

    class SqlScriptReader : ScriptReader
    {
        public SqlScriptReader(ScriptSplitter splitter)
            : base(splitter)
        {
        }

        protected override bool ReadNext()
        {
            if(EndOfLine) //end of line
            {
                splitter.Append(Current);
                splitter.SetParser(new SeparatorLineReader(splitter));
                return false;
            }

            splitter.Append(Current);
            return false;
        }
    }
}