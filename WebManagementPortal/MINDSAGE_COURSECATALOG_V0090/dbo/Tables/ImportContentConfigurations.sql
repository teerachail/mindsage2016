-- Creating table 'ImportContentConfigurations'
CREATE TABLE [dbo].[ImportContentConfigurations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BaseURL] varchar(max)  NOT NULL,
    [HomePageURL] varchar(max)  NOT NULL,
    [PagesURLs] varchar(max)  NOT NULL,
    [ReferenceFileURLs] varchar(max)  NOT NULL,
    [ReplaceSections] varchar(max)  NOT NULL,
    [StorageInfo] varchar(max)  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL
);
GO
-- Creating primary key on [Id] in table 'ImportContentConfigurations'
ALTER TABLE [dbo].[ImportContentConfigurations]
ADD CONSTRAINT [PK_ImportContentConfigurations]
    PRIMARY KEY CLUSTERED ([Id] ASC);