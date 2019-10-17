// Copyright 2017 Amazon Web Services, Inc. or its affiliates. All rights reserved.

using Amazon;
using Amazon.DynamoDBv2;
using Amazon.S3;
using Amazon.S3.Model;
using System.Diagnostics;
using System.IO;

namespace Lab3
{
    public static class InfectionsDataUploader
    {
        public static readonly string InfectionsTableName = InfectionsTableCreator.InfectionsTableName;
        public static readonly string S3BucketName = Utils.LabS3BucketName;
        public static readonly string S3BucketRegion = Utils.LabS3BucketRegion;
        public static readonly string InfectionsDataKeyFile = Utils.InfectionsDataKeyFile;

        private static AmazonDynamoDBClient dynamoDBClient = null;
        private static AmazonS3Client s3 = null;

        public static int numberOfFailures = 0;

        public static void Main()
        {
            Init();
            LoadInfectionsData();
        }

        private static void LoadInfectionsData()
        {
            string line;
            char[] splitter = { ',' };
            StreamReader reader = null;

            try
            {
                // Retrieve the infections data file from the S3 bucket
                GetObjectRequest requestFromS3 = null;
                requestFromS3 = new GetObjectRequest()
                {
                    BucketName = S3BucketName,
                    Key = InfectionsDataKeyFile
                };

                using (var responseFromS3 = s3.GetObject(requestFromS3))
                {
                    reader = new StreamReader(responseFromS3.ResponseStream);
                    // Skip first line because it has the attribute names only
                    reader.ReadLine();

                    while ((line = reader.ReadLine()) != null)
                    {
                        // separate CSV values by comma
                        string[] infectionsDataAttrValues = line.Split(splitter);

                        try
                        {
                            if (!infectionsDataAttrValues[0].ToLower().Equals("patientid"))
                            {
                                // Create an instance of a DynamoDB item with attribute
                                // values read from the infections data file
                                Debug.WriteLine("\n");
                                AddItemToTable(infectionsDataAttrValues);
                                Debug.WriteLine("Added item:" + line);
                            }
                        }
                        catch (AmazonDynamoDBException ex)
                        {
                            Debug.WriteLine("Failed to create item in" + InfectionsTableName);
                            Debug.WriteLine(ex.Message);
                            numberOfFailures++;
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine(e.StackTrace);
                numberOfFailures = 9999;
            }
            catch (IOException e)
            {
                Debug.WriteLine(e.StackTrace);
                numberOfFailures = 9999;
            }
            finally
            {
                if (reader != null)
                {
                    try
                    {
                        reader.Close();
                    }
                    catch (IOException e)
                    {
                        Debug.WriteLine(e.StackTrace);
                    }
                }
            }

            Debug.WriteLine("Infections data upload complete.");
        }

        private static void Init()
        {
            dynamoDBClient = new AmazonDynamoDBClient();
            RegionEndpoint region = RegionEndpoint.USWest2;
            s3 = new AmazonS3Client(region);
        }

        /**
         * Add the item to the infections data table
         *
         * @param infectionsDataAttrValues    Values to be inserted into the table
         */
        private static void AddItemToTable(string[] infectionsDataAttrValues)
        {
            // TODO 1: Replace the solution with your own code
            Solution.AddItemToTable(dynamoDBClient, infectionsDataAttrValues, InfectionsTableName);
        }
    }
}
