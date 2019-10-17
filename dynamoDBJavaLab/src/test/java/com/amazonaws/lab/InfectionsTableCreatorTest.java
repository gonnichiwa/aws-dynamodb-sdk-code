package com.amazonaws.lab;
// Copyright 2017 Amazon Web Services, Inc. or its affiliates. All rights reserved.

import static org.junit.Assert.fail;

import org.junit.Test;

import com.amazonaws.services.dynamodbv2.model.ResourceNotFoundException;

public class InfectionsTableCreatorTest {

	@Test
	public void test() throws Exception {
		InfectionsTableCreator.main(new String[0]);
		try {
			InfectionsTableCreator.dynamoDBClient.describeTable(InfectionsTableCreator.INFECTIONS_TABLE_NAME);
		} catch (ResourceNotFoundException e) {
			String msg = InfectionsTableCreator.INFECTIONS_TABLE_NAME
					+ " %s table does not exist. Do not need to remove it.";
			fail(msg);
		}
	}
}
