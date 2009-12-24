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
using Similarity.Net;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Services.SearchEngine
{

    public class SearchEngineService : ISearchEngineService, IDisposable
    {
        private Directory _directory;
        private Analyzer _analyzer;
        private static IndexWriter _writer;
        private IndexSearcher _searcher;
        private QueryParser _parser;
        private FullTextSearchEngineSettings _settings;

        private bool _indexUpdatedSinceLastOpen = true;

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

        public SearchEngineService(Directory directory, Analyzer analyzer, FullTextSearchEngineSettings settings)
        {
            _directory = directory;
            _analyzer = analyzer;
            _settings = settings;
            if(_writer==null)
            {
                _writer = new IndexWriter(_directory, _analyzer);
                _writer.Optimize();
            }
            _parser = new QueryParser(BODY, _analyzer);
            _parser.SetDefaultOperator(QueryParser.Operator.AND);
        }

        public void AddPost(SearchEngineEntry post)
        {
            AddPosts(new[] { post });
        }


        public void AddPosts(IEnumerable<SearchEngineEntry> posts)
        {
            foreach (var post in posts)
            {
                ExecuteRemovePost(post.EntryId);
                _writer.AddDocument(CreateDocument(post));
            }
            _indexUpdatedSinceLastOpen = true;
            _writer.Flush();
            _writer.Optimize();
        }

        public void RemovePost(int postId)
        {
            ExecuteRemovePost(postId);
            _indexUpdatedSinceLastOpen = true;
            _writer.Flush();
            _writer.Optimize();
        }

        public int GetIndexedEntryCount(int blogId)
        {
            Term searchTerm = GetBlogIdSearchTerm(blogId);
            TermQuery query = new TermQuery(searchTerm);
            Hits hits = GetSearcher().Search(query);
            return hits.Length();
        }

        public int GetTotalIndexedEntryCount()
        {
            return IndexReader.Open(_directory).NumDocs();
        }

        private void ExecuteRemovePost(int entryId)
        {
            Term searchTerm = GetIdSearchTerm(entryId);
            _writer.DeleteDocuments(searchTerm);
        }

        private Term GetIdSearchTerm(int id)
        {
            return new Term(ENTRYID, NumberTools.LongToString(id));
        }

        private Term GetBlogIdSearchTerm(int id)
        {
            return new Term(BLOGID, NumberTools.LongToString(id));
        }

        protected virtual Document CreateDocument(SearchEngineEntry post)
        {
            Document doc = new Document();

            Field postId = new Field(ENTRYID,
                NumberTools.LongToString(post.EntryId),
                Field.Store.YES,
                Field.Index.UN_TOKENIZED,
                Field.TermVector.NO);

            Field title = new Field(TITLE,
                post.Title,
                Field.Store.YES,
                Field.Index.TOKENIZED,
                Field.TermVector.YES);
            title.SetBoost(_settings.Parameters.TitleBoost);

            Field body = new Field(BODY,
                post.Body,
                Field.Store.NO,
                Field.Index.TOKENIZED,
                Field.TermVector.YES);
            body.SetBoost(_settings.Parameters.BodyBoost);

            Field tags = new Field(TAGS,
                post.Tags,
                Field.Store.NO,
                Field.Index.TOKENIZED,
                Field.TermVector.YES);
            tags.SetBoost(_settings.Parameters.TagsBoost);

            Field blogId = new Field(BLOGID,
                NumberTools.LongToString(post.BlogId),
                Field.Store.NO,
                Field.Index.UN_TOKENIZED,
                Field.TermVector.NO);

            Field published = new Field(PUBLISHED,
                post.IsPublished.ToString(),
                Field.Store.NO,
                Field.Index.UN_TOKENIZED,
                Field.TermVector.NO);

            Field pubDate = new Field(PUBDATE,
                DateTools.DateToString(post.PublishDate, DateTools.Resolution.MINUTE),
                Field.Store.YES,
                Field.Index.UN_TOKENIZED,
                Field.TermVector.NO);

            Field groupId = new Field(GROUPID,
                NumberTools.LongToString(post.GroupId),
                Field.Store.NO,
                Field.Index.UN_TOKENIZED,
                Field.TermVector.NO);

            Field blogName = new Field(BLOGNAME,
                post.BlogName,
                Field.Store.YES,
                Field.Index.NO,
                Field.TermVector.NO);

            Field postName = new Field(ENTRYNAME,
                post.EntryName,
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

        private IndexSearcher GetSearcher()
        {
            if (_indexUpdatedSinceLastOpen)
            {
                if (_searcher != null) _searcher.Close();
                _searcher = new IndexSearcher(_directory);
                _indexUpdatedSinceLastOpen = false;
            }
            return _searcher;
        }

        protected virtual SearchEngineResult CreateSearchResult(Document doc, float score)
        {
            SearchEngineResult result = new SearchEngineResult();
            result.BlogName = doc.Get(BLOGNAME);
            result.EntryId = (int)NumberTools.StringToLong(doc.Get(ENTRYID));
            result.PublishDate = DateTools.StringToDate(doc.Get(PUBDATE));
            result.Title = doc.Get(TITLE);
            result.EntryName = doc.Get(ENTRYNAME);
            result.Score = score;

            return result;
        }

        public IEnumerable<SearchEngineResult> RelatedContents(int entryId, int max, int blogId)
        {
            List<SearchEngineResult> list = new List<SearchEngineResult>();
            IndexSearcher searcher = GetSearcher();

            //First look for the original doc
            Query query = new TermQuery(GetIdSearchTerm(entryId));
            Hits hits = searcher.Search(query);

            if(hits.Length()<=0) return list;

            int docNum = hits.Id(0);

            //Setup MoreLikeThis searcher
            MoreLikeThis mlt = new MoreLikeThis(searcher.GetIndexReader());
            mlt.SetAnalyzer(_analyzer);
            mlt.SetFieldNames(new[] { TITLE, BODY, TAGS });
            mlt.SetMinDocFreq(_settings.Parameters.MinimumDocumentFrequency);
            mlt.SetMinTermFreq(_settings.Parameters.MinimumTermFrequency);
            mlt.SetBoost(_settings.Parameters.MoreLikeThisBoost);

            query = mlt.Like(docNum);
            return PerformQuery(list, query, max, blogId, entryId);
        }

        public IEnumerable<SearchEngineResult> Search(string queryString, int max, int blogId)
        {
            List<SearchEngineResult> list = new List<SearchEngineResult>();
            if (String.IsNullOrEmpty(queryString)) return list;
            Query bodyQuery = _parser.Parse(queryString);

            
            string queryStringMerged = String.Format("({0}) OR ({1}) OR ({2})",
                                                     bodyQuery,
                                                     bodyQuery.ToString().Replace("Body", "Title"),
                                                     bodyQuery.ToString().Replace("Body", "Tags"));

            Query query = _parser.Parse(queryStringMerged);
            

            return PerformQuery(list, query, max, blogId,-1);
        }

        private IEnumerable<SearchEngineResult> PerformQuery(List<SearchEngineResult> list, Query queryOrig, int max, int blogId, int idToFilter)
        {
            Query isPublishedQuery = new TermQuery(new Term(PUBLISHED, true.ToString()));
            Query isBlogQuery = new TermQuery(GetBlogIdSearchTerm(blogId));
            
            BooleanQuery query = new BooleanQuery();
            query.Add(isPublishedQuery, BooleanClause.Occur.MUST);
            query.Add(queryOrig, BooleanClause.Occur.MUST);
            query.Add(isBlogQuery, BooleanClause.Occur.MUST);

            Hits hits = GetSearcher().Search(query);
            int length = hits.Length();
            int resultsAdded = 0;
            float minScore = _settings.MinimumScore;
            for (int i = 0; i < length && resultsAdded < max; i++)
            {
                SearchEngineResult result = CreateSearchResult(hits.Doc(i), hits.Score(i));
                if (idToFilter != result.EntryId && result.Score > minScore && result.PublishDate < DateTime.Now)
                {
                    list.Add(result);
                    resultsAdded++;
                }
                    
            }
            return list;
        }

        public void Dispose()
        {
            var searcher = _searcher;
            if (searcher != null)
            {
                searcher.Close();
            }

            var writer = _writer;
            if (writer != null)
            {
                writer.Close();
            }

            var directory = _directory;
            if(directory != null) {
              directory.Close();
            }

            writer = _writer;
            if (writer != null)
            {
                _writer = null;
            }
        }
    }
}
