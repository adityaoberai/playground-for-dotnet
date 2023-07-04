﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Appwrite;
using Appwrite.Services;
using Appwrite.Models;
using Newtonsoft.Json;

namespace playground_for_dotnet
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Client client = new Client();
            client.SetEndpoint("[ENDPOINT]");
            client.SetProject("[PROJECT_ID]");
            client.SetKey("[API_KEY]");

            Databases databases = new Databases(client);

            Database database;
            Collection collection;

            Storage storage = new Storage(client);

            Bucket bucket;

            /**
                Create Database
            */
            try
            {
                Console.WriteLine("Running Create Database API");
                database = await databases.Create(
                    databaseId: ID.Unique(),
                    name: "MoviesDB"
                );
                Console.WriteLine("Done");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                throw;
            }

            /**
                Create Collection
            */

            try
            {
                Console.WriteLine("Running Create Collection API");
                collection = await databases.CreateCollection(
                    databaseId: database.Id,
                    collectionId: ID.Unique(),
                    name: "Movies",
                    permissions: new List<string> { Permission.Read(Role.Any()), Permission.Write(Role.Any()) }
                );

                Console.WriteLine("Creating Attribute \"name\"");

                await databases.CreateStringAttribute(
                    databaseId: database.Id,
                    collectionId: collection.Id,
                    key: "name",
                    size: 255,
                    required: true
                );

                Console.WriteLine("Creating Attribute \"release_year\"");

                await databases.CreateIntegerAttribute(
                    databaseId: database.Id,
                    collectionId: collection.Id,
                    key: "release_year",
                    required: true
                );

                Console.WriteLine("Done");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                throw;
            }

            /**
                List Collections
            */
            try
            {
                Console.WriteLine("Running List Collection API");
                var collectionsList = await databases.ListCollections(
                    databaseId: database.Id
                );
                foreach (var element in collectionsList.Collections)
                {
                    Console.WriteLine($"- {element.Name}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                throw;
            }

            /**
                Add Document
            */
            Movie movie1 = new Movie("Alien", 1979);
            Movie movie2 = new Movie("Equilibrium", 2002);
            try
            {
                Console.WriteLine("Running Create Documents API");
                await databases.CreateDocument(
                    databaseId: database.Id,
                    collectionId: collection.Id,
                    documentId: ID.Unique(),
                    data: movie1
                );
                await databases.CreateDocument(
                    databaseId: database.Id,
                    collectionId: collection.Id,
                    documentId: ID.Unique(),
                    data: movie2
                );
                Console.WriteLine("Done");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                throw;
            }

            /**
                List Documents
            */
            try
            {
                Console.WriteLine("Running List Documents API");
                var documentsList = await databases.ListDocuments(
                    databaseId: database.Id,
                    collectionId: collection.Id
                );
                foreach (var element in documentsList.Documents)
                {
                    var movie = JsonConvert.DeserializeObject<Movie>(JsonConvert.SerializeObject(element));
                    Console.WriteLine($"- {movie.Name} ({movie.release_year})");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                throw;
            }

            /**
                Create Bucket
            */

            try
            {
                Console.WriteLine("Running Create Bucket API");

                bucket = await storage.CreateBucket(
                    bucketId: ID.Unique(),
                    name: "Files",
                    permissions: new List<string> { Permission.Read(Role.Any()), Permission.Write(Role.Any()) }
                );

                Console.WriteLine("Done");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                throw;
            }

            /**
                Create File
            */

            try
            {
                Console.WriteLine("Running Create File API");

                var file = await storage.CreateFile(
                    bucketId: bucket.Id,
                    fileId: ID.Unique(),
                    file: InputFile.FromPath("[DIRECTORY_PATH]/appwrite-overview.png"),
                    permissions: new List<string> { Permission.Read(Role.Any()), Permission.Write(Role.Any()) }
                );

                Console.WriteLine("Done");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                throw;
            }
        }
    }
    public class Movie
    {
        public Movie(string name, int releaseYear)
        {
            Name = name;
            release_year = releaseYear;
        }
        public string Name { get; }
        public int release_year { get; }

    }
}
