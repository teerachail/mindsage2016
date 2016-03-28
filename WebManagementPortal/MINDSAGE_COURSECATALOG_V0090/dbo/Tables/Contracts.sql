-- Creating table 'Contracts'
CREATE TABLE [dbo].[Contracts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(255)  NOT NULL,
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