-- Creating table 'Advertisements'
CREATE TABLE [dbo].[Advertisements] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ImageUrl] varchar(max)  NOT NULL,
    [LinkUrl] varchar(max)  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL,
    [LessonId] int  NOT NULL
);
GO
-- Creating foreign key on [LessonId] in table 'Advertisements'
ALTER TABLE [dbo].[Advertisements]
ADD CONSTRAINT [FK_LessonAdvertisement]
    FOREIGN KEY ([LessonId])
    REFERENCES [dbo].[Lessons]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'Advertisements'
ALTER TABLE [dbo].[Advertisements]
ADD CONSTRAINT [PK_Advertisements]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_LessonAdvertisement'
CREATE INDEX [IX_FK_LessonAdvertisement]
ON [dbo].[Advertisements]
    ([LessonId]);