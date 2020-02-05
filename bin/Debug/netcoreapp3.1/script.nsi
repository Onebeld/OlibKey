!include "MUI2.nsh"

Unicode true
Name "Olib Password Manager"
OutFile "OPMSetup.exe"
InstallDir "$PROGRAMFILES\OlibPasswordManager"
InstallDirRegKey HKCU "Software\OlibPasswordManager" ""
RequestExecutionLevel admin


Var StartMenuFolder

!define MUI_LANGDLL_ALLLANGUAGES
!define MUI_WELCOMEFINISHPAGE_BITMAP "F:\OlibSetup\Olib.bmp"
!define MUI_HEADERIMAGE_UNBITMAP "F:\OlibSetup\olib_cR0_icon.ico"
!define MUI_ICON "F:\OlibSetup\olib_cR0_icon.ico"
!define MUI_HEADERIMAGE_BITMAP "F:\OlibSetup\olib_cR0_icon.ico"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU" 
!define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\Modern UI Test" 
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"
!define MUI_FINISHPAGE_RUN "OlibPasswordManager.exe"
!define NETCoreInstall "dotnet-runtime-3.1.1-win-x86.exe"
!define NETCoreVersion "3.1"

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH



!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "German"
!insertmacro MUI_LANGUAGE "French"
!insertmacro MUI_LANGUAGE "Russian"
!insertmacro MUI_LANGUAGE "Armenian"
!insertmacro MUI_LANGUAGE "Ukrainian"

LangString DESC_OPM ${LANG_ENGLISH} "Install Olib Password Manager"
LangString DESC_OPM ${LANG_GERMAN} "Installation Olib Password Manager"
LangString DESC_OPM ${LANG_FRENCH} "L'installation Olib Password Manager"
LangString DESC_OPM ${LANG_RUSSIAN} "Установка Olib Password Manager"
LangString DESC_OPM ${LANG_ARMENIAN} "Տեղադրում Olib Password Manager"
LangString DESC_OPM ${LANG_UKRAINIAN} "Установка Olib Password Manager"

LangString DESC_Core ${LANG_ENGLISH} "Install .NET Core 3.1. Need to run the program."
LangString DESC_Core ${LANG_GERMAN} "Installation .NET Core 3.1. Erforderlich, um das Programm auszuführen."
LangString DESC_Core ${LANG_FRENCH} "L'installation .NET Core 3.1. Requis pour exécuter le programme"
LangString DESC_Core ${LANG_RUSSIAN} "Установка .NET Core 3.1. Необходим для запуска программы"
LangString DESC_Core ${LANG_ARMENIAN} "Տեղադրում .NET Core 3.1. Ծրագիրը գործարկելու համար պահանջվում է:"
LangString DESC_Core ${LANG_UKRAINIAN} "Установка .NET Core 3.1. Необхідний для запуску програми"

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${opm} $(DESC_OPM)
    !insertmacro MUI_DESCRIPTION_TEXT ${core} $(DESC_Core)
!insertmacro MUI_FUNCTION_DESCRIPTION_END

Section "Olib Password Manager" opm
    SetOutPath $INSTDIR
    File OlibPasswordManager.exe
    File OlibPasswordManager.dll
    File Newtonsoft.Json.dll
    File OlibPasswordManager.deps.json
    File OlibPasswordManager.runtimeconfig.dev.json
    File OlibPasswordManager.runtimeconfig.json

    WriteUninstaller "$INSTDIR\Uninstall.exe"
    WriteRegStr HKCU "Software\OlibPasswordManager" "" $INSTDIR

    !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
        CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
        CreateShortcut "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
    !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

Section ".NET Core 3.1" core
    SetOutPath $INSTDIR
    File /oname=${NETCoreInstall} ${NETCoreInstall}
    DetailPrint "Starting Microsoft .NET Core v${NETCoreVersion} Setup..."
    ExecWait "${NETCoreInstall}"
    Delete "${NETCoreInstall}"
SectionEnd

Section "Uninstall"
    Delete "OlibPasswordManager.exe"
    Delete "OlibPasswordManager.dll"
    Delete "Newtonsoft.Json.dll"
    Delete "OlibPasswordManager.deps.json"
    Delete "OlibPasswordManager.runtimeconfig.dev.json"
    Delete "OlibPasswordManager.runtimeconfig.json"
    DeleteRegKey /ifempty HKCU "Software\OlibPasswordManager"
    Delete "Uninstall.exe"
    RMDir "$INSTDIR"

    !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
    
    Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk"
    RMDir "$SMPROGRAMS\$StartMenuFolder"
SectionEnd