-- Creating table 'ExtraContents'
CREATE TABLE [dbo].[ExtraContents] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentURL] varchar(max)  NOT NULL,
    [Description] varchar(max)  NOT NULL,
    [IconURL] varchar(max)  NOT NULL,
    [LessonId] int  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL
);
GO
-- Creating foreign key on [LessonId] in table 'ExtraContents'
ALTER TABLE [dbo].[ExtraContents]
ADD CONSTRAINT [FK_LessonExtraContent]
    FOREIGN KEY ([LessonId])
    REFERENCES [dbo].[Lessons]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'ExtraContents'
ALTER TABLE [dbo].[ExtraContents]
ADD CONSTRAINT [PK_ExtraContents]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_LessonExtraContent'
CREATE INDEX [IX_FK_LessonExtraContent]
ON [dbo].[ExtraContents]
    ([LessonId]);