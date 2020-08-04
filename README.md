![](https://github.com/MagnificentEagle/OlibKey/blob/master/ForRepository/OlibKeyLogo.png)
# OlibKey
![GitHub release (latest by date)](https://img.shields.io/github/v/release/MagnificentEagle/OlibPasswordManager) ![GitHub](https://img.shields.io/github/license/MagnificentEagle/OlibPasswordManager)

![](https://github.com/MagnificentEagle/OlibPasswordManager/blob/master/ForRepository/ScreenProgram.png)

OlibKey is free software that allows you to store passwords, notes, documents and other important personal data on your computer. Powered by Microsoft .NET 5 Prewiev 6 using  Avalonia UI. With OlibKey, you can store passwords on your computer **using AES encryption**.

For feedback and the latest news, use the links below:

[Facebook](https://www.facebook.com/olibpasswordmanager/?ref=aymt_homepage_panel&eid=ARAA4Mn8v3ZSafKTICcoEgyj6FJ8K-uk8ZuGpGJKJFaayB8eXGf4IHUWrmIMkLhctf9m2oBQFtj7_vKm) - the rest of the community.

[VKontakte](https://www.facebook.com/olibpasswordmanager/?ref=aymt_homepage_panel&eid=ARAA4Mn8v3ZSafKTICcoEgyj6FJ8K-uk8ZuGpGJKJFaayB8eXGf4IHUWrmIMkLhctf9m2oBQFtj7_vKm) - only Russian community.

## Why this project?
There are many different password managers. One is free, the other is paid, the third has the most functionality, and the fourth has less. **My goal of this project** is to combine the best of other managers and implement it together.

## Features:
* Tabs and interaction with multiple bases;
* Supports x86, x64 and ARM architectures;
* Localization support (6 languages supported):
   * English;
   * French;
   * German;
   * Russian;
   * Armenian;
   * Ukrainian.
* Cross-platform;
* Modern (built on the latest technology);
* Simple

## OlibKey Versions
OlibKey has several versions: Noyon and Legacy
### OlibKey code name Noyon
This version is compiled using AOT CoreRT, which allows you to run applications without dependence and the speed is equal to C/C++, supports only x64 and is recommended for use. Additional testing on real machines is required.
#### Requirements:
* All platforms except x86 and ARM ([see here](https://github.com/dotnet/core/blob/master/release-notes/5.0/5.0-supported-os.md)) (For some reason, it only works on the Windows version 10 family)

### OlibKey Legacy
This version supports other platforms (x86, ARM), .NET Runtime is required to run. macOS and Linux does not support x86
#### Requirements:
* Almost all platforms ([see here](https://github.com/dotnet/core/blob/master/release-notes/5.0/5.0-supported-os.md))
* .NET Runtime 5 Preview 7;
