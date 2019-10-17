// Copyright 2017 Amazon Web Services, Inc. or its affiliates. All rights reserved.

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using System;

namespace Lab3
{
    class Solution
    {
        public static void AddItemToTable(AmazonDynamoDBClient dynamoDBClient, string[] infectionsDataAttrValues, string InfectionsTableName)
        {
            Debug.WriteLine(String.Format("RUNNING SOLUTION CODE: {0}! Follow the steps in the lab guide to replace this method with your own implementation.\n", "AddItemToTable"));
            var requestDiseaseListing = new PutItemRequest
            {
                TableName = InfectionsTableName,
                Item = new Dictionary<string, AttributeValue>()
                {
                    { "PatientID", new AttributeValue { S = infectionsDataAttrValues[0] } },
                    { "City",      new AttributeValue { S = infectionsDataAttrValues[1] } },
                    { "Date",      new AttributeValue { S = infectionsDataAttrValues[2] } },
                }
            };
            dynamoDBClient.PutItem(requestDiseaseListing);
        }

        public static QueryResponse QueryCityRelatedItems(AmazonDynamoDBClient dynamoDBClient, string inputCity, string InfectionsTableName, string CityDateIndexName)
        {
            Debug.WriteLine(String.Format("RUNNING SOLUTION CODE: {0}! Follow the steps in the lab guide to replace this method with your own implementation.\n", "QueryCityRelatedItems"));
            QueryRequest request = new QueryRequest
            {
                TableName = InfectionsTableName,
                IndexName = CityDateIndexName,
                KeyConditionExpression = "City = :v_city",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":v_city", new AttributeValue { S = inputCity } }
                }
            };

            QueryResponse response = dynamoDBClient.Query(request);
            return response;
        }

        public static UpdateItemResponse UpdateItem(AmazonDynamoDBClient dynamoDBClient, UpdateItemRequest requestUpdate)
        {
            Debug.WriteLine(String.Format("RUNNING SOLUTION CODE: {0}! Follow the steps in the lab guide to replace this method with your own implementation.\n", "UpdateItem"));
            UpdateItemResponse response = dynamoDBClient.UpdateItem(requestUpdate);
            return response;
        }
    }
}
