Use PaymentDb

--create schema payment

GO

CREATE TABLE [payment].[Pay] (

    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [OrderId] uniqueidentifier default newid(),
    [StoreId] uniqueidentifier default newid(),
	[PayType] VARCHAR (50) NOT NULL,
	[CardNumber] VARCHAR (50) NOT NULL,
	[CardOwnerName] VARCHAR (150) NOT NULL,
	[SecurityCode] CHAR (3) NOT NULL,
	[ExpirationDate] DATETIME NOT NULL,
	[RequesterId] uniqueidentifier default newid()
)

CREATE SEQUENCE [payment].[PaymentSequenceHiLo] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 10
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
