#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Similarity.Net;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;

namespace Subtext.Framework.Services.SearchEngine
{

    public class SearchEngineService : ISearchEngineService
    {
        private readonly Directory _directory;
        private readonly Analyzer _analyzer;
        private static IndexWriter _writer;
        private readonly FullTextSearchEngineSettings _settings;

        private const string TITLE = "Title";
        private const string BODY = "Body";
        private const string TAGS = "Tags";
        private const string PUBDATE = "PubDate";
        private const string BLOGID = "BlogId";
        private const string GROUPID = "GroupId";
        private const string BLOGNAME = "BlogName";
        private const string ENTRYID = "PostId";
        private const string PUBLISHED = "IsPublished";
        private const string ENTRYNAME = "EntryName";

        private static readonly Object WriterLock = new Object();

        private static readonly Log Log = new Log();
        private bool _disposed;

        public SearchEngineService(Directory directory, Analyzer analyzer, FullTextSearchEngineSettings settings)
        {
            _directory = directory;
            _analyzer = analyzer;
            _settings = settings;
        }

        private void DoWriterAction(Action<IndexWriter> action)
        {
            lock(WriterLock)
            {
                EnsureIndexWriter();
            }
            action(_writer);
        }

        private T DoWriterAction<T>(Func<IndexWriter,T> action)
        {
            lock (WriterLock)
            {
                EnsureIndexWriter();
            }
            return action(_writer);
        }
      
        // Method should only be called from within a lock.
        void EnsureIndexWriter()
        {
            if(_writer == null)
            {
                if(IndexWriter.IsLocked(_directory))
                {
                    Log.Error("Something left a lock in the index folder: deleting it");
                    IndexWriter.Unlock(_directory);
                    Log.Info("Lock Deleted... can proceed");
                }
                _writer = new IndexWriter(_directory, _analyzer,IndexWriter.MaxFieldLength.UNLIMITED);
                _writer.SetMergePolicy(new LogDocMergePolicy(_writer));
                _writer.SetMergeFactor(5);
            }
        }

        private IndexSearcher Searcher { 
            get {return DoWriterAction(writer => new IndexSearcher(writer.GetReader())); }
        }


        private QueryParser BuildQueryParser()
        {
            var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_CURRENT,BODY, _analyzer);
            parser.SetDefaultOperator(QueryParser.Operator.AND);
            return parser;
        }

        public IEnumerable<IndexingError> AddPost(SearchEngineEntry post)
        {
            return AddPosts(new[] { post }, false);
        }

        public IEnumerable<IndexingError> AddPosts(IEnumerable<SearchEngineEntry> posts)
        {
            return AddPosts(posts, true);
        }

        public IEnumerable<IndexingError> AddPosts(IEnumerable<SearchEngineEntry> posts, bool optimize)
        {
            IList<IndexingError> errors = new List<IndexingError>();
            foreach (var post in posts)
            {
                ExecuteRemovePost(post.EntryId);
                try
                {
                    var currentPost = post;
                    DoWriterAction(writer => writer.AddDocument(CreateDocument(currentPost)));
                }
                catch(Exception ex)
                {
                    errors.Add(new IndexingError(post, ex));
                }
            }
            DoWriterAction(writer =>
            {
                writer.Commit();
                if(optimize)
                {
                    writer.Optimize();
                }

            });
            
            return errors;
        }

        public void RemovePost(int postId)
        {
            ExecuteRemovePost(postId);
            DoWriterAction(writer => writer.Commit());
        }

        public int GetIndexedEntryCount(int blogId)
        {
            var query = GetBlogIdSearchQuery(blogId);
            TopDocs hits = Searcher.Search(query,1);
            return hits.totalHits;
        }

        public int GetTotalIndexedEntryCount()
        {
            return DoWriterAction(writer => writer.GetReader().NumDocs());
        }

        private void ExecuteRemovePost(int entryId)
        {
            Query searchQuery = GetIdSearchQuery(entryId);
            DoWriterAction(writer => writer.DeleteDocuments(searchQuery));
        }

        private Query GetIdSearchQuery(int id)
        {
            return new TermQuery(new Term(ENTRYID, NumericUtils.IntToPrefixCoded(id)));
        }

        private Query GetBlogIdSearchQuery(int id)
        {
            return new TermQuery(new Term(BLOGID, NumericUtils.IntToPrefixCoded(id)));
        }

        protected virtual Document CreateDocument(SearchEngineEntry post)
        {
            var doc = new Document();

            Field postId = new Field(ENTRYID,
                NumericUtils.IntToPrefixCoded(post.EntryId),
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);

            Field title = new Field(TITLE,
                post.Title,
                Field.Store.YES,
                Field.Index.ANALYZED,
                Field.TermVector.YES);
            title.SetBoost(_settings.Parameters.TitleBoost);

            Field body = new Field(BODY,
                post.Body,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.YES);
            body.SetBoost(_settings.Parameters.BodyBoost);

            Field tags = new Field(TAGS,
                post.Tags,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.YES);
            tags.SetBoost(_settings.Parameters.TagsBoost);

            Field blogId = new Field(BLOGID,
                NumericUtils.IntToPrefixCoded(post.BlogId),
                Field.Store.NO,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);


            Field published = new Field(PUBLISHED,
                post.IsPublished.ToString(),
                Field.Store.NO,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);

            Field pubDate = new Field(PUBDATE,
                DateTools.DateToString(post.PublishDate, DateTools.Resolution.MINUTE),
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);

            Field groupId = new Field(GROUPID,
                NumericUtils.IntToPrefixCoded(post.GroupId),
                Field.Store.NO,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);

            Field blogName = new Field(BLOGNAME,
                post.BlogName,
                Field.Store.YES,
                Field.Index.NO,
                Field.TermVector.NO);

            Field postName = new Field(ENTRYNAME,
                post.EntryName ?? "",
                Field.Store.YES,
                Field.Index.NO,
                Field.TermVector.NO);
            postName.SetBoost(_settings.Parameters.EntryNameBoost);


            doc.Add(postId);
            doc.Add(title);
            doc.Add(body);
            doc.Add(tags);
            doc.Add(blogId);
            doc.Add(published);
            doc.Add(pubDate);
            doc.Add(groupId);
            doc.Add(blogName);
            doc.Add(postName);

            return doc;
        }

        protected virtual SearchEngineResult CreateSearchResult(Document doc, float score)
        {
            var result = new SearchEngineResult();
            result.BlogName = doc.Get(BLOGNAME);
            result.EntryId = NumericUtils.PrefixCodedToInt(doc.Get(ENTRYID));
            result.PublishDate = DateTools.StringToDate(doc.Get(PUBDATE));
            result.Title = doc.Get(TITLE);
            string entryName = doc.Get(ENTRYNAME);
            result.EntryName = !String.IsNullOrEmpty(entryName) ? entryName : null;
            result.Score = score;

            return result;
        }

        public IEnumerable<SearchEngineResult> RelatedContents(int entryId, int max, int blogId)
        {
            var list = new List<SearchEngineResult>();

            //First look for the original doc
            Query query = GetIdSearchQuery(entryId);
            TopDocs hits = Searcher.Search(query, max);

            if(hits.scoreDocs.Length <= 0) 
            {
                return list;
            }

            int docNum = hits.scoreDocs[0].doc;

            //Setup MoreLikeThis searcher
            var reader = DoWriterAction(w => w.GetReader());
            var mlt = new MoreLikeThis(reader);
            mlt.SetAnalyzer(_analyzer);
            mlt.SetFieldNames(new[] { TITLE, BODY, TAGS });
            mlt.SetMinDocFreq(_settings.Parameters.MinimumDocumentFrequency);
            mlt.SetMinTermFreq(_settings.Parameters.MinimumTermFrequency);
            mlt.SetBoost(_settings.Parameters.MoreLikeThisBoost);

            var moreResultsQuery = mlt.Like(docNum);
            return PerformQuery(list, moreResultsQuery, max+1, blogId, entryId);
        }

        public IEnumerable<SearchEngineResult> Search(string queryString, int max, int blogId)
        {
            return Search(queryString, max, blogId, -1);
        }

        public IEnumerable<SearchEngineResult> Search(string queryString, int max, int blogId, int entryId)
        {
            List<SearchEngineResult> list = new List<SearchEngineResult>();
            if (String.IsNullOrEmpty(queryString)) return list;
            QueryParser parser = BuildQueryParser();
            Query bodyQuery = parser.Parse(queryString);

            
            string queryStringMerged = String.Format("({0}) OR ({1}) OR ({2})",
                                                     bodyQuery,
                                                     bodyQuery.ToString().Replace("Body", "Title"),
                                                     bodyQuery.ToString().Replace("Body", "Tags"));

            Query query = parser.Parse(queryStringMerged);
            

            return PerformQuery(list, query, max, blogId, entryId);
        }

        private IEnumerable<SearchEngineResult> PerformQuery(ICollection<SearchEngineResult> list, Query queryOrig, int max, int blogId, int idToFilter)
        {
            Query isPublishedQuery = new TermQuery(new Term(PUBLISHED, true.ToString()));
            Query isBlogQuery = GetBlogIdSearchQuery(blogId);
            
            var query = new BooleanQuery();
            query.Add(isPublishedQuery, BooleanClause.Occur.MUST);
            query.Add(queryOrig, BooleanClause.Occur.MUST);
            query.Add(isBlogQuery, BooleanClause.Occur.MUST);
            IndexSearcher searcher = Searcher;
            TopDocs hits = searcher.Search(query, max);
            int length = hits.scoreDocs.Length;
            int resultsAdded = 0;
            float minScore = _settings.MinimumScore;
            float scoreNorm = 1.0f / hits.GetMaxScore(); 
            for (int i = 0; i < length && resultsAdded < max; i++)
            {
                float score = hits.scoreDocs[i].score * scoreNorm;
                SearchEngineResult result = CreateSearchResult(searcher.Doc(hits.scoreDocs[i].doc), score);
                if (idToFilter != result.EntryId && result.Score > minScore && result.PublishDate < DateTime.Now)
                {
                    list.Add(result);
                    resultsAdded++;
                }
                    
            }
            return list;
        }

        ~SearchEngineService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private void Dispose(bool disposing)
        {
            lock (WriterLock)
            {
                if (!_disposed)
                {
                    //Never checking for disposing = true because there are
                    //no managed resources to dispose

                    var writer = _writer;

                    if (writer != null)
                    {
                        writer.Close();
                        _writer = null;
                    }
                    
                    var directory = _directory;
                    if(directory != null) {
                        directory.Close();
                    }

                    _disposed = true;
                }
            }
        }
    }
}
