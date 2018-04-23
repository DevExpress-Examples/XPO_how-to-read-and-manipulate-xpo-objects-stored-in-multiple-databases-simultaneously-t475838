Imports DevExpress.Data.Filtering
Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports DevExpress.Xpo.Metadata
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ConsoleApplication
    Friend Class Program

        Private Shared DB1dal As IDataLayer
        Private Shared DB2dal As IDataLayer

        Shared Sub Main(ByVal args() As String)
            'Setup data layers
            DB1dal = SetupDB1()
            DB2dal = SetupDB2()

            'connect DB1
            Using session1 As New UnitOfWork(DB1dal)
                Dim contact As Contact = session1.FindObject(Of Contact)(CriteriaOperator.Parse("LastName = ?", "Smith"))
                'connect DB2
                Using session2 As New UnitOfWork(DB2dal)
                    Dim task As New Task(session2)
                    task.Subject = "Improve"
                    task.SetContact(contact)
                    session2.CommitChanges()
                End Using
            End Using

            'check 1
            Using session1 As New UnitOfWork(DB1dal)
                Dim contact As Contact = session1.FindObject(Of Contact)(CriteriaOperator.Parse("LastName = ?", "Smith"))
                'connect DB2
                Using session2 As New UnitOfWork(DB2dal)
                    Dim tasks = contact.GetTasks(session2)
                    For Each task In tasks
                        Console.WriteLine(task.Subject)
                    Next task
                End Using
            End Using

            Console.WriteLine()

            'check 2
            'connect DB2
            Using session2 As New UnitOfWork(DB2dal)
                Dim task = session2.FindObject(Of Task)(CriteriaOperator.Parse("Subject = ?", "Improve"))
                Using session1 As New UnitOfWork(DB1dal)
                    Dim contact As Contact = task.GetContact(session1)
                    Console.WriteLine("{0} {1}", contact.FirstName, contact.LastName)
                End Using
            End Using

            Console.ReadLine()
        End Sub

        Private Shared Function SetupDB1() As IDataLayer
            Dim connString As String = MSSqlConnectionProvider.GetConnectionString("(local)", "SampleDB1")
            Dim dict As XPDictionary = New ReflectionDictionary()
            dict.CollectClassInfos(GetType(Contact))
            Dim dal As IDataLayer = XpoDefault.GetDataLayer(connString, dict, AutoCreateOption.DatabaseAndSchema)
            Using uow As New UnitOfWork(dal)
                uow.UpdateSchema(GetType(Contact))
                If uow.FindObject(Of Contact)(Nothing) Is Nothing Then
                    Dim contact1 As New Contact(uow) With {.FirstName = "John", .LastName = "Doe"}
                    Dim contact2 As New Contact(uow) With {.FirstName = "Paul", .LastName = "Smith"}
                    uow.CommitChanges()
                End If
            End Using
            Return dal
        End Function

        Private Shared Function SetupDB2() As IDataLayer
            Dim connString As String = MSSqlConnectionProvider.GetConnectionString("(local)", "SampleDB2")
            Dim dict As XPDictionary = New ReflectionDictionary()
            dict.CollectClassInfos(GetType(Task))
            Dim dal As IDataLayer = XpoDefault.GetDataLayer(connString, dict, AutoCreateOption.DatabaseAndSchema)
            Using uow As New UnitOfWork(dal)
                uow.UpdateSchema(GetType(Task))
            End Using
            Return dal
        End Function
    End Class
End Namespace
