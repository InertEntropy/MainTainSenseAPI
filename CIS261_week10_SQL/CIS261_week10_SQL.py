""" Jamie Isaac"""
""" CIS WEEK 10 SQL """
""" 02/24/2023 """
import mysql.connector

mydb = mysql.connector.connect(
    host = "localhost"
    user = "yourusername"
    password = "yourpassword"
)

mycursor = mydb.cursor()

mycursor.execute('''CREATE TABLE phone (
                  phone_id INT,
                  country_code INT NOT NULL,
                  phone_number INT NOT NULL,
                  phone_type VARCHAR(12),
                  PRIMARY KEY(phone_id)
                )''')
data = [
    (1, 1, 234567890, "MOBILE"),
    (2, 44, 987654321, "LANDLINE"),
    (3, 49, 1234567890, "CELLULAR"),
    (4, 1, 567890123, "MOBILE"),
]

# Insert sample data into the table
mycursor.executemany("INSERT INTO phone (phone_id, country_code, phone_number, phone_type) VALUES (?, ?, ?, ?)", data)

# Select phone numbers from US (country code 1)
mycursor.execute("SELECT phone_number FROM phone WHERE country_code = ?", (1,))
rows = mycursor.fetchall()

# Print the selected phone numbers
print("US phone numbers:")
for row in rows:
    print(row[0])

# Update phone_type from "CELLULAR" to "MOBILE"
mycursor.execute("UPDATE phone SET phone_type = 'MOBILE' WHERE phone_type = 'CELLULAR'")
mycursor.commit()  # Commit the changes to the database

# Delete rows with country code "XX"
mycursor.execute("DELETE FROM phone WHERE country_code = ?", ("XX",))
mycursor.commit()

# Drop the phone table 
mycursor.execute("DROP TABLE phone")  # Uncomment to drop the table

# Close the connection
mycursor.close()


