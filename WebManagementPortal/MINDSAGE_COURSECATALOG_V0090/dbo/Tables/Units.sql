-- Creating table 'Units'
CREATE TABLE [dbo].[Units] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] varchar(255)  NOT NULL,
    [Description] varchar(max)  NOT NULL,
    [TotalWeeks] int  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL,
    [SemesterId] int  NOT NULL
);
GO
-- Creating foreign key on [SemesterId] in table 'Units'
ALTER TABLE [dbo].[Units]
ADD CONSTRAINT [FK_SemesterUnit]
    FOREIGN KEY ([SemesterId])
    REFERENCES [dbo].[Semesters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'Units'
ALTER TABLE [dbo].[Units]
ADD CONSTRAINT [PK_Units]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_SemesterUnit'
CREATE INDEX [IX_FK_SemesterUnit]
ON [dbo].[Units]
    ([SemesterId]);