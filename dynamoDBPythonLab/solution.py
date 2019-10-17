# Copyright 2017 Amazon Web Services, Inc. or its affiliates. All rights
# reserved.

import boto3
import time
from boto3.dynamodb.conditions import Key


# Put an item in the infections table using the attribute values for
# PatientId, City, and Date attributes
def add_item_to_table(infections_table, patient_id, city, date):
    print(
        "\nRUNNING SOLUTION CODE:",
        "put_item_in_table!",
        "Follow the steps in the lab guide to replace this method with your own implementation.")

    infections_table.put_item(
        Item={
            'PatientId': patient_id,
            'City': city,
            'Date': date})


# Query the table's global secondary index for items that contain the
# given city name
def query_city_related_items(dynamodb, infections_table_name, gsi_name, city):
    print(
        "\nRUNNING SOLUTION CODE:",
        "query_city_related_items!",
        "Follow the steps in the lab guide to replace this method with your own implementation.")

    infections_table = dynamodb.Table(infections_table_name)
    recs = infections_table.query(
        IndexName=gsi_name,
        KeyConditionExpression=Key('City').eq(city)
    )

    return recs


def update_item_with_link(
        dynamodb,
        infections_table_name,
        patient_id,
        report_url):
    print(
        "\nRUNNING SOLUTION CODE:",
        "updateItemWithLink!",
        "Follow the steps in the lab guide to replace this method with your own implementation.")
    myTable = dynamodb.Table(infections_table_name)
    try:
        # Update item in table for the given patient_id key.
        resp = myTable.update_item(
            Key={'PatientId': str(patient_id)},
            UpdateExpression='set PatientReportUrl=:val1',
            ExpressionAttributeValues={':val1': {'S': report_url}})
        print("Item updated")
        print(
            "PatientId:{0}, PatientReportUrl:{1}".format(
                patient_id,
                report_url))
    except Exception as err:
        print("Error message {0}".format(err))
