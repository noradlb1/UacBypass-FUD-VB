Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms

Namespace UACME
	Friend NotInheritable Class Program

		Private Sub New()
		End Sub

		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		<STAThread> _
		Shared Sub Main()
			Console.WriteLine("Good Luck (:")
			Thread.Sleep(1000)
			uac.regen.BoopMe()
			Console.WriteLine("Nice Computer Here (:")
		End Sub

	End Class
End Namespace
