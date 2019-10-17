# Copyright 2017 Amazon Web Services, Inc. or its affiliates. All rights
# reserved.

import boto3
import time
import utils
import solution as dynamodb_solution

INFECTIONS_TABLE_NAME = utils.LAB_S3_INFECTIONS_TABLE_NAME
HTTP_STATUS_SUCCESS = 200


def remove_infections_table():
    # Name of the table
    table_name = INFECTIONS_TABLE_NAME

    # Removes the table_name from the region given as input
    rval = True
    if utils.is_table_active(table_name):
        print("{0} Table exists and will be removed.".format(table_name))
        try:
            rval = False
            dynamoDB = boto3.resource('dynamodb')
            table = dynamoDB.Table(table_name)
            resp = table.delete()
            time.sleep(15)
            if resp['ResponseMetadata'][
                    'HTTPStatusCode'] == HTTP_STATUS_SUCCESS:
                rval = True
                print("{0} Table has been deleted.".format(table_name))
        except Exception as err:
            print(
                "Existing table deletion failed: {0} Table".format(table_name))
            print("Error Message: {0}".format(err))
            rval = False
    return rval


def create_infections_table_wrapper():
    # Name of the table
    table_name = INFECTIONS_TABLE_NAME

    # Attributes for partition keys and sort key
    patient_id_attr_name = 'PatientId'
    city_attr_name = 'City'
    date_attr_name = 'Date'

    # Name of the global secondary index
    gsi_name = 'InfectionsByCityDate'

    # Create a DynamoDB table and global secondary index
    create_infections_table(
        table_name,
        gsi_name,
        patient_id_attr_name,
        city_attr_name,
        date_attr_name)


def create_infections_table(
        ddb_table_name,
        ddb_gsi_name,
        patient_id_attr_name,
        city_attr_name,
        date_attr_name):

    dynamodb = boto3.resource('dynamodb')

    # The variables below transform the arguments into the parameters expected by dynamodb.create_table.

    table_name = ddb_table_name
    key_schema = [{'AttributeName': patient_id_attr_name, 'KeyType': 'HASH'}]
    provisioned_throughput = {'ReadCapacityUnits': 5, 'WriteCapacityUnits': 10}

    global_secondary_indexes = [{
            'IndexName': ddb_gsi_name,
            'KeySchema': [
                {'AttributeName': city_attr_name, 'KeyType': 'HASH'},
                {'AttributeName': date_attr_name, 'KeyType': 'RANGE'}],
            'Projection': {'ProjectionType': 'ALL'},
            'ProvisionedThroughput': {'ReadCapacityUnits': 5, 'WriteCapacityUnits': 10}
    }]
    attribute_definitions = [
        {'AttributeName': patient_id_attr_name, 'AttributeType': 'S'},
        {'AttributeName': city_attr_name, 'AttributeType': 'S'},
        {'AttributeName': date_attr_name, 'AttributeType': 'S'}
    ]

    try:
        # Create a DynamoDB table with the parameters provided

        table = dynamodb.create_table(TableName=table_name,
                                      KeySchema=key_schema,
                                      AttributeDefinitions=attribute_definitions,
                                      ProvisionedThroughput=provisioned_throughput,
                                      GlobalSecondaryIndexes=global_secondary_indexes,
                                      )

    except Exception as err:
        print("{0} Table could not be created".format(table_name))
        print("Error message {0}".format(err))

    # Wait until the table is created before returning
    dynamodb.meta.client.get_waiter('table_exists').wait(TableName=table_name)


if __name__ == '__main__':
    print('===============================================================')
    print('Lab 3 DynamoDB - Infections Table creation')
    print('===============================================================')
    remove_infections_table()
    create_infections_table_wrapper()
    print(INFECTIONS_TABLE_NAME + " Table created")
