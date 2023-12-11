''' <summary>
''' This Application Sarted from here
''' This is Used for to connect the SAP and dot net console
''' And create menu
''' 1)SetApplication
'''  It is used to get connection to SAP.
'''  Here we are using SAPbouiCOM.SboGuiApi, it is part of the SAP Business One Software Development Kit (SDK), and exposes user 
'''  interface elements of the SAP Business One front end
''' 2)SetFilter
'''   Sets an EventFilter object that filters in events on specific forms
''' 3)CookieConnect
'''  It is represent the one of the Company Data base
'''  It is enable to connect the company and Create the Business Object to use the company data base
''' 4)ConnectionContext
'''   It is used for to connect the company
''' 5)TableCreation
'''   It is used for to craete user tables and user define objects
''' 6)SetEventFilter
'''   User to filder the events for particilar forms
'''   it is used to high performance 
''' 7)AddXML
'''   It is used to add memu XML 
''' </summary>
''' <remarks></remarks>

Imports System.Net
Imports System.IO
Imports System.Security.Cryptography

#Region "... Main ..."

Module Root

    Sub Main()
        Try

            oGFun.SetApplication() '1)
            'oApplication.SetFilter(New SAPbouiCOM.EventFilter) '2)
            oApplication = oGFun.oApplication
            If Not oGFun.CookieConnect() = 0 Then '3)
                oApplication.MessageBox("DI Api Connection Failed")
                End
            End If
            'oGFun.HWKEY = HWKEY
            If Not oGFun.ConnectionContext() = 0 Then '4)
                System.Windows.Forms.MessageBox.Show("Failed to Connect Company", addonName)
                If oGFun.oCompany.Connected Then oGFun.oCompany.Disconnect()
                System.Windows.Forms.Application.Exit()
                End
            End If
            oCompany = oGFun.oCompany
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show("Application Not Found", addonName)
            System.Windows.Forms.Application.ExitThread()
        Finally
        End Try
        Try
            Try
                Dim oTableCreation As New TableCreation     '5)     
                oGFun.addReport_Layouttype("ActivityPlan")
                oGFun.addReport_Layouttype("CarriedOut")
                oGFun.addReport_Layouttype("AJobCard")
                oGFun.addReport_Layouttype("Breakdown")
                oGFun.addReport_Layouttype("PMCheckList")
                ''EventHandler.SetEventFilter()
                oGFun.AddXML("Menu.xml")                          '7)
                'oApplication.Forms.Item("1174000000").Items.Add("ll", SAPbouiCOM.BoFormItemTypes.it_EDIT)
                'Dim oMeniItem As SAPbouiCOM.MenuItem = EventHandler.oApplication.Menus.Item("INM")
                'oMeniItem.Image = System.Windows.Forms.Application.StartupPath & "\ManImage.bmp"
            Catch ex As Exception
                System.Windows.Forms.MessageBox.Show(ex.Message)
                System.Windows.Forms.Application.ExitThread()
            Finally
            End Try
            oApplication.StatusBar.SetText(addonName & " Addon Connected Successfully.......", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)

            'System.Windows.Forms.Application.Run()
            Application.Run()

        Catch ex As Exception
            oApplication.StatusBar.SetText(addonName & " Main Method Failed : ", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
        End Try
    End Sub
End Module

#End Region

