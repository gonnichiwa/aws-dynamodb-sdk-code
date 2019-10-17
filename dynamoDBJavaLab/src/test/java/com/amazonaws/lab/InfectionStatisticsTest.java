package com.amazonaws.lab;
// Copyright 2017 Amazon Web Services, Inc. or its affiliates. All rights reserved.

import static org.junit.Assert.*;

import org.junit.Test;

public class InfectionStatisticsTest {

	@Test
	public void test() throws Exception {
		InfectionStatistics.main(new String[] { "Reno" });
		assertEquals(178, InfectionStatistics.itemCount, 0);
	}
}
