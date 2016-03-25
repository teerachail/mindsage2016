-- Creating table 'Advertisments'
CREATE TABLE [dbo].[Advertisments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ImageUrl] varchar(max)  NOT NULL,
    [LinkUrl] varchar(max)  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL,
    [LessonId] int  NOT NULL
);
GO
-- Creating foreign key on [LessonId] in table 'Advertisments'
ALTER TABLE [dbo].[Advertisments]
ADD CONSTRAINT [FK_LessonAdvertisment]
    FOREIGN KEY ([LessonId])
    REFERENCES [dbo].[Lessons]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'Advertisments'
ALTER TABLE [dbo].[Advertisments]
ADD CONSTRAINT [PK_Advertisments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_LessonAdvertisment'
CREATE INDEX [IX_FK_LessonAdvertisment]
ON [dbo].[Advertisments]
    ([LessonId]);