; 该脚本使用 HM VNISEdit 脚本编辑器向导产生

; 安装程序初始定义常量
!define PRODUCT_NAME "Hisense Transtech Guidance Screen"
!define PRODUCT_VERSION "2.0.3"
!define PRODUCT_PUBLISHER "Hunan Yestech Optoelectronic Co., LTD."
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\HisenseTranstechGuidanceScreen.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI.nsh"

; MUI 预定义常量
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\orange-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\orange-uninstall.ico"

; 语言选择窗口常量设置
!define MUI_LANGDLL_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
!define MUI_LANGDLL_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "NSIS:Language"

; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!insertmacro MUI_PAGE_FINISH

; 安装卸载过程页面
!insertmacro MUI_UNPAGE_INSTFILES

; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "SimpChinese"

; 安装预释放文件
!insertmacro MUI_RESERVEFILE_LANGDLL
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI 现代界面定义结束 ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Hisense CSJT V2.0.3.exe"
InstallDir "$APPDATA\Hunan Yestech\HisenseTranstechGuidanceScreen"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUnInstDetails show

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite try
  File "HisenseTranstechGuidanceScreen\bin\HisenseTranstechGuidanceScreen.exe.config"
  CreateDirectory "$SMPROGRAMS\Hisense Transtech Guidance Screen"
  CreateShortCut "$SMPROGRAMS\Hisense Transtech Guidance Screen\Hisense Transtech Guidance Screen.lnk" "$INSTDIR\HisenseTranstechGuidanceScreen.exe"
  CreateShortCut "$DESKTOP\Hisense Transtech Guidance Screen.lnk" "$INSTDIR\HisenseTranstechGuidanceScreen.exe"
  File "HisenseTranstechGuidanceScreen\bin\HisenseTranstechGuidanceScreen.exe"
  File "HisenseTranstechGuidanceScreen\bin\DaemonService.exe.config"
  File "HisenseTranstechGuidanceScreen\bin\DaemonService.exe"
  SetOutPath "$INSTDIR\DLL"
  File /r "HisenseTranstechGuidanceScreen\bin\DLL\*.*"
  SetOutPath "$INSTDIR\Tmp"
  File /r "HisenseTranstechGuidanceScreen\bin\Tmp\*.*"
  SetOutPath "$INSTDIR\Data"
  SetOverwrite off
  File "HisenseTranstechGuidanceScreen\bin\Data\Setting.xml"
SectionEnd

Section -AdditionalIcons
  SetOutPath $INSTDIR
  CreateShortCut "$SMPROGRAMS\Hisense Transtech Guidance Screen\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\HisenseTranstechGuidanceScreen.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\HisenseTranstechGuidanceScreen.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#

Function .onInit
  !insertmacro MUI_LANGDLL_DISPLAY
FunctionEnd

/******************************
 *  以下是安装程序的卸载部分  *
 ******************************/

Section Uninstall
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\Data\Setting.xml"
  Delete "$INSTDIR\DaemonService.exe"
  Delete "$INSTDIR\DaemonService.exe.config"
  Delete "$INSTDIR\HisenseTranstechGuidanceScreen.exe"
  Delete "$INSTDIR\HisenseTranstechGuidanceScreen.exe.config"

  Delete "$SMPROGRAMS\Hisense Transtech Guidance Screen\Uninstall.lnk"
  Delete "$DESKTOP\Hisense Transtech Guidance Screen.lnk"
  Delete "$SMPROGRAMS\Hisense Transtech Guidance Screen\Hisense Transtech Guidance Screen.lnk"

  RMDir "$SMPROGRAMS\Hisense Transtech Guidance Screen"
  RMDir "$INSTDIR\Data"

  RMDir /r "$INSTDIR\Tmp"
  RMDir /r "$INSTDIR\DLL"

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#

Function un.onInit
!insertmacro MUI_UNGETLANGUAGE
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "您确实要完全移除 $(^Name) ，及其所有的组件？" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从您的计算机移除。"
FunctionEnd
