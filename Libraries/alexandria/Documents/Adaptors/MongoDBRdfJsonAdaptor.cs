﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using Alexandria.Utilities;

namespace Alexandria.Documents.Adaptors
{
    public class MongoDBRdfJsonAdaptor : IDataAdaptor<Document,Document>
    {
        private RdfJsonParser _parser = new RdfJsonParser();
        private RdfJsonWriter _writer = new RdfJsonWriter();

        public void ToGraph(IGraph g, IDocument<Document, Document> document)
        {
            //Get our JSON String
            Document mongoDoc = document.BeginRead();
            String json = mongoDoc["graph"].ToString();
            document.EndRead();

            //Then parse this
            StringParser.Parse(g, json, this._parser);
        }

        public void ToDocument(IGraph g, IDocument<Document, Document> document)
        {
            //Generate our JSON String
            String json = StringWriter.Write(g, this._writer);

            //Then convert this to a Document
            Document mongoDoc = document.BeginWrite(false);
            mongoDoc["graph"] = MongoDBHelper.JsonToDocument(json);
            document.EndWrite();
        }

        public void AppendTriples(IEnumerable<Triple> ts, IDocument<Document, Document> document)
        {
            throw new NotImplementedException();
        }

        public void DeleteTriples(IEnumerable<Triple> ts, IDocument<Document, Document> document)
        {
            throw new NotImplementedException();
        }
    }
}
