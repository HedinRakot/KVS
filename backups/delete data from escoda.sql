delete from [dbo].[DeregistrationOrder]
delete from [dbo].[RegistrationOrder]

UPDATE [dbo].[Vehicle]
 SET [CurrentRegistrationId] = NULL

delete from [dbo].[Registration]
delete from [dbo].[Vehicle]
delete from [dbo].[CarOwner]

delete from [dbo].[OrderInvoice]
delete from [dbo].[InvoiceItem]
delete from [dbo].[OrderItem]

UPDATE [dbo].[PackingList]
 SET [OldOrderId] = NULL

delete from [dbo].[Order]
delete from [dbo].[PackingList]

delete from [dbo].[DocketList]

delete from [dbo].[InvoiceNumber]

delete from [dbo].[Mailinglist]


delete from [dbo].[Invoice]

delete from [dbo].[Document]
delete from [dbo].[InvoiceAccountBackup]
delete from [dbo].[Systemlog]