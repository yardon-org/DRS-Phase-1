```js

## Installation

	* Once you have checked-out the source code, create a new folder anywhere on the system and call it 'drs-backend-phase1'
	* Load up the solution in Visual Studio, right-click on the DRS-Phase1-Backend project and select 'Publish' from the menu.
	* On the screen that pops-up, select 'Profile' and click on 'Custom', enter 'Release' when it asks for a Profile name.
	* The screen will move to 'Connection' and under the 'Publish' method select 'File System'.
	* In 'Target Location' selec the folder you created in the first step.
	* Click 'Next'
	* Expand the 'File Publish Options' and select 'Delete all existing files prior to publish'.
	* Press 'Publish
	
	* Go to IIS Manager
	* Create a new site named whatever you like.
	* Set the physical path to the folder you created in the first step.
	* Set the port to whatever you like - probably not Port 80 as it maybe in use.
	
	* In the source code there is a client for testing calls to the backend server - 'OAuthHMAC-Test-Client'
	* If you are going to use the client, make sure you change you AD username/password in the Program.cs file.
	* Also, make sure that the 'apiBaseAddress' in Program.cs is set the address:port of the site you created in IIS.

## Settings

  * The 'privatekey' and 'initializevector' strings used in the HMAC encryption process are stored in the 'appSettings' segment of the web.config in the 'DRS-Phase1-Backend' project.  
  * If you want to test the web api without requiring HMAC or OAuth authentication, you need to comment-out the '[HMACAuthentication]' and '/[Authorize(Roles = "PERSONNEL")]' attributes in the controller classes.
  * Currently, two databases are used by the Wep API - DRS and PAF - the web.config currently points to these on AG_SQLStage02.
'''
