Imports DevExpress.Data.Filtering
Imports DevExpress.Xpo
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ConsoleApplication

    Public Class Contact
        Inherits XPLiteObject

        Private fContactID As Integer
        <Key(True)> _
        Public Property ContactID() As Integer
            Get
                Return fContactID
            End Get
            Set(ByVal value As Integer)
                SetPropertyValue("ContactID", fContactID, value)
            End Set
        End Property
        Private fFirstName As String
        Public Property FirstName() As String
            Get
                Return fFirstName
            End Get
            Set(ByVal value As String)
                SetPropertyValue("FirstName", fFirstName, value)
            End Set
        End Property
        Private fLastName As String
        Public Property LastName() As String
            Get
                Return fLastName
            End Get
            Set(ByVal value As String)
                SetPropertyValue("LastName", fLastName, value)
            End Set
        End Property

        Public Function GetTasks(ByVal foreignSession As Session) As XPCollection(Of Task)
            Return New XPCollection(Of Task)(foreignSession, New BinaryOperator("ContactID", Me.ContactID))
        End Function

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
    End Class

End Namespace
