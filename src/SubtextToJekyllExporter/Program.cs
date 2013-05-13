using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subtext.Extensibility;
using Subtext.Framework.Data;
using System.IO;
using System.Globalization;

namespace SubtextToJekyllExporter
{
    class Program
    {
        const string defaultConnectionStringFormat = @"Server=.\SQLEXPRESS;Database={0};Trusted_Connection=True;User Instance=false;";
        const string filenameFormat = "{0}-{1}.markdown";
        const string postFormat = 
@"---
layout: post
title: ""0}""
date: {1}
comments: true
categories: {2}
---

{3}
";
        static void Main(string[] args)
        {
            string connectionString = null;
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a connection string or the database name");
                connectionString = Console.ReadLine();
            }
            else
            {
                connectionString = args[0];
            }

            if (connectionString == "?" || connectionString == "help") {
                Console.WriteLine("Subtext to Jekyll Exporter");
                Console.WriteLine("--------------------------");
                Console.WriteLine();
                Console.WriteLine("Usage: SubtextToJekyllExporter ConnectionString");
                Console.WriteLine();
                Console.WriteLine("This is just going to barf the files in the current directory.");
                return;
            }

            if (!connectionString.Contains(';')) 
            {
                // must be the database name
                connectionString = String.Format(defaultConnectionStringFormat, connectionString);
            }

            var sprocs = new StoredProcedures(connectionString);
            var repository = new DatabaseObjectProvider(0, sprocs);
            var entries = repository.GetEntries(int.MaxValue, PostType.BlogPost, PostConfig.IsActive, true);

            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            foreach(var entry in entries)
            {
                string jekyllDate = entry.DateSyndicated.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string slug = (entry.EntryName + ".aspx").Replace("\r", "").Replace("\n", "");

                var categories = String.Join(" ", entry.Categories.Select(c => "\"" + c + "\"").ToArray());

                string fileName = String.Format(CultureInfo.InvariantCulture, filenameFormat, jekyllDate, slug);
                foreach (var c in Path.GetInvalidFileNameChars()) 
                {
                    fileName = fileName.Replace("" + c, "");
                }
                string contents = String.Format(CultureInfo.InvariantCulture, postFormat, entry.Title, jekyllDate, categories, entry.Body);
                Console.WriteLine("Writing: " + slug);
                using (var writer = new StreamWriter(fileName, false, utf8WithoutBom))
                {
                    writer.WriteLine(contents);
                }
            }
        }
    }
}
