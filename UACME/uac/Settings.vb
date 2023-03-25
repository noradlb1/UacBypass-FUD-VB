Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading.Tasks

Namespace UACME.uac
	Friend Class Settings
		<DllImport("shell32.dll", SetLastError := True)> _
		Public Shared Function IsUserAnAdmin() As <MarshalAs(UnmanagedType.Bool)> Boolean
		End Function

		Public Shared Sub CheckA()
			Console.WriteLine(IsUserAnAdmin())

			'Console.ReadKey(); //
		End Sub
	End Class
End Namespace
