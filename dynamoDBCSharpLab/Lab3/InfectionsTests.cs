// Copyright 2017 Amazon Web Services, Inc. or its affiliates. All rights reserved.

using Amazon.DynamoDBv2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab3
{
    [TestClass]
    //@Test
    public class InfectionsTests
    {
        [TestMethod]
        public void TestPatientReportLinker()
        {
            PatientReportLinker.main();

            // Checks if PatientReportUrl attribute is present.
            for (int i = 1; i < 4; i++)
            {
                Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
                {
                    { "PatientID", new AttributeValue { S = "" + i } }
                };

                // Create GetItem request
                GetItemRequest request = new GetItemRequest
                {
                    TableName = PatientReportLinker.InfectionsTableName,
                    Key = key,
                };

                GetItemResponse response = PatientReportLinker.dynamoDBClient.GetItem(request);

                bool patientReportUrlAttrPresent = false;

                if(response.Item["PatientReportUrl"].S.ToString() != null)
                {
                    patientReportUrlAttrPresent = true;
                }

                Debug.WriteLine("Test - PatientID: {0}, patientReportUrlAttrPresent: {1}", i, patientReportUrlAttrPresent);

                if (patientReportUrlAttrPresent == false)
                {
                    Assert.Fail("Patient Report Linker Failure");
                }
            }
        }

        [TestMethod]
        public void TestInfectionStatistics()
        {
            InfectionStatistics.main(new string[] { "Reno" });
            if(InfectionStatistics.itemCount != 178)
            {
                Assert.Fail("Infection Statistics Failure");
            }
        }

        [TestMethod]
        public void TestTableCreation()
        {
            InfectionsTableCreator.main();
            try
            {
                InfectionsTableCreator.dynamoDBClient.DescribeTable(InfectionsTableCreator.InfectionsTableName);
            } catch (ResourceNotFoundException e)
            {
                string msg = InfectionsTableCreator.InfectionsTableName + " {0} table does not exist. Do not need to remove it.";
                Assert.Fail(msg + e.Message);
            }
        }

        [TestMethod]
        public void TestDataUploader()
        {
            InfectionsDataUploader.Main();
            if (InfectionsDataUploader.numberOfFailures != 0)
            {
                Assert.Fail("Infections Data Uploader Failures: " + InfectionsDataUploader.numberOfFailures);
            }
        }
    }
}
