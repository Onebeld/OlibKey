![OlibKey](https://github.com/Onebeld/OlibKey/assets/44552715/997852a1-52eb-4492-b533-cff7b8085ea1)
# OlibKey
![GitHub release (latest by date)](https://img.shields.io/github/v/release/MagnificentEagle/OlibPasswordManager) ![GitHub](https://img.shields.io/github/license/MagnificentEagle/OlibPasswordManager)

![image](https://github.com/Onebeld/OlibKey/assets/44552715/c6f78465-0e3a-4757-ba03-903e93ec3e04)

OlibKey is a program for storing passwords and other data in encrypted form as a separate file called a vault (password manager).

This program must support AOT compilation.

## Features
- Strong AES-256 encryption with salt;
- Smart search by element;
- Grouping items by tags;
- Two-factor authentication;
- Markdown support;
- Creating custom sections with their own data for each item;
- Attaching files and images.

## Build
You must clone this repository and open the project solution. Now you can compile it and run it.

To compile with AOT, the command must be entered in the OlibKey project folder:

```batch
dotnet publish -r <platform> -c Release
```

<img src="https://github.com/Onebeld/OlibKey/assets/44552715/96ec5c40-00b5-4f30-813b-6611d55ac304" width="360" align="right"/>
