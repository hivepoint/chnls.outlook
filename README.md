# chnls.outlook
Outlook plugin for @channels

### Versioning

1. major
2. minor 
3. dot - this increments each time we release to the customer
 2. odd number is development build
 1. even number is customer build
4. build number - this increments each time we publish a build

### Build/Deployment Process
1. update the version information in properties
2. 


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


