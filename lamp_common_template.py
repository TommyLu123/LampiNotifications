import paho.mqtt.client
# RENAME THIS TO lamp_common.py

DEVICE_ID = 'DEVICE_ID' # macid of raspberry

# MQTT Topic Names
TOPIC_SET_LAMP_CONFIG = "devices/" + DEVICE_ID + "/lamp/set_config"

# Certifications - Path assuming Windows
LAMPI_CA_PATH = "CERT" # ex: c:/lampi_ca.crt
BROKER_CERT_PATH = "BROKER.CRT" # ex: c:/deviceid_broker.crt
BROKER_KEY_PATH = "BROKER.KEY" # ex: c:/deviceid_broker.key

# MQTT Broker Connection info
MQTT_VERSION = paho.mqtt.client.MQTTv311
MQTT_BROKER_HOST = "amazonaws.com" # ex: ec2-1-23-123-123.compute-1amazonaws.com
MQTT_BROKER_PORT = 8883
MQTT_BROKER_KEEP_ALIVE_SECS = 60
