# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [3.1.0] - 2020-08-04
### Added
- Added click animations
- Added Noto Sans font
- Added tabs in settings and for storages
- Added information about the number of items and bases in the list to StatusBar
- With the addition of tabs, two new commands have been added: "Save all storages", "Unlock all storages" and "Lock all storages"
- Added "Turquoise" theme
- Added Drag and Drop support
- Added more hotkeys
- Added automatic blocking of storages (except for those who have an activated "Reminder" element)
- New item type: Notes

### Changed
- Moving to .NET 5 Preview 7
- Migrating to Avalonia 0.10.0-preview2
- Open menu in rounded corners will not show a piece of background
- Button icon color now changes when pressed
- Styled ScrollBar
- The operation of the encryptor has been changed (you can specify the iteration and the number of encryption procedures) **We need to create the repository again!**
- Supplemented by Personal Data
- In the password generator, when copying an empty field, it now first generates and then copies
- With the addition of tabs, you can now open multiple storages at a time
- Message box with OK button, you can press Enter to close
- Redrawn logo, simplified geometry in vector images
- In the settings, you can now change the autosave interval and message display duration in the StatusBar
- If the field with username is empty, Email will be shown on the main screen
- In the list of items, if you click on an empty field, the selection will be inactive and will redirect to the initial screen

### Fixed
- Fixed crash in search (element name was null)
- TextBox does not focus when editing folder
- With the addition of the font, the symbols of the Armenian language were fixed on some OS ("squares")
- Fixed some hotkey interactions on locked storage

## [3.0.0] - 2020-07-16
### Added
- Custom Elements Added
- Added status bar
- Added new theme "Mysterious"

### Changed
- Upgrade to .NET 5 Preview 6
- Rewritten application on Avalonia UI
- Updated AboutWindow
- Now the search button is next to the Add button
- No animation in StartPage
- Now opens .olib files by default

## [2.1.0] - 2020-04-30
### Added
- New item Reminder added.
- New option for quick rendering in the settings.
- Added ability to create custom folders.
- Search added.
- New icons added.

### Changed
- Updated user interface.
- Base storage method changed.
- Animations on the start page are simplified.
- Now, when changing the theme, the resource with icons is reloaded.
- The name of the database now does not contain an extension.
- Minor changes in the logical component.

### Fixed
- Partially fixed display of text (blur).
- If you lock the store, and then create a new one, the unlock button will become active.

## [2.0.0] - 2020-03-04
### Added
- Added buttons for moving items.
- Added icons for items in the list.
- Now the name of the storage is displayed in the program itself.
- Cancel button added in some windows

### Changed
- Completely rewritten application.
- Start page changed.
- Changed the program logo and name.
- Encryption logic changed.
- Montserrat font removed.
- The change in the item is displayed in real time.
- Changed links in About window.
- Changed interface in Password Generator.

### Fixed
- Fixed autorun program.
- The color of the pointer in the text input field is fixed, now it depends on the theme (dark and white).

## [1.3.0.295] - 2020-02-15
### Added
- Added notification of changes, item additions, and storage creation.
- Added save indicator.
- Added "Copy" button in password generator.
- Added button to go to Facebook in About.
- Autostart option added.
- Added warning text in settings.
- Added the name of the file storage in the program header.

### Changed
- Information in the About window is centered.
- Two separate components for viewing passwords is now one.
- Autosave is now activated every 2 minutes.

### Fixed
- Fixed a bug where the program ends sometimes after copying some text.
- Critical bug fixed, If you open one more with open storage, do not unlock it and save it, then when you open the latter, what is opened earlier is displayed.
- Fixed a bug where, after changing the login or bank card, the changes were not displayed

## [1.2.0.235] - 2020-02-05
### Added
- Dark theme added.
- Added taskbar icon.
- Added the ability to encrypt passports.
- Added autosave storage in three minutes.
- Added a theme selection to the settings window.
- Added the ability to lock and unlock storage.
- Added a small window when you click on the icon in the taskbar, allowing you to view passwords and nothing more.

### Changed
- Changed the appearance of the settings window.
- The appearance of the program is slightly changed.
- The storage encryption logic has been slightly changed.
- The localization is slightly changed.
- Now you will not be able to add a login until you create or unlock a storage.
- The background color of the windows is changed.

### Fixed
- Fixed a bug when selecting an item from the list, instead of a login, data for a bank card was displayed.

## [1.1.0.145] - 2020-01-18
### Added
- Additional hotkeys added.
- Added item type change.
- Added notification function about new updates.
- 5 new languages added: English, Armenian, French, Ukrainian and German.
- Added saving of "Bank card" element.
- Added language selection to the settings item.

### Changed
- Changed the start background of the program.
- Difficulty indicator colors changed.
- About window changed.

### Fixed
- Fixed termination of the program when there are no connected storages yet.

## [1.0.0.90] - 2020-01-10
- First release.
