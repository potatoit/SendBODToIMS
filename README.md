# SendBODToIMS

This is a quick little hack to illustrate how to send a BOD to ION via the ION APIs (through IMS).  

Though you can do this with a tool like Postman, you would need to escape the BOD itself.  This little application will escape the BOD.

The .ionapi Client ID (ci) must match Client ID in the ION IMS connector.

You can see more info here:
https://potatoit.kiwi/2020/12/21/send-bod-to-ims/

## Annoyances
As mentioned, this was a quick and dirty hack and there is a lot that really should be done to make this more useful.

* code is quick and pulled from various other sources, it could do with some rework
* UI is clunky
* have to manually adjust the message ID
* From logical ID should be stored so when you restart the application it is retrieved
* manually have to set the BOD when it should be derrived from the pasted content

## Download / Install
You can download the compiled code from the SendBODToIMS -> Published.  The zip file contains the executable, just extract the files in to a directory and run from there.  This does need a current .Net 4.7.x installed to run.
