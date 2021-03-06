USE [master]
GO
/****** Object:  Database [Test]    Script Date: 21.07.2015 10:10:29 ******/
CREATE DATABASE [Test]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'KVS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Test.mdf' , SIZE = 210944KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'KVS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Test_log.ldf' , SIZE = 1047104KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Test] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Test].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Test] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Test] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Test] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Test] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Test] SET ARITHABORT OFF 
GO
ALTER DATABASE [Test] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Test] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [Test] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Test] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Test] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Test] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Test] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Test] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Test] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Test] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Test] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Test] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Test] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Test] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Test] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Test] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Test] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Test] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Test] SET RECOVERY FULL 
GO
ALTER DATABASE [Test] SET  MULTI_USER 
GO
ALTER DATABASE [Test] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Test] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Test] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Test] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'KVS', N'ON'
GO
USE [Test]
--GO
--/****** Object:  User [IIS APPPOOL\escodaPool]    Script Date: 21.07.2015 10:10:29 ******/
--CREATE USER [IIS APPPOOL\escodaPool] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [IIS APPPOOL\.NET v4.5]    Script Date: 21.07.2015 10:10:29 ******/
--CREATE USER [IIS APPPOOL\.NET v4.5] FOR LOGIN [IIS APPPOOL\.NET v4.5] WITH DEFAULT_SCHEMA=[dbo]
--GO
--ALTER ROLE [db_owner] ADD MEMBER [IIS APPPOOL\escodaPool]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [IIS APPPOOL\escodaPool]
--GO
--ALTER ROLE [db_owner] ADD MEMBER [IIS APPPOOL\.NET v4.5]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [IIS APPPOOL\.NET v4.5]
--GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Accounts](
	[AccountId] [int] IDENTITY(1, 1) NOT NULL,
	[AccountNumber] [varchar](50) NOT NULL,
	[CustomerId] [int] NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Adress]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Adress](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Street] [varchar](100) NOT NULL,
	[StreetNumber] [varchar](10) NULL,
	[Zipcode] [varchar](10) NOT NULL,
	[City] [varchar](50) NOT NULL,
	[Country] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Adress] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BankAccount]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BankAccount](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[BankName] [varchar](100) NULL,
	[Accountnumber] [varchar](50) NULL,
	[BankCode] [varchar](50) NULL,
	[IBAN] [varchar](100) NULL,
	[BIC] [varchar](100) NULL,
 CONSTRAINT [PK_BankAccount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BIC_DE]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BIC_DE](
	[Bankleitzahl] [nvarchar](50) NULL,
	[Merkmal] [nvarchar](50) NULL,
	[Bezeichnung] [nvarchar](500) NULL,
	[PLZ] [nvarchar](50) NULL,
	[Ort] [nvarchar](500) NULL,
	[Kurzbezeichnung] [nvarchar](500) NULL,
	[PAN] [nvarchar](50) NULL,
	[BIC] [nvarchar](50) NULL,
	[Pruefziffer] [nvarchar](50) NULL,
	[Datensatznummer] [nvarchar](50) NULL,
	[Aenderungskennzeichen] [nvarchar](50) NULL,
	[Bankleitzahlloeschung] [nvarchar](50) NULL,
	[Nachfolgebankleitzahl] [nvarchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CarOwner]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CarOwner](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Firstname] [varchar](50) NULL,
	[AdressId] [int] NULL,
	[ContactId] [int] NULL,
	[BankAccountId] [int] NULL,
 CONSTRAINT [PK_CarOwner] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ChangeLogColumNames]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChangeLogColumNames](
	[TableName] [varchar](255) NOT NULL,
	[ColumnNames] [varchar](max) NULL,
	[IdColumnName] [varchar](250) NULL,
 CONSTRAINT [PK_ChangeLogColumNames] PRIMARY KEY CLUSTERED 
(
	[TableName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Contact](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Phone] [varchar](50) NULL,
	[Fax] [varchar](50) NULL,
	[MobilePhone] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CostCenter]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CostCenter](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[BankAccountId] [int] NULL,
	[CostcenterNumber] [varchar](20) NOT NULL,
 CONSTRAINT [PK_CostCenter] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_CostCenter] UNIQUE NONCLUSTERED 
(
	[CustomerId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[ContactId] [int] NULL,
	[AdressId] [int] NOT NULL,
	[VAT] [decimal](4, 2) NOT NULL,
	[InvoiceAdressId] [int] NOT NULL,
	[InvoiceDispatchAdressId] [int] NOT NULL,
	[TermOfCredit] [int] NULL,
	[CustomerNumber] [varchar](100) NOT NULL,
	[Debitornumber] [varchar](100) NULL,
	[MatchCode] [varchar](100) NULL,
	[eVB_Number] [varchar](50) NULL,
	[InternalId] [int] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CustomerProduct]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerProduct](
	[CustomerId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
 CONSTRAINT [PK_CustomerProduct] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DeregistrationOrder]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeregistrationOrder](
	[OrderNumber] [int] NOT NULL,
	[VehicleId] [int] NOT NULL,
	[RegistrationId] [int] NOT NULL,
 CONSTRAINT [PK_DeregistrationOrder] PRIMARY KEY CLUSTERED 
(
	[OrderNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DocketList]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DocketList](
	[DocketListNumber] [int] IDENTITY(30000,1) NOT NULL,
	[DispatchOrderNumber] [varchar](50) NULL,
	[IsSelfDispatch] [bit] NULL,
	[IsPrinted] [bit] NULL,
	[RecipientAdressId] [int] NOT NULL,
	[Recipient] [varchar](50) NOT NULL,
	[DocumentId] [int] NULL,
 CONSTRAINT [PK_DocketList] PRIMARY KEY CLUSTERED 
(
	[DocketListNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Document]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Document](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[DokumentTypeId] [int] NOT NULL,
	[FileName] [varchar](500) NOT NULL,
	[Data] [varbinary](max) NOT NULL,
	[MimeType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DocumentConfiguration]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DocumentConfiguration](
	[Id] [varchar](50) NOT NULL,
	[Text] [varchar](8000) NOT NULL,
 CONSTRAINT [PK_DocumentConfiguration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DocumentType]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DocumentType](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_DocumentType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[eabwcommunication]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eabwcommunication](
	[ordernumber] [int] NOT NULL,
	[stamp] [datetime] NOT NULL,
	[request] [text] NULL,
	[response] [text] NULL,
 CONSTRAINT [PK_eabwcommunication] PRIMARY KEY CLUSTERED 
(
	[ordernumber] ASC,
	[stamp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EabwCurrent]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EabwCurrent](
	[Ordernumber] [int] NOT NULL,
	[xkfz] [text] NOT NULL,
 CONSTRAINT [PK_EabwCurrent] PRIMARY KEY NONCLUSTERED 
(
	[Ordernumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[eabwdata]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eabwdata](
	[Ordernumber] [int] NOT NULL,
	[iban] [nvarchar](50) NULL,
	[kontoinhaber] [nvarchar](150) NULL,
	[firma] [nvarchar](150) NULL,
	[nachname] [nvarchar](150) NULL,
	[vorname] [nvarchar](150) NULL,
	[geburtsort] [nvarchar](150) NULL,
	[geburtsdatum] [date] NULL,
	[geschlecht] [nvarchar](10) NULL,
	[plz] [nvarchar](10) NULL,
	[staat] [nvarchar](10) NULL,
	[wohnort] [nvarchar](150) NULL,
	[strasse] [nvarchar](150) NULL,
	[hausnummer] [nvarchar](150) NULL,
	[zusatz] [nvarchar](150) NULL,
	[kennzeichen] [nvarchar](50) NULL,
	[erstzulassung] [date] NULL,
	[evb] [nvarchar](150) NULL,
	[zb2nr] [nvarchar](10) NULL,
	[schluesselhersteller] [nvarchar](50) NULL,
	[fin] [nvarchar](50) NULL,
	[schluesseltyp] [nvarchar](50) NULL,
	[schluesselvarianteversion] [nvarchar](50) NULL,
	[pruefziffervarianteversion] [nvarchar](50) NULL,
	[artgenehmigung] [nvarchar](1) NULL,
	[schluesselfahrzeugklasse] [nvarchar](50) NULL,
	[schluesselaufbau] [nvarchar](50) NULL,
	[status] [nvarchar](50) NOT NULL,
	[hauptuntersuchung] [date] NULL,
	[alteskennzeichen] [nvarchar](50) NULL,
	[vorfall] [nvarchar](50) NULL,
	[mappennummer] [int] NULL,
 CONSTRAINT [PK_eabwdata] PRIMARY KEY CLUSTERED 
(
	[Ordernumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EabwHistory]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EabwHistory](
	[Ordernumber] [int] NOT NULL,
	[Stamp] [datetime] NOT NULL,
	[Xkfz] [text] NOT NULL,
	[Source] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_EabwHistory] PRIMARY KEY CLUSTERED 
(
	[Ordernumber] ASC,
	[Stamp] ASC,
	[Source] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Invoice](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[IsPrinted] [bit] NOT NULL,
	[PrintDate] [datetime] NULL,
	[UserId] [int] NULL,
	[InvoiceRecipient] [varchar](100) NOT NULL,
	[InvoiceRecipientAdressId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[DocumentId] [int] NULL,
	[InvoiceText] [varchar](500) NULL,
	[discount] [decimal](18, 0) NULL,
	[InvoiceType] [int] NULL,
	[canceled] [bit] NULL,
 CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvoiceAccountBackup]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InvoiceAccountBackup](
	[IIACCID] [int] IDENTITY(1, 1) NOT NULL,
	[InvoiceItemId] [int] NOT NULL,
	[AccountNumber] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvoiceIDInvoiceRunID]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceIDInvoiceRunID](
	[InvoiceId] [int] NULL,
	[InvoiceRunId] [int] NULL,
	[Complete] [bit] NULL,
	[Updated] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InvoiceItem]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InvoiceItem](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[InvoiceId] [int] NOT NULL,
	[Name] [varchar](500) NOT NULL,
	[Amount] [money] NOT NULL,
	[Count] [int] NOT NULL,
	[VAT] [decimal](4, 2) NOT NULL,
	[OrderItemId] [int] NULL,
	[CostcenterId] [int] NULL,
	[AccountNumber] [varchar](50) NULL,
 CONSTRAINT [PK_InvoiceItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvoiceItemAccountItem]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InvoiceItemAccountItem](
	[IIACCID] [int] IDENTITY(1, 1) NOT NULL,
	[InvoiceItemId] [int] NOT NULL,
	[RevenueAccountText] [varchar](50) NOT NULL,
 CONSTRAINT [PK_InvoiceItemAccountItem] PRIMARY KEY CLUSTERED 
(
	[IIACCID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvoiceNumber]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceNumber](
	[InvoiceId] [int] NOT NULL,
	[Number] [int] IDENTITY(10000,1) NOT NULL,
 CONSTRAINT [PK_InvoiceNumber] PRIMARY KEY CLUSTERED 
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InvoiceRunReport]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceRunReport](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[CustomerId] [int] NULL,
	[InvoiceTypeId] [int] NULL,
	[CreateDate] [datetime] NULL,
	[FinishedDate] [datetime] NULL,
	[InvoiceRunProgress] [int] NULL,
 CONSTRAINT [PK_InvoiceRunReport] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InvoiceTypes]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InvoiceTypes](
	[ID] [int] IDENTITY(1, 1) NOT NULL,
	[InvoiceTypeName] [varchar](255) NULL,
	[contraction] [varchar](50) NULL,
 CONSTRAINT [PK_InvoiceTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LargeCustomer]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LargeCustomer](
	[CustomerId] [int] NOT NULL,
	[MainLocationId] [int] NULL,
	[SendInvoiceToMainLocation] [bit] NOT NULL,
	[SendInvoiceByEmail] [bit] NOT NULL,
	[OrderFinishedNoteSendType] [tinyint] NOT NULL,
	[SendOrderFinishedNoteToCustomer] [bit] NULL,
	[SendOrderFinishedNoteToLocation] [bit] NULL,
	[SendPackingListToCustomer] [bit] NULL,
	[SendPackingListToLocation] [bit] NULL,
	[PersonId] [int] NULL,
	[InvoiceTypesID] [int] NULL,
 CONSTRAINT [PK_LargeCustomer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LargeCustomerRequiredField]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LargeCustomerRequiredField](
	[LargeCustomerId] [int] NOT NULL,
	[RequiredFieldId] [int] NOT NULL,
 CONSTRAINT [PK_LargeCustomerRequiredField] PRIMARY KEY CLUSTERED 
(
	[LargeCustomerId] ASC,
	[RequiredFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Location]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[CustomerId] [int] NULL,
	[Name] [nvarchar](400) NOT NULL,
	[ContactId] [int] NULL,
	[AdressId] [int] NOT NULL,
	[SuperLocationId] [int] NULL,
	[VAT] [decimal](4, 2) NULL,
	[InvoiceAdressId] [int] NULL,
	[InvoiceDispatchAdressId] [int] NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Location] UNIQUE NONCLUSTERED 
(
	[CustomerId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LogType]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LogType](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_LogType_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Mailinglist]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Mailinglist](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[CustomerId] [int] NULL,
	[LocationId] [int] NULL,
	[Email] [varchar](150) NOT NULL,
	[MailinglistTypeId] [int] NULL,
 CONSTRAINT [PK_CustomerMailinglist] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MailinglistType]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MailinglistType](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_MailinglistType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Make]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Make](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[HSN] [varchar](4) NOT NULL,
 CONSTRAINT [PK_Make] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Order]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Order](
	[OrderNumber] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[LocationId] [int] NULL,
	[OrderTypeId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ExecutionDate] [datetime] NULL,
	[UserId] [int] NULL,
	[ErrorReason] [varchar](100) NULL,
	[HasError] [bit] NULL,
	[PackingListNumber] [int] NULL,
	[FreeText] [varchar](200) NULL,
	[HasFinishedNoteBeenSent] [bit] NULL,
	[FinishDate] [datetime] NULL,
	[Zulassungsstelle] [int] NULL,
	[Geprueft] [bit] NULL,
	[ReadyToSend] [bit] NULL,
	[DocketListNumber] [int] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Order] UNIQUE NONCLUSTERED 
(
	[Ordernumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderInvoice]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderInvoice](
	[OrderNumber] [int] NOT NULL,
	[InvoiceId] [int] NOT NULL,
 CONSTRAINT [PK_OrderInvoice] PRIMARY KEY CLUSTERED 
(
	[OrderNumber] ASC,
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderItem](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[OrderNumber] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductName] [varchar](500) NOT NULL,
	[Amount] [money] NOT NULL,
	[Count] [int] NOT NULL,
	[NeedsVAT] [bit] NOT NULL,
	[Status] [int] NOT NULL,
	[CostCenterId] [int] NULL,
	[SuperOrderItemId] [int] NULL,
	[IsAuthorativeCharge] [bit] NOT NULL,
 CONSTRAINT [PK_AuftragPosition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderItemStatus]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderItemStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_OrderItemStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderStatus]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_OrderStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderType]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderType](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [nchar](10) NULL,
 CONSTRAINT [PK_OrderType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PackingList]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PackingList](
	[PackingListNumber] [int] IDENTITY(20000,1) NOT NULL,
	[DispatchOrderNumber] [varchar](50) NULL,
	[IsSelfDispatch] [bit] NULL,
	[IsPrinted] [bit] NULL,
	[RecipientAdressId] [int] NOT NULL,
	[Recipient] [varchar](50) NOT NULL,
	[DocumentId] [int] NULL,
	[OldOrderNumber] [int] NULL,
 CONSTRAINT [PK_PackingList] PRIMARY KEY CLUSTERED 
(
	[PackingListNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PathPosition]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PathPosition](
	[ID] [int] IDENTITY(1, 1) NOT NULL,
	[Path] [varchar](255) NULL,
	[PostionName] [varchar](255) NULL,
 CONSTRAINT [PK_PathPosition] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Permission](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PermissionProfile]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PermissionProfile](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](255) NOT NULL,
 CONSTRAINT [PK_PermissionProfile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PermissionProfilePermission]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionProfilePermission](
	[PermissionProfileId] [int] NOT NULL,
	[PermissionId] [int] NOT NULL,
 CONSTRAINT [PK_PermissionProfilePermission] PRIMARY KEY CLUSTERED 
(
	[PermissionProfileId] ASC,
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Person]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Person](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[Title] [varchar](20) NULL,
	[Gender] [varchar](1) NULL,
	[Extension] [varchar](255) NULL,
	[salutation] [varchar](50) NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Price]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Price](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[LocationId] [int] NULL,
	[Amount] [money] NOT NULL,
	[AuthorativeCharge] [money] NULL,
 CONSTRAINT [PK_Price] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Price] UNIQUE NONCLUSTERED 
(
	[ProductId] ASC,
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PriceAccount]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PriceAccount](
	[PriceId] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
 CONSTRAINT [PK_PriceAccount] PRIMARY KEY CLUSTERED 
(
	[PriceId] ASC,
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[ProductCategoryId] [int] NULL,
	[Name] [varchar](150) NOT NULL,
	[ItemNumber] [varchar](10) NOT NULL,
	[OrderTypeId] [int] NOT NULL,
	[RegistrationOrderTypeId] [int] NULL,
	[NeedsVAT] [bit] NOT NULL,
	[IsLocked] [bit] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Product_1] UNIQUE NONCLUSTERED 
(
	[ItemNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_ProductCategory] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Registration]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Registration](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[CarOwnerId] [int] NOT NULL,
	[VehicleId] [int] NOT NULL,
	[Licencenumber] [varchar](15) NULL,
	[GeneralInspectionDate] [datetime] NULL,
	[EmissionCode] [varchar](5) NULL,
	[RegistrationDate] [date] NULL,
	[RegistrationDocumentNumber] [varchar](20) NULL,
	[eVBNumber] [varchar](7) NULL,
 CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RegistrationLocation]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RegistrationLocation](
	[ID] [int] IDENTITY(1, 1) NOT NULL,
	[RegistrationLocationName] [varchar](255) NULL,
	[RegistrationLocationAdressId] [int] NULL,
 CONSTRAINT [PK_RegistrationLocation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RegistrationOrder]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RegistrationOrder](
	[OrderNumber] [int] NOT NULL,
	[VehicleId] [int] NOT NULL,
	[RegistrationId] [int] NOT NULL,
	[Licencenumber] [varchar](15) NULL,
	[PreviousLicencenumber] [varchar](15) NULL,
	[RegistrationOrderTypeId] [int] NOT NULL,
	[eVBNumber] [varchar](7) NULL,
 CONSTRAINT [PK_RegistrationOrder] PRIMARY KEY CLUSTERED 
(
	[OrderNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_RegistrationOrder] UNIQUE NONCLUSTERED 
(
	[RegistrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RegistrationOrderType]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RegistrationOrderType](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RegistrationType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_RegistrationOrderType] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RequiredField]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RequiredField](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[OrderTypeId] [int] NOT NULL,
 CONSTRAINT [PK_RequiredFields] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RequiredFieldTranslation]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RequiredFieldTranslation](
	[RequiredFieldId] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK_RequiredFieldTranslation] PRIMARY KEY CLUSTERED 
(
	[RequiredFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SmallCustomer]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SmallCustomer](
	[CustomerId] [int] NOT NULL,
	[PersonId] [int] NOT NULL,
	[BankAccountId] [int] NULL,
 CONSTRAINT [PK_SmallCustomer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Systemlog]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Systemlog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[LogUserId] [int] NOT NULL,
	[TableName] [varchar](50) NULL,
	[TableProperty] [varchar](50) NULL,
	[ReferenceId] [int] NULL,
	[ChildReferenceId] [int] NULL,
	[Text] [varchar](max) NULL,
	[Exception] [varchar](max) NULL,
	[LogTypeId] [int] NOT NULL,
 CONSTRAINT [PK_Systemlog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[temp_Customers2]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[temp_Customers2](
	[Kundennummer] [varchar](255) NULL,
	[Debitorenkonto] [varchar](255) NULL,
	[Anrede] [varchar](255) NULL,
	[Name] [varchar](255) NULL,
	[Matchcode] [varchar](255) NULL,
	[Vorname] [varchar](255) NULL,
	[Firma] [varchar](255) NULL,
	[Zusatz] [varchar](255) NULL,
	[Ansprechpartner] [varchar](255) NULL,
	[Strasse] [varchar](255) NULL,
	[Hausnummer] [varchar](50) NULL,
	[Zip] [varchar](50) NULL,
	[Ort] [varchar](255) NULL,
	[Land] [varchar](255) NULL,
	[Telefon] [varchar](255) NULL,
	[Telefax] [varchar](255) NULL,
	[EMAIL] [varchar](250) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[temp_Products]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[temp_Products](
	[Artikelnummer] [varchar](255) NULL,
	[Bezeichnung] [nvarchar](250) NULL,
	[Warengruppe] [nvarchar](250) NULL,
	[KontoInland] [nvarchar](250) NULL,
	[Preis] [nchar](10) NULL,
	[Type] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TranslationRequiered]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TranslationRequiered](
	[Id] [int] IDENTITY(1, 1)NOT NULL,
	[Name] [varchar](255) NULL,
	[RequiredFieldId] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[ContactId] [int] NULL,
	[Login] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Salt] [varchar](50) NOT NULL,
	[LastLogin] [datetime] NULL,
	[IsLocked] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserPermission]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPermission](
	[UserId] [int] NOT NULL,
	[PermissionId] [int] NOT NULL,
 CONSTRAINT [PK_UserPermission] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserPermissionProfile]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPermissionProfile](
	[UserId] [int] NOT NULL,
	[PermissionProfileId] [int] NOT NULL,
 CONSTRAINT [PK_UserPermissionProfile] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[PermissionProfileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Vehicle]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Vehicle](
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[VIN] [varchar](17) NULL,
	[HSN] [varchar](4) NULL,
	[TSN] [varchar](4) NULL,
	[Variant] [varchar](50) NULL,
	[CurrentRegistrationId] [int] NULL,
	[FirstRegistrationDate] [datetime] NULL,
	[ColorCode] [int] NULL,
 CONSTRAINT [PK_Vehicle] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WebServiceLogin]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WebServiceLogin](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Login] [varchar](20) NOT NULL,
	[Password] [varchar](20) NOT NULL,
 CONSTRAINT [PK_WebServiceLogin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WebServiceLoginCustomer]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebServiceLoginCustomer](
	[WebServiceLoginId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
 CONSTRAINT [PK_WebServiceLoginCustomer] PRIMARY KEY CLUSTERED 
(
	[WebServiceLoginId] ASC,
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[AuthorativeChargeAccounts]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[AuthorativeChargeAccounts]
AS
SELECT 
od.Id as OrderItemId,
it.InvoiceId as InvoiceId,
inac.IIACCID as InvoiceItemAccountItemId,
inac.InvoiceItemId as InvoiceItemId,
inac.RevenueAccountText as RevenueAccountText,
od.IsAuthorativeCharge as IsAuthorativeCharge

FROM [OrderItem]  as od 
inner join InvoiceItem as it on od.Id = it.OrderItemId
inner join InvoiceItemAccountItem as inac on it.Id = inac.InvoiceItemId
where  od.IsAuthorativeCharge=1


GO
/****** Object:  View [dbo].[ChangeLog]    Script Date: 21.07.2015 10:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ChangeLog]
AS
SELECT        dbo.Person.Id, dbo.Person.Name, dbo.Person.FirstName, dbo.Systemlog.Id AS Expr1, dbo.Systemlog.Date, dbo.Systemlog.TableName, 
                         dbo.Systemlog.TableProperty, dbo.Systemlog.ReferenceId, dbo.Systemlog.Text, dbo.Systemlog.LogTypeId, dbo.Systemlog.Exception, dbo.[User].Login, 
                         dbo.[User].LastLogin, dbo.LogType.Name AS Expr2, dbo.Systemlog.LogUserId
FROM            dbo.Person INNER JOIN
                         dbo.[User] ON dbo.Person.Id = dbo.[User].PersonId INNER JOIN
                         dbo.Systemlog ON dbo.[User].Id = dbo.Systemlog.LogUserId INNER JOIN
                         dbo.LogType ON dbo.Systemlog.LogTypeId = dbo.LogType.Id



GO
ALTER TABLE [dbo].[CarOwner]  WITH CHECK ADD  CONSTRAINT [FK_CarOwner_Adress] FOREIGN KEY([AdressId])
REFERENCES [dbo].[Adress] ([Id])
GO
ALTER TABLE [dbo].[CarOwner] CHECK CONSTRAINT [FK_CarOwner_Adress]
GO
ALTER TABLE [dbo].[CarOwner]  WITH CHECK ADD  CONSTRAINT [FK_CarOwner_BankAccount] FOREIGN KEY([BankAccountId])
REFERENCES [dbo].[BankAccount] ([Id])
GO
ALTER TABLE [dbo].[CarOwner] CHECK CONSTRAINT [FK_CarOwner_BankAccount]
GO
ALTER TABLE [dbo].[CarOwner]  WITH CHECK ADD  CONSTRAINT [FK_CarOwner_Contact] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contact] ([Id])
GO
ALTER TABLE [dbo].[CarOwner] CHECK CONSTRAINT [FK_CarOwner_Contact]
GO
ALTER TABLE [dbo].[CostCenter]  WITH CHECK ADD  CONSTRAINT [FK_CostCenter_BankAccount] FOREIGN KEY([BankAccountId])
REFERENCES [dbo].[BankAccount] ([Id])
GO
ALTER TABLE [dbo].[CostCenter] CHECK CONSTRAINT [FK_CostCenter_BankAccount]
GO
ALTER TABLE [dbo].[CostCenter]  WITH CHECK ADD  CONSTRAINT [FK_CostCenter_LargeCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[LargeCustomer] ([CustomerId])
GO
ALTER TABLE [dbo].[CostCenter] CHECK CONSTRAINT [FK_CostCenter_LargeCustomer]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Adress] FOREIGN KEY([AdressId])
REFERENCES [dbo].[Adress] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Adress]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Contact] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contact] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Contact]
GO
ALTER TABLE [dbo].[DeregistrationOrder]  WITH CHECK ADD  CONSTRAINT [FK_DeregistrationOrder_Order] FOREIGN KEY([OrderNumber])
REFERENCES [dbo].[Order] ([OrderNumber])
GO
ALTER TABLE [dbo].[DeregistrationOrder] CHECK CONSTRAINT [FK_DeregistrationOrder_Order]
GO
ALTER TABLE [dbo].[DeregistrationOrder]  WITH CHECK ADD  CONSTRAINT [FK_DeregistrationOrder_Registration] FOREIGN KEY([RegistrationId])
REFERENCES [dbo].[Registration] ([Id])
GO
ALTER TABLE [dbo].[DeregistrationOrder] CHECK CONSTRAINT [FK_DeregistrationOrder_Registration]
GO
ALTER TABLE [dbo].[DeregistrationOrder]  WITH CHECK ADD  CONSTRAINT [FK_DeregistrationOrder_Vehicle] FOREIGN KEY([VehicleId])
REFERENCES [dbo].[Vehicle] ([Id])
GO
ALTER TABLE [dbo].[DeregistrationOrder] CHECK CONSTRAINT [FK_DeregistrationOrder_Vehicle]
GO
ALTER TABLE [dbo].[DocketList]  WITH CHECK ADD  CONSTRAINT [FK_DocketList_Adress] FOREIGN KEY([RecipientAdressId])
REFERENCES [dbo].[Adress] ([Id])
GO
ALTER TABLE [dbo].[DocketList] CHECK CONSTRAINT [FK_DocketList_Adress]
GO
ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_DocumentType] FOREIGN KEY([DokumentTypeId])
REFERENCES [dbo].[DocumentType] ([Id])
GO
ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_DocumentType]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Adress] FOREIGN KEY([InvoiceRecipientAdressId])
REFERENCES [dbo].[Adress] ([Id])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_Adress]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_Customer]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Document] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[Document] ([Id])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_Document]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_InvoiceTypes] FOREIGN KEY([InvoiceType])
REFERENCES [dbo].[InvoiceTypes] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_InvoiceTypes]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_User]
GO
ALTER TABLE [dbo].[InvoiceIDInvoiceRunID]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceIDInvoiceRunID_Invoice] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoice] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceIDInvoiceRunID] CHECK CONSTRAINT [FK_InvoiceIDInvoiceRunID_Invoice]
GO
ALTER TABLE [dbo].[InvoiceIDInvoiceRunID]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceIDInvoiceRunID_InvoiceRunReport] FOREIGN KEY([InvoiceRunId])
REFERENCES [dbo].[InvoiceRunReport] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceIDInvoiceRunID] CHECK CONSTRAINT [FK_InvoiceIDInvoiceRunID_InvoiceRunReport]
GO
ALTER TABLE [dbo].[InvoiceItem]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceItem_CostCenter] FOREIGN KEY([CostcenterId])
REFERENCES [dbo].[CostCenter] ([Id])
GO
ALTER TABLE [dbo].[InvoiceItem] CHECK CONSTRAINT [FK_InvoiceItem_CostCenter]
GO
ALTER TABLE [dbo].[InvoiceItem]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceItem_Invoice] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoice] ([Id])
GO
ALTER TABLE [dbo].[InvoiceItem] CHECK CONSTRAINT [FK_InvoiceItem_Invoice]
GO
ALTER TABLE [dbo].[InvoiceItem]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceItem_OrderItem] FOREIGN KEY([OrderItemId])
REFERENCES [dbo].[OrderItem] ([Id])
GO
ALTER TABLE [dbo].[InvoiceItem] CHECK CONSTRAINT [FK_InvoiceItem_OrderItem]
GO
ALTER TABLE [dbo].[InvoiceItemAccountItem]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceItemAccountItem_InvoiceItem] FOREIGN KEY([InvoiceItemId])
REFERENCES [dbo].[InvoiceItem] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceItemAccountItem] CHECK CONSTRAINT [FK_InvoiceItemAccountItem_InvoiceItem]
GO
ALTER TABLE [dbo].[InvoiceNumber]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceNumber_Invoice] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoice] ([Id])
GO
ALTER TABLE [dbo].[InvoiceNumber] CHECK CONSTRAINT [FK_InvoiceNumber_Invoice]
GO
ALTER TABLE [dbo].[LargeCustomer]  WITH CHECK ADD  CONSTRAINT [FK_LargeCustomer_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[LargeCustomer] CHECK CONSTRAINT [FK_LargeCustomer_Customer]
GO
ALTER TABLE [dbo].[LargeCustomer]  WITH CHECK ADD  CONSTRAINT [FK_LargeCustomer_InvoiceTypes] FOREIGN KEY([InvoiceTypesID])
REFERENCES [dbo].[InvoiceTypes] ([ID])
GO
ALTER TABLE [dbo].[LargeCustomer] CHECK CONSTRAINT [FK_LargeCustomer_InvoiceTypes]
GO
ALTER TABLE [dbo].[LargeCustomer]  WITH CHECK ADD  CONSTRAINT [FK_LargeCustomer_Location] FOREIGN KEY([MainLocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[LargeCustomer] CHECK CONSTRAINT [FK_LargeCustomer_Location]
GO
ALTER TABLE [dbo].[LargeCustomer]  WITH CHECK ADD  CONSTRAINT [FK_LargeCustomer_Person] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Person] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LargeCustomer] CHECK CONSTRAINT [FK_LargeCustomer_Person]
GO
ALTER TABLE [dbo].[LargeCustomerRequiredField]  WITH CHECK ADD  CONSTRAINT [FK_LargeCustomerRequiredField_LargeCustomer] FOREIGN KEY([LargeCustomerId])
REFERENCES [dbo].[LargeCustomer] ([CustomerId])
GO
ALTER TABLE [dbo].[LargeCustomerRequiredField] CHECK CONSTRAINT [FK_LargeCustomerRequiredField_LargeCustomer]
GO
ALTER TABLE [dbo].[LargeCustomerRequiredField]  WITH CHECK ADD  CONSTRAINT [FK_LargeCustomerRequiredField_RequiredField] FOREIGN KEY([RequiredFieldId])
REFERENCES [dbo].[RequiredField] ([Id])
GO
ALTER TABLE [dbo].[LargeCustomerRequiredField] CHECK CONSTRAINT [FK_LargeCustomerRequiredField_RequiredField]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Adress] FOREIGN KEY([AdressId])
REFERENCES [dbo].[Adress] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_Adress]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_InvoiceAdress] FOREIGN KEY([InvoiceAdressId])
REFERENCES [dbo].[Adress] ([Id])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_InvoiceAdress]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_InvoiceDispatchAdress] FOREIGN KEY([InvoiceDispatchAdressId])
REFERENCES [dbo].[Adress] ([Id])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_InvoiceDispatchAdress]
GO
--ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Adress3] FOREIGN KEY([InvoiceAdressId])
--REFERENCES [dbo].[Adress] ([Id])
--GO
--ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_Adress3]
--GO
--ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Adress4] FOREIGN KEY([InvoiceDispatchAdressId])
--REFERENCES [dbo].[Adress] ([Id])
--GO
--ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_Adress4]
--GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Contact] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contact] ([Id])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_Contact]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_LargeCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[LargeCustomer] ([CustomerId])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_LargeCustomer]
GO
--ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Location] FOREIGN KEY([SuperLocationId])
--REFERENCES [dbo].[Location] ([Id])
--GO
--ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_Location]
--GO
ALTER TABLE [dbo].[Mailinglist]  WITH CHECK ADD  CONSTRAINT [FK_CustomerMailinglist_LargeCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[LargeCustomer] ([CustomerId])
GO
ALTER TABLE [dbo].[Mailinglist] CHECK CONSTRAINT [FK_CustomerMailinglist_LargeCustomer]
GO
ALTER TABLE [dbo].[Mailinglist]  WITH CHECK ADD  CONSTRAINT [FK_Mailinglist_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Mailinglist] CHECK CONSTRAINT [FK_Mailinglist_Location]
GO
ALTER TABLE [dbo].[Mailinglist]  WITH CHECK ADD  CONSTRAINT [FK_Mailinglist_MailinglistType] FOREIGN KEY([MailinglistTypeId])
REFERENCES [dbo].[MailinglistType] ([Id])
GO
ALTER TABLE [dbo].[Mailinglist] CHECK CONSTRAINT [FK_Mailinglist_MailinglistType]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_DocketList] FOREIGN KEY([DocketListNumber])
REFERENCES [dbo].[DocketList] ([DocketListNumber])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_DocketList]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Location]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_OrderStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[OrderStatus] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_OrderStatus]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_OrderType] FOREIGN KEY([OrderTypeId])
REFERENCES [dbo].[OrderType] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_OrderType]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_PackingList] FOREIGN KEY([PackingListNumber])
REFERENCES [dbo].[PackingList] ([PackingListNumber])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_PackingList]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_User]
GO
ALTER TABLE [dbo].[OrderInvoice]  WITH CHECK ADD  CONSTRAINT [FK_OrderInvoice_Invoice] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoice] ([Id])
GO
ALTER TABLE [dbo].[OrderInvoice] CHECK CONSTRAINT [FK_OrderInvoice_Invoice]
GO
ALTER TABLE [dbo].[OrderInvoice]  WITH CHECK ADD  CONSTRAINT [FK_OrderInvoice_Order] FOREIGN KEY([OrderNumber])
REFERENCES [dbo].[Order] ([OrderNumber])
GO
ALTER TABLE [dbo].[OrderInvoice] CHECK CONSTRAINT [FK_OrderInvoice_Order]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_CostCenter] FOREIGN KEY([CostCenterId])
REFERENCES [dbo].[CostCenter] ([Id])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_CostCenter]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY([OrderNumber])
REFERENCES [dbo].[Order] ([OrderNumber])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Order]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_OrderItem] FOREIGN KEY([SuperOrderItemId])
REFERENCES [dbo].[OrderItem] ([Id])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_OrderItem]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_OrderItemStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[OrderItemStatus] ([Id])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_OrderItemStatus]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Product]
GO
ALTER TABLE [dbo].[PackingList]  WITH CHECK ADD  CONSTRAINT [FK_PackingList_Adress] FOREIGN KEY([RecipientAdressId])
REFERENCES [dbo].[Adress] ([Id])
GO
ALTER TABLE [dbo].[PackingList] CHECK CONSTRAINT [FK_PackingList_Adress]
GO
--ALTER TABLE [dbo].[PackingList]  WITH CHECK ADD  CONSTRAINT [FK_PackingList_Order] FOREIGN KEY([OldOrderId])
--REFERENCES [dbo].[Order] ([Id])
--GO
--ALTER TABLE [dbo].[PackingList] CHECK CONSTRAINT [FK_PackingList_Order]
--GO
ALTER TABLE [dbo].[PermissionProfilePermission]  WITH CHECK ADD  CONSTRAINT [FK_PermissionProfilePermission_Permission] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permission] ([Id])
GO
ALTER TABLE [dbo].[PermissionProfilePermission] CHECK CONSTRAINT [FK_PermissionProfilePermission_Permission]
GO
ALTER TABLE [dbo].[PermissionProfilePermission]  WITH CHECK ADD  CONSTRAINT [FK_PermissionProfilePermission_PermissionProfile] FOREIGN KEY([PermissionProfileId])
REFERENCES [dbo].[PermissionProfile] ([Id])
GO
ALTER TABLE [dbo].[PermissionProfilePermission] CHECK CONSTRAINT [FK_PermissionProfilePermission_PermissionProfile]
GO
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK_Price_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Price] CHECK CONSTRAINT [FK_Price_Location]
GO
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK_Price_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[Price] CHECK CONSTRAINT [FK_Price_Product]
GO
ALTER TABLE [dbo].[PriceAccount]  WITH CHECK ADD  CONSTRAINT [FK_PriceAccount_Accounts] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([AccountId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PriceAccount] CHECK CONSTRAINT [FK_PriceAccount_Accounts]
GO
ALTER TABLE [dbo].[PriceAccount]  WITH CHECK ADD  CONSTRAINT [FK_PriceAccount_Price] FOREIGN KEY([PriceId])
REFERENCES [dbo].[Price] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PriceAccount] CHECK CONSTRAINT [FK_PriceAccount_Price]
GO
ALTER TABLE [dbo].[Registration]  WITH CHECK ADD  CONSTRAINT [FK_Registration_CarOwner] FOREIGN KEY([CarOwnerId])
REFERENCES [dbo].[CarOwner] ([Id])
GO
ALTER TABLE [dbo].[Registration] CHECK CONSTRAINT [FK_Registration_CarOwner]
GO
ALTER TABLE [dbo].[Registration]  WITH CHECK ADD  CONSTRAINT [FK_Registration_Vehicle] FOREIGN KEY([VehicleId])
REFERENCES [dbo].[Vehicle] ([Id])
GO
ALTER TABLE [dbo].[Registration] CHECK CONSTRAINT [FK_Registration_Vehicle]
GO
ALTER TABLE [dbo].[RegistrationLocation]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationLocation_Adress] FOREIGN KEY([RegistrationLocationAdressId])
REFERENCES [dbo].[Adress] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RegistrationLocation] CHECK CONSTRAINT [FK_RegistrationLocation_Adress]
GO
ALTER TABLE [dbo].[RegistrationOrder]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationOrder_Order] FOREIGN KEY([OrderNumber])
REFERENCES [dbo].[Order] ([OrderNumber])
GO
ALTER TABLE [dbo].[RegistrationOrder] CHECK CONSTRAINT [FK_RegistrationOrder_Order]
GO
ALTER TABLE [dbo].[RegistrationOrder]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationOrder_Registration] FOREIGN KEY([RegistrationId])
REFERENCES [dbo].[Registration] ([Id])
GO
ALTER TABLE [dbo].[RegistrationOrder] CHECK CONSTRAINT [FK_RegistrationOrder_Registration]
GO
ALTER TABLE [dbo].[RegistrationOrder]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationOrder_RegistrationType] FOREIGN KEY([RegistrationOrderTypeId])
REFERENCES [dbo].[RegistrationOrderType] ([Id])
GO
ALTER TABLE [dbo].[RegistrationOrder] CHECK CONSTRAINT [FK_RegistrationOrder_RegistrationType]
GO
ALTER TABLE [dbo].[RegistrationOrder]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationOrder_Vehicle] FOREIGN KEY([VehicleId])
REFERENCES [dbo].[Vehicle] ([Id])
GO
ALTER TABLE [dbo].[RegistrationOrder] CHECK CONSTRAINT [FK_RegistrationOrder_Vehicle]
GO
--ALTER TABLE [dbo].[RequiredField]  WITH CHECK ADD  CONSTRAINT [FK_RequiredField_RequiredFieldTranslation] FOREIGN KEY([Id])
--REFERENCES [dbo].[RequiredFieldTranslation] ([RequiredFieldId])
--ON UPDATE CASCADE
--ON DELETE CASCADE
--GO
--ALTER TABLE [dbo].[RequiredField] CHECK CONSTRAINT [FK_RequiredField_RequiredFieldTranslation]
--GO
ALTER TABLE [dbo].[RequiredField]  WITH CHECK ADD  CONSTRAINT [FK_RequiredFields_OrderType] FOREIGN KEY([OrderTypeId])
REFERENCES [dbo].[OrderType] ([Id])
GO
ALTER TABLE [dbo].[RequiredField] CHECK CONSTRAINT [FK_RequiredFields_OrderType]
GO
ALTER TABLE [dbo].[SmallCustomer]  WITH CHECK ADD  CONSTRAINT [FK_SmallCustomer_BankAccount] FOREIGN KEY([BankAccountId])
REFERENCES [dbo].[BankAccount] ([Id])
GO
ALTER TABLE [dbo].[SmallCustomer] CHECK CONSTRAINT [FK_SmallCustomer_BankAccount]
GO
ALTER TABLE [dbo].[SmallCustomer]  WITH CHECK ADD  CONSTRAINT [FK_SmallCustomer_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[SmallCustomer] CHECK CONSTRAINT [FK_SmallCustomer_Customer]
GO
ALTER TABLE [dbo].[SmallCustomer]  WITH CHECK ADD  CONSTRAINT [FK_SmallCustomer_Person] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[SmallCustomer] CHECK CONSTRAINT [FK_SmallCustomer_Person]
GO
ALTER TABLE [dbo].[Systemlog]  WITH CHECK ADD  CONSTRAINT [FK_Systemlog_LogType] FOREIGN KEY([LogTypeId])
REFERENCES [dbo].[LogType] ([Id])
GO
ALTER TABLE [dbo].[Systemlog] CHECK CONSTRAINT [FK_Systemlog_LogType]
GO
ALTER TABLE [dbo].[Systemlog]  WITH CHECK ADD  CONSTRAINT [FK_Systemlog_User] FOREIGN KEY([LogUserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Systemlog] CHECK CONSTRAINT [FK_Systemlog_User]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Contact] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contact] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Contact]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Person] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Person]
GO
ALTER TABLE [dbo].[UserPermission]  WITH CHECK ADD  CONSTRAINT [FK_User_Right_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserPermission] CHECK CONSTRAINT [FK_User_Right_User]
GO
ALTER TABLE [dbo].[UserPermission]  WITH CHECK ADD  CONSTRAINT [FK_UserPermission_Permission] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permission] ([Id])
GO
ALTER TABLE [dbo].[UserPermission] CHECK CONSTRAINT [FK_UserPermission_Permission]
GO
ALTER TABLE [dbo].[UserPermissionProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserPermissionProfile_PermissionProfile] FOREIGN KEY([PermissionProfileId])
REFERENCES [dbo].[PermissionProfile] ([Id])
GO
ALTER TABLE [dbo].[UserPermissionProfile] CHECK CONSTRAINT [FK_UserPermissionProfile_PermissionProfile]
GO
ALTER TABLE [dbo].[UserPermissionProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserPermissionProfile_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserPermissionProfile] CHECK CONSTRAINT [FK_UserPermissionProfile_User]
GO
--ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_Registration] FOREIGN KEY([CurrentRegistrationId])
--REFERENCES [dbo].[Registration] ([Id])
--GO
--ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_Registration]
--GO
ALTER TABLE [dbo].[WebServiceLoginCustomer]  WITH CHECK ADD  CONSTRAINT [FK_WebServiceLoginCustomer_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[WebServiceLoginCustomer] CHECK CONSTRAINT [FK_WebServiceLoginCustomer_Customer]
GO
ALTER TABLE [dbo].[WebServiceLoginCustomer]  WITH CHECK ADD  CONSTRAINT [FK_WebServiceLoginCustomer_WebServiceLogin] FOREIGN KEY([WebServiceLoginId])
REFERENCES [dbo].[WebServiceLogin] ([Id])
GO
ALTER TABLE [dbo].[WebServiceLoginCustomer] CHECK CONSTRAINT [FK_WebServiceLoginCustomer_WebServiceLogin]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Person"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 307
               Right = 205
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Systemlog"
            Begin Extent = 
               Top = 6
               Left = 243
               Bottom = 282
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "User"
            Begin Extent = 
               Top = 6
               Left = 460
               Bottom = 307
               Right = 627
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "LogType"
            Begin Extent = 
               Top = 119
               Left = 652
               Bottom = 214
               Right = 819
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ChangeLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ChangeLog'
GO
USE [master]
GO
ALTER DATABASE [Test] SET  READ_WRITE 
GO
