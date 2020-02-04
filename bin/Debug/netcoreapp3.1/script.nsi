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
    Delete "Uninstall.exe"
    DeleteRegKey /ifempty HKCU "Software\OlibPasswordManager"
    RMDir "$INSTDIR"

    !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
    
    Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk"
    RMDir "$SMPROGRAMS\$StartMenuFolder"
SectionEnd
