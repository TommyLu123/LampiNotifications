#!/usr/bin/env python
import json
import sys
import argparse
import colorsys
from time import sleep
import paho.mqtt.client as mqtt
from lamp_common import *
import ssl

MQTT_CLIENT_ID = "lamp_notification"


class LampiCmd(object):

    def __init__(self):
        self.received_lamp_state = {}
        self.client = mqtt.Client(client_id=MQTT_CLIENT_ID)
        self.client.tls_set(LAMPI_CA_PATH,
               certfile=BROKER_CERT_PATH,
               keyfile=BROKER_KEY_PATH,
               cert_reqs=ssl.CERT_REQUIRED,
               tls_version=ssl.PROTOCOL_TLSv1_2,
               ciphers=None)
        self.client.connect(MQTT_BROKER_HOST,
                            port=MQTT_BROKER_PORT,
                            keepalive=MQTT_BROKER_KEEP_ALIVE_SECS)

    def build_argument_parser(self):
        help_text = 'Any string for notification'

        parser = argparse.ArgumentParser()

        parser.add_argument('--notification', '-n', default=None, type=str, help=help_text)
        return parser

    def _print_lamp_state(self):
        self.client.publish(TOPIC_SET_LAMP_CONFIG,
                            '{"client": "lamp_notification", "notification": "Hello World and Hello World"}',
                            qos = 1)

    def update_lamp_state(self):
        args = self.build_argument_parser().parse_args()

        self.received_lamp_state['client'] = MQTT_CLIENT_ID

        self.received_lamp_state['notification'] = args.notification

        self.client.publish(TOPIC_SET_LAMP_CONFIG,
                            json.dumps(self.received_lamp_state),
                            qos = 1)
        # delay for a bit to allow message to be published then
        #  shutdown MQTT thread
        sleep(0.1)
        self.client.loop_stop()


def main():

    lampi_cmd = LampiCmd()

    if len(sys.argv) > 1:
        lampi_cmd.update_lamp_state()
    else:
        lampi_cmd._print_lamp_state()

if __name__ == '__main__':
    main()
