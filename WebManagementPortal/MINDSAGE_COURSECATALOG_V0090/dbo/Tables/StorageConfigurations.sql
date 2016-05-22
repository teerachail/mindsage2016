-- Creating table 'StorageConfigurations'
CREATE TABLE [dbo].[StorageConfigurations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BaseUrl] varchar(max)  NOT NULL,
    [AccountName] varchar(max)  NOT NULL,
    [APIKey] varchar(max)  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL
);
GO
-- Creating primary key on [Id] in table 'StorageConfigurations'
ALTER TABLE [dbo].[StorageConfigurations]
ADD CONSTRAINT [PK_StorageConfigurations]
    PRIMARY KEY CLUSTERED ([Id] ASC);