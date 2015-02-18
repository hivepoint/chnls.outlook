# chnls.outlook
Outlook plugin for @channels

### Versioning

1. major
2. minor 
3. dot - this increments each time we release to the customer
 2. odd number is development build
 1. even number is customer build
4. build number - this increments each time we publish a build

### Code Signing
When publishing, you need to have a code signing cert.  One is currently stored in the repository.  If this needs to be regenerated, the command to generate it is:
```
openssl pkcs12 -export -inkey HivePoint.key -in HivePoint.crt -out HivePoint.p12
```

### Build/Deployment Process
#### Build
1. update the version information in properties per version rules above
2. Rebuild - Menu --> Build --> Rebuild Solution
3. Select and right click on Project
4. Select "Publish ADX Project"
5. Select the version that you want to publish
6. Click "Populate"
7. Scroll down to "fav.ico" and use the combo box to select "AppIcon"
8. Update "provider url"
   1. for production, enter http://downloads.emailchannels.com/outlook/chnlsoutlookaddin.application
   2. for dev, enter http://downloads.emailchannels.net/outlook/chnlsoutlookaddin.application
   3. NOTE there is no US version, just dev and production
9. Ensure that the cert file is the right file, the path will be something like "C:\Users\sevogle\Documents\git\chnls.outlook\code-signing\HivePoint.p12"
10. Enter password for key cert.  This password is in LastPass "HivePoint Code Signing Key"
11. The timestamp server should be "http://timestamp.comodoca.com/authenticode"
12. Click Publish

This will result in the files being published into the Publish directory of the project
`C:\Users\sevogle\Documents\git\chnls.outlook\ChnlsOutlookAddIn\Publish`

#### Deploy
1. Open this directory in Explorer
2. rename setup.ext to EmailChannelsSetup.exe
3. create a new directory `chnsl.outlook-<version>`
4. move all the files into that folder
    1. EmailChannelsSetup.exe
	2. chnsloutlookaddin.application
	3. folder with version number
5. Right click on that file and select "Send to compressed zipped file"
    1. this will generate a zip file `chnls.outlook-<version>`
6. Open github to create release (optional)
    1. https://github.com/SevogleAdmin/chnls.outlook/releases/new
	2. select the branch that you want to associate the release with
	3. enter tag version alpha/<version> or beta/<version> or just <version>
	4. Enter release title - include version and anything else
	5. Add any description that you want
	6. Drag or select the zip file from above to add it to the release. 
	7. Wait until it is uploaded
	8. if this release is going to be used internally, i.e. it has an "odd" dot version number, select "this is a pre-release"
	9. Click Publish Release
7. Upload to S3
    1. Open Firefix and use S3 fox
	2. Select the directory you created above `C:\Users\sevogle\Documents\git\chnls.outlook\ChnlsOutlookAddIn\Publish\chnls.outlook-1.0.1.21`
	3. select the target directory in S3
	    1. Production: `downloads.emailchannels.com/outlook`
	    2. Development: `downloads.emailchannels.net/outlook`
    4. Select the directory with your version nubmer `1.0.1.21` and click the upload button, the arrow pointing to the right
	5. Wait until that completes
	6. Select `chnlsoutlookaddin.application` and select upload and overwrite when prompted
	7. Select `EmailChannelsSetup.exe` and select upload and overwrite when prompted
8. Send out an email
    1. Production ops.deploy.outlook.C@channels.hivepoint.com
    2. Development ops.deploy.outlook.D@channels.hivepoint.com


### General Email Channels Information

The Email Channels Outlook Add-in is designed to connect Outlook with the Email Channels service. Briefly, Email Channels is a rethink on how to work with emails in groups.  At it's simplest, users can create channels, each channel has an email address and other people are able to subscribe or watch the content in that channel.  On a team, one would probably want to create a domain to group your channels and have them all have your name space.

More information can be found at http://emailchannels.us (currently you will need to go to this page first https://emailchannels.us#cookie: )

Once you have gone there, there is more information in the "learn more" section, either clicked at the bottom or https://emailchannels.us/#!learn:

An account will need to be created.

When the add-in is first run, it needs to be pointed at this server.  To do that:
1. Got to "About" in the Email Channels tab group
2. CTRL+SHIFT+CLICK on the Logo (this should bring up a new dialog)
3. Select Beta (EmailChannels.net)

### Add-in information

The add-in adds three main functions to Outlook
1. A sidebar is added to the main explorer window, this sidebar hosts the EmailChannels web site and provides bi-directional communications
  1. The web application communicates with Outlook by updating the web browser location.  Outlook accepts this information then cancels the navigation.
  2. Outlook communicates back to the web application by executing specific javascript commands.
2. A sidebar in the compose window
  1. The compose window is all about showing users which channels have been selected
  2. Allowing the user to find/select other channels
  3. Showing the user who is watching or subscribed channels that the message is being sent to
3. Sending current messages to channels
  1. This is done by selecting messages and selecting the "Add to channels button" and then selecting various channels.
  2. Messages are forwarded as attachments to the EmailChannels server

The add-in keeps a local cache of all it's state, to help with compose, this is done through the PropertiesService, which writes them out to files.


### Development Environment

* License information can be found here:
 *  https://docs.google.com/a/hivepoint.com/document/d/1CFuvsVw3Q38PkSRCIaqde0vU6LGbOAsHNZIHBGUuaeQ/edit
* Windows 8 or 10
* Add-in Express for Office and .NET, Professional
 * https://www.add-in-express.com/downloads/adxnet.php
* SourceTree + Git installed
* Outlook 2010 or 2013
 * 2013 is Office 365 install
 * 2010 is physical media install
 * We don't support 2007
* Visual Studio 2013
  * Visual Studio will need to always be stared as *administrator*, done by right clicking on the icon and selecting `run as administrator`.
  * select the appropriate Outlook Exec as the debug target
  * Everything should be for .Net 4.0

  
To register and unregister the add-in with outlook, right click on the project and select the appropriate menu items

