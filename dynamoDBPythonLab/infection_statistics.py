# Copyright 2017 Amazon Web Services, Inc. or its affiliates. All rights
# reserved.

import boto3
from boto3.dynamodb.conditions import Key
import sys
from datetime import datetime, timedelta
import utils
import solution as dynamodb_solution

INFECTIONS_TABLE_NAME = utils.LAB_S3_INFECTIONS_TABLE_NAME
CITY_DATE_INDEX_NAME = "InfectionsByCityDate"


def query_by_city(city):
    print("City name is {0}".format(city))
    # Query Infections table based on the input city and count the number of
    # infections
    count_for_city = 0
    try:
        dynamodb = boto3.resource('dynamodb')
        recs = query_city_related_items(
            dynamodb, INFECTIONS_TABLE_NAME, CITY_DATE_INDEX_NAME, city)

        # Retrieves and prints from recs dictionary returned by the query.
        for rec in recs['Items']:
            print("\t", rec['PatientId'], rec['Date'])
        count_for_city = len(recs['Items'])
        print("Count of Infections in the city: {0}".format(count_for_city))
    except Exception as err:
        print("Error Message: {0}".format(err))
    return count_for_city


def query_city_related_items(dynamodb, infections_table_name, gsi_name, city):
    """Return the items returned by the query

    Keyword arguments:
    dynamodb -- DynamoDB resource
    infections_table_name -- Table name
    gsi_name -- Table index name
    city -- City name
    """

    # TODO 2: Replace the solution with your own code
    return dynamodb_solution.query_city_related_items(
        dynamodb, infections_table_name, gsi_name, city)

if __name__ == '__main__':
    print("Querying items by city")
    query_by_city(city="Reno")
