-- Creating table 'Semesters'
CREATE TABLE [dbo].[Semesters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] varchar(255)  NOT NULL,
    [Description] varchar(max)  NOT NULL,
    [TotalWeeks] int  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL,
    [CourseCatalogId] int  NOT NULL
);
GO
-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CourseCatalogId] in table 'Semesters'
ALTER TABLE [dbo].[Semesters]
ADD CONSTRAINT [FK_CourseCatalogSemester]
    FOREIGN KEY ([CourseCatalogId])
    REFERENCES [dbo].[CourseCatalogs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'Semesters'
ALTER TABLE [dbo].[Semesters]
ADD CONSTRAINT [PK_Semesters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_CourseCatalogSemester'
CREATE INDEX [IX_FK_CourseCatalogSemester]
ON [dbo].[Semesters]
    ([CourseCatalogId]);