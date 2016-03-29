-- Creating table 'Contracts'
CREATE TABLE [dbo].[Contracts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SchoolName] varchar(255)  NOT NULL,
    [City] varchar(50)  NOT NULL,
    [State] varchar(50)  NOT NULL,
    [ZipCode] varchar(50)  NOT NULL,
    [PrimaryContractName] varchar(255)  NOT NULL,
    [PrimaryPhoneNumber] varchar(50)  NOT NULL,
    [PrimaryEmail] varchar(50)  NOT NULL,
    [SecondaryContractName] varchar(255)  NULL,
    [SecondaryPhoneNumber] varchar(50)  NULL,
    [SecondaryEmail] varchar(50)  NULL,
    [StartDate] datetime  NOT NULL,
    [ExpiredDate] datetime  NOT NULL,
    [TimeZone] varchar(255)  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL
);
GO
-- Creating primary key on [Id] in table 'Contracts'
ALTER TABLE [dbo].[Contracts]
ADD CONSTRAINT [PK_Contracts]
    PRIMARY KEY CLUSTERED ([Id] ASC);