package com.amazonaws.lab;
// Copyright 2017 Amazon Web Services, Inc. or its affiliates. All rights reserved.

import static org.junit.Assert.*;

import org.junit.Test;

public class InfectionsDataUploaderTest {

	@Test
	public void test() throws Exception {
		InfectionsDataUploader.main(new String[0]);
		assertEquals(1000, InfectionsDataUploader.numItemsAdded, 0);
	}
}
