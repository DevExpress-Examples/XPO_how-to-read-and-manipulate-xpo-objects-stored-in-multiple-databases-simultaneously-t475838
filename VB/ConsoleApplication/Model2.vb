Imports DevExpress.Xpo
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Text

Namespace ConsoleApplication

    'class persisted in DB2
    Public Class Task
        Inherits XPLiteObject

        Private fTaskID As Integer
        <Key(True)> _
        Public Property TaskID() As Integer
            Get
                Return fTaskID
            End Get
            Set(ByVal value As Integer)
                SetPropertyValue("TaskID", fTaskID, value)
            End Set
        End Property
        Private fSubject As String
        Public Property Subject() As String
            Get
                Return fSubject
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Subject", fSubject, value)
            End Set
        End Property
        Private fDone As Boolean
        Public Property Done() As Boolean
            Get
                Return fDone
            End Get
            Set(ByVal value As Boolean)
                SetPropertyValue("Done", fDone, value)
            End Set
        End Property

        <Browsable(False)> _
        Public ContactID? As Integer

        Public Function GetContact(ByVal foreignSession As Session) As Contact
            Return If(ContactID.HasValue, foreignSession.GetObjectByKey(Of Contact)(ContactID), Nothing)
        End Function
        Public Sub SetContact(ByVal contact As Contact)
            ContactID = If(contact Is Nothing, DirectCast(Nothing, Integer?), CType(contact.ContactID, Integer?))
        End Sub

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
    End Class

End Namespace
