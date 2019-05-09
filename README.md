# LampiNotifications
Lampi Notification API for laptop/desktop devices.

# Bridge Setup for General Use
1. Download Windows mosquitto (see http://www.steves-internet-guide.com/install-mosquitto-broker/) for a zip file with all necessary files
2. See conf.d for template file
3. Run a session utilizing `./mosquitto.exe -c "conf.d path" -v`

# Setting up Python
1. Install Python
2. Install `paho-mqtt`
3. Install `pyzt` onto Lampi itself (needed for correct time zones). Currently set to EST
4. Set up `lamp_common.py` utilizing template for Windows machine's `lamp_cmd.py`.
5. Drop `lampi_app.py` and replace old one in Lampi
6. Restart Lampi

# LampiNotifications
1. Install Visual Studio 2017 Update 5+
2. Run LampiNotifications
3. Set correct paths to `lampi_app.py` for command and `python.exe` for python path.

# Credits
Stefan Wick for his sample code utilized for LampiNotifications (see his blog posts https://stefanwick.com/2018/04/06/uwp-with-desktop-extension-part-1/)