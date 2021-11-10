#define BLACK           0x00
#define BLUE            0xE0
#define RED             0x03
#define GREEN           0x1C
#define DGREEN          0x0C
#define YELLOW          0x1F
#define WHITE           0xFF
#define ALPHA           0xFE

#if defined (ARDUINO_ARCH_AVR)
#define SerialMonitorInterface Serial
#elif defined(ARDUINO_ARCH_SAMD)
#define SerialMonitorInterface SerialUSB
#endif

#include <TinyScreen.h>
#include <WiFi101.h>
#include <SPI.h>
#include <Wire.h>
#include <STBLE.h>
WiFiClient client;

#include "Wifi_information.h" 

TinyScreen display = TinyScreen(0);
char ssid[] = SECRET_SSID;        // your network SSID (name)
char pass[] = SECRET_PASS;    // your network password (use for WPA, or use as key for WEP)
int status = WL_IDLE_STATUS;     // the WiFi radio's status
int answer = 2;

void setup() 
{
  WiFi.setPins(8, 2, A3, -1);

  SerialMonitorInterface.begin(9600);
  while (!SerialMonitorInterface)
  {
    ;
  }
  if (WiFi.status() == WL_NO_SHIELD) {
    SerialMonitorInterface.println("WiFi shield not present");
    // don't continue:
    while (true);
  }
  while ( status != WL_CONNECTED) {
    SerialMonitorInterface.print("Attempting to connect to WPA SSID: ");
    SerialMonitorInterface.println(ssid);
    //SerialMonitorInterface.print("With password: " );
    //SerialMonitorInterface.print(pass);
    // Connect to WPA/WPA2 network:
    status = WiFi.begin(ssid, pass);

    // wait 10 seconds for connection:
    delay(10000);
  }
  SerialMonitorInterface.print("You're connected to the network");
  printCurrentNet();
  printWiFiData();
  Wire.begin();
  display.begin();
}

void loop() 
{
  printCurrentNet();

  while (client.available()) 
  {
    char c = client.read();
    SerialMonitorInterface.write(c);
  }
  /*
  Code to get temperature here
  */
  httpRequest(answer);
  delay(10000); // 5min

  //Initial Loading Screen
  display.setFlip(true);
  display.setFont(liberationSans_14ptFontInfo);
  display.setCursor(0,21);
  display.fontColor(BLUE,BLACK);
  display.print("Quiz");
  display.fontColor(YELLOW,BLACK);
  display.print("Circuit");
  delay(2000);
  display.clearWindow(0, 0, 96, 64);

  //Menu
  menu();
  
  //while(1)
  //{
    //Return int values
    //button(&answer);
    //print value  
  //}
  
}
void httpRequest(int answer) 
{
  String request = "POST /api/question/submitanswer/2 HTTP/1.1";

  // if there's a successful connection:
  IPAddress server(118,200,54,209);
  if (client.connect(server, 9000)) 
  {
    SerialMonitorInterface.println("connected to server");
    client.println(request);
    client.println("Host: 118.200.54.209");
    client.println("Content-Length: 1");
    client.println();
  }
  else {
    SerialMonitorInterface.println("Connection failed");
  }
}

void printWiFiData() 
{
  // print your WiFi shield's IP address:
  IPAddress ip = WiFi.localIP();
  SerialMonitorInterface.print("IP Address: ");
  SerialMonitorInterface.println(ip);
  SerialMonitorInterface.println(ip);

  // print your MAC address:
  byte mac[6];
  WiFi.macAddress(mac);
  SerialMonitorInterface.print("MAC address: ");
  printMacAddress(mac);
}

void printCurrentNet() 
{
  // print the SSID of the network you're attached to:
  SerialMonitorInterface.print("SSID: ");
  SerialMonitorInterface.println(WiFi.SSID());

  // print the MAC address of the router you're attached to:
  byte bssid[6];
  WiFi.BSSID(bssid);
  SerialMonitorInterface.print("BSSID: ");
  printMacAddress(bssid);

  // print the received signal strength:
  long rssi = WiFi.RSSI();
  SerialMonitorInterface.print("signal strength (RSSI):");
  SerialMonitorInterface.println(rssi);

  // print the encryption type:
  byte encryption = WiFi.encryptionType();
  SerialMonitorInterface.print("Encryption Type:");
  SerialMonitorInterface.println(encryption, HEX);
  SerialMonitorInterface.println();
}

void printMacAddress(byte mac[]) {
  for (int i = 5; i >= 0; i--) {
    if (mac[i] < 16) {
      SerialMonitorInterface.print("0");
    }
    SerialMonitorInterface.print(mac[i], HEX);
    if (i > 0) {
      SerialMonitorInterface.print(":");
    }
  }
  SerialMonitorInterface.println();
}

void menu()
{
  //Box Background
  display.drawRect(0, 0, 51, 32, TSRectangleFilled, RED);
  display.drawRect(0, 32, 50, 32, TSRectangleFilled, YELLOW);
  display.drawRect(50, 0, 49, 32, TSRectangleFilled, BLUE);
  display.drawRect(50, 32, 50, 32, TSRectangleFilled, DGREEN);

  //Option 1
  display.setFont(liberationSans_10ptFontInfo);
  display.setCursor(0,7);
  display.fontColor(WHITE,RED);
  display.print("Option 1");

  //Option 2
  display.setFont(liberationSans_10ptFontInfo);
  display.setCursor(0,40);
  display.fontColor(WHITE,YELLOW);
  display.print("Option 2");

  //Option 3
  display.setFont(liberationSans_10ptFontInfo);
  display.setCursor(50,7);
  display.fontColor(WHITE,BLUE);
  display.print("Option 3");

  //Option 4
  display.setFont(liberationSans_10ptFontInfo);
  display.setCursor(50,40);
  display.fontColor(WHITE,DGREEN);
  display.print("Option 4");  
}

void button(int *answer) 
{
  //Option 1
  if (display.getButtons(TSButtonUpperLeft)) 
  {
    *answer = 1;
    display.clearWindow(0, 0, 96, 64);
    display.drawRect(0, 0, 96, 64, TSRectangleFilled, RED);
    display.setFont(liberationSans_10ptFontInfo);
    display.setCursor(0,20);
    display.fontColor(WHITE,RED);
    display.print("Option 1 Choosen");
    delay(3000);
    display.clearWindow(0, 0, 96, 64);
    menu();   
  }
  else 
  {
    display.println("          ");
  }

  //Option 2
  if (display.getButtons(TSButtonLowerLeft)) 
  {
    *answer = 2;
    display.clearWindow(0, 0, 96, 64);
    display.drawRect(0, 0, 96, 64, TSRectangleFilled, YELLOW);
    display.setFont(liberationSans_10ptFontInfo);
    display.setCursor(0,20);
    display.fontColor(WHITE,YELLOW);
    display.print("Option 2 Choosen"); 
    delay(3000);
    display.clearWindow(0, 0, 96, 64);
    menu(); 
  }
  else 
  {
    display.println("          ");
  }

  //Option 3
  if (display.getButtons(TSButtonUpperRight)) 
  {
    *answer = 3;
    display.clearWindow(0, 0, 96, 64);
    display.drawRect(0, 0, 96, 64, TSRectangleFilled, BLUE);
    display.setFont(liberationSans_10ptFontInfo);
    display.setCursor(0,20);
    display.fontColor(WHITE,BLUE);
    display.print("Option 3 Choosen"); 
    delay(3000);
    display.clearWindow(0, 0, 96, 64);
    menu();
  }
  else 
  {
    display.println("          ");
  }

  //Option 4
  if (display.getButtons(TSButtonLowerRight)) 
  {
    *answer = 4;
    display.clearWindow(0, 0, 96, 64);
    display.drawRect(0, 0, 96, 64, TSRectangleFilled, DGREEN);
    display.setFont(liberationSans_10ptFontInfo);
    display.setCursor(0,20);
    display.fontColor(WHITE,DGREEN);
    display.print("Option 4 Choosen"); 
    delay(3000);
    display.clearWindow(0, 0, 96, 64);
    menu();
  }
  else 
  {
    display.println("          ");
  }
}
