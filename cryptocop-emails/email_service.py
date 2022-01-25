import pika
import requests
import json
import os

try:
    connection_string = os.environ['PRODUCTION_ENV']
except KeyError:
    connection_string = 'localhost'

connection = pika.BlockingConnection(pika.ConnectionParameters(connection_string))
channel = connection.channel()
exchange_name = 'order_exchange'
create_order_routing_key = 'create_order'
email_queue_name = 'email_queue'
email_template = '<h2>Thank you for your order!</h2><table><thead><tr style="background-color: rgba(155, 155, 155, .2)"><th>Currency</th><th>Quantity</th><th>Unit Price</th><th>Row price</th></tr></thead><tbody>%s</tbody></table>'

# Declare the exchange, if it doesn't exist
channel.exchange_declare(exchange=exchange_name, exchange_type='direct', durable=True)
# Declare the queue, if it doesn't exist
channel.queue_declare(queue=email_queue_name, durable=True)
# Bind the queue to a specific exchange with a routing key
channel.queue_bind(exchange=exchange_name, queue=email_queue_name, routing_key=create_order_routing_key)

def send_simple_message(to, subject, body):
    return requests.post(
        "https://api.mailgun.net/v3/sandboxa4986c632dc94afda64149a11a4171e5.mailgun.org/messages",
        auth=("api", "92b70c37b092145e09130509d3db827b-2fbe671d-7e7a34d5"),
        data={"from": "Mailgun Sandbox <postmaster@sandboxa4986c632dc94afda64149a11a4171e5.mailgun.org>",
              "to": to,
              "subject": subject,
              "html": body})

def send_order_email(ch, method, properties, data):
    parsed_msg = json.loads(data)
    email = parsed_msg['Email']
    items = parsed_msg['OrderItems']
    items_html = ''.join([ '<tr><td>%s</td><td>%.02f</td><td>%.02f</td><td>%.02f</td></tr>' % (item['ProductIdentifier'], float(item['Quantity']), float(item['UnitPrice']), float(item['TotalPrice'])) for item in items ])
    buyer_html = '<p>Name: %18s<br>Address: %18s %d<br>City: %18s<br>Zip code: %18s<br>Country: %18s<br>Date of order: %18s</p>' % (parsed_msg['FullName'], parsed_msg['StreetName'], int(parsed_msg['HouseNumber']), parsed_msg['City'], parsed_msg['ZipCode'], parsed_msg['Country'], parsed_msg['OrderDate'])
    representation = email_template % items_html + buyer_html
    send_simple_message(parsed_msg['Email'], 'Successful order!', representation)

channel.basic_consume(queue=email_queue_name, on_message_callback=send_order_email, auto_ack=True)
channel.start_consuming()
connection.close()
