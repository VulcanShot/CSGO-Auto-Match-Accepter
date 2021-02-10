# CSGO-Auto-Match-Accepter
## Description
This is a program that detects and automatically accepts your CS:GO competitive matches and sends you an SMS to let you know. It is completley VAC risk free since it does not interact with the CS:GO files directly.

## Requirements
You need to have the game in windowed mode and running it in any 16:9 or 16:10 resolution when searching for a match. However, for convenience, it is possible to make a bind to toggle between 16:9/16:10 resolutions and 4:3 resolutions, and between windowed and full screen mode. Check this posts if needed:

`mat_setvideomode` command is used to change these settings. It is used like this: `mat_setvideomode <width> <height> <0-1>`\
0 = fullscreen\
1 = windowed

Source:\
[Command explanation (Shrek vnature's comment)](https://steamcommunity.com/app/730/discussions/0/412447613579131479/)\
[Bind/toggle for the command](https://www.reddit.com/r/CounterStrikeBinds/comments/3b7fum/request_bind_to_change_screen_resolution/)

## Development and Extra Info
The program itself is made with C# winforms. It basically analyzes the color of some specific pixels and compares them to the color of the "Accept" button (Text is not involved, so it works in any language). For notifying the user, I'm using Twilio, which is a service to send custom SMS on demand. As for now I'm using a Twilio Trial account, so the number of messages are limited. Depending on how does the development goes I'll consider upgrading my account for a subscription one soon!
