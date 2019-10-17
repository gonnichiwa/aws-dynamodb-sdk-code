# Copyright 2017 Amazon Web Services, Inc. or its affiliates. All rights
# reserved.

import csv
import boto3
from botocore.exceptions import ClientError
import utils
import solution as dynamodb_solution

BUCKET_NAME = utils.LAB_S3_BUCKET_NAME
BUCKET_REGION = utils.LAB_S3_BUCKET_REGION
INFECTIONS_DATA_FILE_KEY = utils.LAB_S3_INFECTIONS_DATA_FILE_KEY
FILE_NAME = utils.LAB_S3_FILE_KEY
INFECTIONS_TABLE_NAME = utils.LAB_S3_INFECTIONS_TABLE_NAME
DELIMITER = ","


def load_infections_data(
        tableName=INFECTIONS_TABLE_NAME,
        bucketRegion=BUCKET_REGION,
        bucket=BUCKET_NAME,
        fKey=INFECTIONS_DATA_FILE_KEY,
        FName=FILE_NAME):
    num_failures = 0
    try:
        # Create an S3 resource to download the infections data file from the
        # S3 bucket
        S3 = boto3.resource('s3', bucketRegion)
        try:
            # Check if you have permissions to access the bucket and then
            # retrieve a reference to it
            S3.meta.client.head_bucket(Bucket=bucket)
            myBucket = S3.Bucket(bucket)
        except ClientError as err:
            print("Could not find bucket")
            print("Error message {0}".format(err))
            num_failures = 9999
            return num_failures
        except Exception as err:
            print("Error message {0}".format(err))
            num_failures = 9999
            return num_failures

        try:
            # Download the CSV-formatted infections data file
            myBucket.download_file(fKey, FName)
        except Exception as err:
            print("Error message {0}".format(err))
            print("Failed to download the infections data file from S3 bucket")
            num_failures = 9999
            return num_failures
        print("Reading infections data from file, going to begin upload")
        with open(FName, newline='') as fh:
            reader = csv.DictReader(fh, delimiter=DELIMITER)
            # Create a DynamoDB resource
            dynamodb = boto3.resource('dynamodb')

            # Retrieve a reference to the Infections table
            infections_table = dynamodb.Table(INFECTIONS_TABLE_NAME)

            # Add an item for each row in the file
            for row in reader:
                try:
                    add_item_to_table(
                        infections_table,
                        row['PatientId'],
                        row['City'],
                        row['Date'])
                except Exception as err:
                    print("Error message {0}".format(err))
                    num_failures += 1
            print("Upload completed.")
    except Exception as err:
        print("Failed to add item in {0}".format(tableName))
        print("Error message {0}".format(err))
        num_failures = 9999
    return num_failures


def add_item_to_table(infections_table, patient_id, city, date):
    """Put an item in the infections table using the attribute values in the row object

    Keyword arguments:
    infections_table -- Table object
    patient_id -- Patient ID
    city -- City Name
    date -- Date
    """

    # TODO 1: Replace the solution with your own code
    dynamodb_solution.add_item_to_table(
        infections_table, patient_id, city, date)

if __name__ == '__main__':
    print("Going to load data")
    load_infections_data()
