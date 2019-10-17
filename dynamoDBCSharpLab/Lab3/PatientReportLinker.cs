// Copyright 2017 Amazon Web Services, Inc. or its affiliates. All rights reserved.

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Lab3
{
    // The PatientReportLinker class updates DynamoDB items with the corresponding
    // link to a patient's report on S3
    public static class PatientReportLinker
    {
        public static readonly string InfectionsTableName = InfectionsTableCreator.InfectionsTableName;
        public static readonly string PatientReportPrefix = Utils.PatientReportPrefix;
        public static readonly string S3BucketName = Utils.LabS3BucketName;
        public static readonly string S3BucketRegion = Utils.LabS3BucketRegion;
        public static AmazonDynamoDBClient dynamoDBClient = null;

        public static void main()
        {
            Init();
            LinkPatientReport();
        }

        private static void LinkPatientReport()
        {
            string reportUrl = null;
            string objectKey = null;

            // Sample reports exist for patient ids 1, 2, 3
            for (int i = 1; i < 4; i++)
            {
                objectKey = PatientReportPrefix + i + ".txt";
                reportUrl = "https://s3-" + S3BucketRegion + ".amazonaws.com/" + S3BucketName + "/" + objectKey;
                UpdateItemWithLink(("" + i), reportUrl);
            }
        }

        private static void UpdateItemWithLink(string patientId, string reportUrl)
        {
            Debug.WriteLine("Updating item patientId: {0}, reportURL: {1}", patientId, reportUrl);

            UpdateItemRequest requestUpdate = new UpdateItemRequest
            {
                TableName = InfectionsTableName,
                Key = new Dictionary<string, AttributeValue>()
                        {
                            { "PatientID", new AttributeValue { S = patientId } }
                        },
                ExpressionAttributeNames = new Dictionary<string, string>()
                        {
                            { "#purl", "PatientReportUrl" },
                        },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                        {
                             { ":val1", new AttributeValue {S = reportUrl} },
                        },
                // This expression does the following:
                // Adds a new attribute to the item
                UpdateExpression = "SET #purl = :val1"
            };

            UpdateItemResponse responseUpdate = UpdateItem(requestUpdate);
            Debug.WriteLine("Printing item after adding attribute:");

            string jsonDisplayText = JsonConvert.SerializeObject(responseUpdate);
            Debug.WriteLine("Display item.");
            Debug.WriteLine(jsonDisplayText);
        }

        private static void Init()
        {
            dynamoDBClient = new AmazonDynamoDBClient();
        }

        /**
         * Update the item in the table
         *
         * @param requestUpdate     Update Item Request
         * @return                  Order
         */
        private static UpdateItemResponse UpdateItem(UpdateItemRequest requestUpdate)
        {
            // TODO 3: Replace the solution with your own code
            return Solution.UpdateItem(dynamoDBClient, requestUpdate);
        }
    }
}
