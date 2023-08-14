# diabetesApp
__*Legal disclaimer* I assume no liability and warranty for this app/product. Use at your own risk and responsibly!__

## Requirements
- Android handy 
- For better usability, use a Voice Assistant
- Compatible with Bixby (Samsung) 
- Compatible with Win 10 / 11 Experimental at this point you have to compile it for your self


## Install
1. [Install the app](https://github.com/Marius1342/readBloodSugar_dexcom/releases)
2. Open the app, then go to Settings
3. Enter your Username and Password
4. Select a language
5. Save the settings 
6. Go into the Home menu, then go to Read.
7. If the app crash or show an error, [check this out](#error-dexcom)

## Compile your self
1. You need VS Studio 
2. Just execute the "compile.ps1"
3. Install the app

## Listen on SMS
1. Go into the settings and check the button, Check for SMS
2. Enter your own telephone number like +4916, without spaces and 0
3. Write in the last field the text, like DEXCOM, be careful all without white spaces
4. Save 
5. Setup in google a routine to send this SMS to your self with the text from step 3, like a voice routine
6. Test it
7. Tipp disable the Auto read at startup

## Error-Dexcom
1. Check if your dexcom share is active. (Invite somebody, you don't have to accept the request.)
2. Username or Password id wrong
3. The app is not update to date
4. Dexcom closed the api 
5. App cannot be installed, just delete the old and install the new. You have to reconfigure the app

## Example to use this with a voice assistant 
I personally use this app while riding my motorcycle. I have an intercom system installed, and I've created a routine for Google Assistant. When I say a specific keyword, Google Assistant sends me an SMS. Bixby is programmed to listen for this incoming SMS and then opens the app. The app, in turn, provides me with my blood sugar level information. You have to use the Smart Lock, otherwise you have to unlock your phone, when my intercom is connected my smartphone is unlocked.

## What comes next?
- :page_with_curl: UI update

## Help?
Fell free to ask for help, just write an email km3814837@gmail.com
