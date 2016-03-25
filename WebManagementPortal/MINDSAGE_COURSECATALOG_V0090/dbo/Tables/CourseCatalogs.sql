-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CourseCatalogs'
CREATE TABLE [dbo].[CourseCatalogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GroupName] varchar(255)  NOT NULL,
    [Grade] varchar(50)  NOT NULL,
    [Advertisements] varchar(max)  NOT NULL,
    [SideName] varchar(255)  NOT NULL,
    [PriceUSD] float  NOT NULL,
    [Series] varchar(255)  NOT NULL,
    [Title] varchar(255)  NOT NULL,
    [FullDescription] varchar(max)  NOT NULL,
    [DescriptionImageUrl] varchar(max)  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL
);
GO
-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'CourseCatalogs'
ALTER TABLE [dbo].[CourseCatalogs]
ADD CONSTRAINT [PK_CourseCatalogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);