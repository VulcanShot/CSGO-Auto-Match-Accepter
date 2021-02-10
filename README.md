# CSGO-Auto-Match-Accepter
## Description
This is a program that detects and automatically accepts your CS:GO competitive matches and sends you an SMS to let you know. It is completley VAC risk free since it does not interact with the games files directly.

## Requirements
You need to have the game in windowed mode and running it in any 16:9 or 16:10 resolution when searching for a match. However, for convenience, it is possible to make a bind to toggle between 16:9/16:10 resolutions and 4:3 resolutions, and between windowed and full screen mode.

**Command to change resolution:**

The command to change the resolution is: `mat_setvideomode`\
How to use it: `mat_setvideomode width height x`\
Replace "x" with 0 = fullscreen or 1 = windowed.

**Bind the previous command as a toggle:**

`alias "res1920" "mat_setvideomode 1920 1080 0; bind x res1280"`\
`alias "res1280" "mat_setvideomode 1280 960 0; bind x res1920"`\
`bind x "res1920"`\
Replace "x" with the desiered key.


**Credits:**\
[shrek_vnature](https://steamcommunity.com/profiles/76561198037904541)\
[u/FoxlinkThunder](https://www.reddit.com/user/FoxlinkThunder/)

## Development and other info
The program itself is made with C# winforms. It basically analyzes the color of some specific pixels and compares them to the color of the "Accept" button (Text is not involved, so it works in any language). For notifying the user, I'm using Twilio, which is a service to send custom SMS on demand. As for now I'm using a Twilio Trial account, so the number of messages are limited. Depending on how does the development goes I'll consider upgrading my account for a subscription one soon!
