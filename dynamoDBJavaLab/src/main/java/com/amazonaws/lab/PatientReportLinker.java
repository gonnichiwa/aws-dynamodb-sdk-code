package com.amazonaws.lab;
// Copyright 2017 Amazon Web Services, Inc. or its affiliates. All rights reserved.

import com.amazonaws.services.dynamodbv2.AmazonDynamoDB;
import com.amazonaws.services.dynamodbv2.AmazonDynamoDBClientBuilder;
import com.amazonaws.services.dynamodbv2.document.DynamoDB;
import com.amazonaws.services.dynamodbv2.document.UpdateItemOutcome;

// The PatientReportLinker class updates DynamoDB items with the corresponding link to a patient's report on S3.
public class PatientReportLinker {

	public static final String INFECTIONS_TABLE_NAME = InfectionsTableCreator.INFECTIONS_TABLE_NAME;
	public static final String PATIENT_REPORT_PREFIX = Utils.PATIENT_REPORT_PREFIX;
	public static final String S3_BUCKET_NAME = Utils.LAB_S3_BUCKET_NAME;
	public static final String S3_BUCKET_REGION = Utils.LAB_S3_BUCKET_REGION;

	public static DynamoDB dynamoDB = null;
	private static AmazonDynamoDB dynamoDBClient = null;

	public static void main(String[] args) throws Exception {

		// Instantiate DynamoDB client and object

		// Instantiate DynamoDB client and object
		dynamoDBClient = AmazonDynamoDBClientBuilder.standard().build();

		dynamoDB = new DynamoDB(dynamoDBClient);

		String reportUrl = null;
		String objectKey = null;

		// Sample reports exist for patient ids 1, 2, 3
		for (int i = 1; i < 4; i++) {
			objectKey = PATIENT_REPORT_PREFIX + i + ".txt";

			// Construct the URL for the patient report
			reportUrl = "https://s3-" + S3_BUCKET_REGION + ".amazonaws.com/" + S3_BUCKET_NAME + "/" + objectKey;

			System.out.printf("Updating item for patientId: %d %n", i);

			// Update the DynamoDB item with the link
			UpdateItemOutcome outcome = updateItemWithLink(dynamoDB, INFECTIONS_TABLE_NAME, ("" + i), reportUrl);
			System.out.println("\nPrinting item after adding attribute:");

			// Print item in JSON format
			System.out.println(outcome.getItem().toJSONPretty());
			System.out.println("-------------------------------------");
		}
	}

	/**
	 * Update the item for the patient id with an attribute called PatientReportUrl
	 *
	 * @param dynamoDB
	 *            Instance of DynamoDB class
	 * @param tableName
	 *            Table name
	 * @param patientId
	 *            Patient ID
	 * @param reportUrl
	 *            Report ID
	 * @return Query results
	 */
	private static UpdateItemOutcome updateItemWithLink(DynamoDB dynamoDB, String tableName, String patientId,
			String reportUrl) {
		// TODO 3: Replace the solution with your own code
		return Solution.updateItemWithLink(dynamoDB, tableName, patientId, reportUrl);
	}
}
