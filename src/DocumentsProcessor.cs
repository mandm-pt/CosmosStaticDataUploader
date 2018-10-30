using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CosmosStaticDataUploader.Utils;

namespace CosmosStaticDataUploader
{
    internal static class DocumentsProcessor
    {
        private static DocumentClient client;

        public static async Task Start(string workingFolder, Config.Environment env)
        {
            client = new DocumentClient(new Uri(env.Endpoint), env.AuthorizationKey);

            foreach (var dirPath in Directory.EnumerateDirectories(workingFolder))
            {
                string databaseId = Path.GetFileName(dirPath);
                var databaseLink = UriFactory.CreateDatabaseUri(databaseId);

                await client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseId });

                bool checkCollection = false;
                foreach (var subDirPath in Directory.EnumerateDirectories(dirPath))
                {
                    string collectionId = Path.GetFileName(subDirPath);
                    var collectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);

                    if (!checkCollection)
                    {
                        await client.CreateDocumentCollectionIfNotExistsAsync(databaseLink, new DocumentCollection { Id = collectionId });
                        checkCollection = true;
                    }

                    WriteLine($"Starting processing: {databaseId} {collectionId}", ConsoleColor.White);

                    await ProcessFolderFiles(subDirPath, collectionLink);

                    var envFolders = Directory.GetDirectories(subDirPath)
                        .Where(fp => string.Equals(Path.GetFileName(fp), env.Name, StringComparison.InvariantCultureIgnoreCase));

                    foreach (var envSubDir in envFolders)
                    {
                        await ProcessFolderFiles(envSubDir, collectionLink);
                    }
                }
            }
        }

        private static async Task ProcessFolderFiles(string folderPath, Uri collectionLink)
        {
            foreach (var file in Directory.EnumerateFiles(folderPath))
            {
                string fileContents = await File.ReadAllTextAsync(file);
                dynamic @object = JObject.Parse(fileContents);

                try
                {
                    var created = await client.UpsertDocumentAsync(collectionLink, @object);
                    WriteLine($"\tFile upserted - {Path.GetFileName(file)}", ConsoleColor.Green);
                }
                catch (Exception ex)
                {
                    WriteLine($"\tThere was a problem upserting the following file: {Path.GetFileName(file)}", ConsoleColor.Red);
                }
            }
        }
    }
}
