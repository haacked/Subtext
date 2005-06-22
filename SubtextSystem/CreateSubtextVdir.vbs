'----------------------------------------------
'Creates a Virtual Directory pointing for the Subtext blog engine.
'Assumes this script is located in the SubtextSystem folder.
'
'Adapted from DasBlog (Scott Hanselman scott@hanselman.com and Omar Shahine et all)
'ORIGINAL LICENSE INFORMATION BELOW!
'modified by Phil Haack (2005.06.18)
' Copyright (c) 2005 Phil Haack.  Under the BSD License.
'----------------------------------------------
vDirName = "Subtext.Web" ' Replace with the name of the virtual directory

Set shell = Wscript.CreateObject( "WScript.Shell" )

' Get the name of the current directory
Set fso = WScript.CreateObject( "Scripting.FileSystemObject" )
vDirPath = fso.GetFolder( ".\SubtextSolution\Subtext.Web" ).Path

' Does this IIS application already exist in the metabase?
On Error Resume Next
Set objIIS = GetObject( "IIS://localhost/W3SVC/1/Root/" & vDirName )
If Err.Number = 0 Then
    result = shell.Popup( "A virtual directory named " & vDirName & " already exists. " & vbCrLf & vbCrLf & "Would you like it re-mapped for this sample?", 0 ,"Remap Virtual Directory?", 4 + 32 )' 4 = YesNo & 32 = Question
    If result = 6 Then ' 6 = Yes
        DeleteVirtualDirectory vDirName 
    Else
        WScript.Quit
    End If
End If

'Using IIS Administration object , turn on script/execute permissions and define the virtual directory as an 'in-process application. 
Set objIIS  = GetObject( "IIS://localhost/W3SVC/1/Root" )
Set vDirObj = objIIS.Create( "IISWebVirtualDir", vDirName )

vDirObj.Path                  = vDirPath
vDirObj.AuthNTLM              = True
vDirObj.AccessRead            = True
vDirObj.AccessWrite           = True 
vDirObj.AccessScript          = True
vDirObj.AccessExecute         = True
vDirObj.AuthAnonymous         = True
'vDirObj.AnonymousUserName     = owner
vDirObj.AnonymousPasswordSync = True
vDirObj.AppCreate True
vDirObj.SetInfo 

If Err.Number > 0 Then
    shell.Popup Err.Description, 0, "Error", 16 ' 16 = Stop
    WScript.Quit
Else
    shell.Popup "Virtual directory created." & vbCrLf & "setting folder permissions ..." , 1, "Status", 64 ' 64 = Information
End If

' Get the name of the account for the anonymous user in IIS
owner = vDirObj.AnonymousUserName

' Change necessary folder permissions using CACLS.exe
aclCmd = "cmd /c echo y| CACLS "
aclCmd = aclCmd & """" & vDirPath & """"
aclCmd = aclCmd & " /E /G " & owner & ":C"
rtc = shell.Run( aclCmd , 0, True )

aclCmd = "cmd /c echo y| CACLS "
aclCmd = aclCmd & """" & vDirPath & """"
aclCmd = aclCmd & " /E /G ""VS Developers"":C"
rtc = shell.Run( aclCmd , 0, True )

If Err.Number > 0 Then
    shell.Popup Err.Description, 0, "Error", 16 ' 16 = Stop
    WScript.Quit
Else
    res = vDirName & " has been created at" & vbCrLf & vDirPath
    shell.Popup res, 0, "All done", 64 ' 64 = Information
End If

Sub DeleteVirtualDirectory( NameOfVdir )

    Set iis = GetObject("IIS://localhost/W3SVC/1/Root")
    iis.Delete "IISWebVirtualDir", vDirName
    
    If Err.Number = 0 Then
        shell.Popup "Virtual directory deleted sucessfully", 1, "Status", 64 ' 64 = Information
    Else
        shell.Popup Err.Description, 0, "Error", 16 ' 16 = Stop
    End If

End Sub


'Copyright (c) 2003, newtelligence AG. (http://www.newtelligence.com)
'Original BlogX Source Code: Copyright (c) 2003, Chris Anderson (http://simplegeek.com)
'All rights reserved.
' 
'Redistribution and use in source and binary forms, with or without modification, are permitted 
'provided that the following conditions are met: 
' 
'(1) Redistributions of source code must retain the above copyright notice, this list of 
'conditions and the following disclaimer. 
'(2) Redistributions in binary form must reproduce the above copyright notice, this list of 
'conditions and the following disclaimer in the documentation and/or other materials 
'provided with the distribution. 
'(3) Neither the name of the newtelligence AG nor the names of its contributors may be used 
'to endorse or promote products derived from this software without specific prior 
'written permission.
'     
'THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS 
'OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
'AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
'CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
'DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
'DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
'IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT 
'OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'-------------------------------------------------------------------------
'
'Original BlogX source code (c) 2003 by Chris Anderson (http://simplegeek.com)
'
'newtelligence is a registered trademark of newtelligence Aktiengesellschaft.
'
'For portions of this software, the some additional copyright notices may apply 
'which can either be found in the license.txt file included in the source distribution
'or following this notice. 
'
'
'--------------------------------------------------------------------
'
'Copyright (c) 2003 Chris Anderson
'
'This software is provided 'as-is', without any express or implied warranty. 
'In no event will the authors be held liable for any damages arising from the 
'use of this software.
'
'Permission is granted to anyone to use this software for any purpose, 
'including commercial applications, and to alter it and redistribute it freely, 
'subject to the following restrictions:
'
'1. The origin of this software must not be misrepresented; you must not claim 
'that you wrote the original software. If you use this software in a product, 
'an acknowledgment in the product documentation would be appreciated but is not 
'required.
'
'2. Altered source versions must be plainly marked as such, and must not be 
'misrepresented as being the original software.
'
'3. This notice may not be removed or altered from any source distribution.