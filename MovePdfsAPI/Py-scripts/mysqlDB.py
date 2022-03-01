import mysql.connector
from CREDENTIALS import HOST, PORT, USER, PASSWORD, DATABASE

db = mysql.connector.connect(

    host=HOST,
    port= PORT,
    user=USER,
    passwd=PASSWORD,
    database=DATABASE
)

mycursor = db.cursor()

def insertDoc(filename, hospital_name, keywords, reg_num=None, status=None):
    # print("inserting doc...")
    query = """INSERT INTO `documents` (`name`, `hospital_name`, `registrationNum`, `status`, `keywords`) 
    VALUES (%s, %s, %s, %s, %s);"""

    mycursor.execute(query,(filename, hospital_name, reg_num, status, keywords))
    db.commit()

def insertElementsList(elem_list):
    # print("elements inserting")
    query = """INSERT INTO `elements` (`sequence_number`, `keywords`, `content`, `item_type`, `doc_id`) 
        VALUES (%s, %s, %s, %s, %s);"""
    
    mycursor.executemany(query, elem_list)
    
    db.commit()

def getLastInsertID(table_name):
    query = "SELECT MAX(id) FROM "+ table_name +";"
    mycursor.execute(query)

    for x in mycursor:
        return x[0]


