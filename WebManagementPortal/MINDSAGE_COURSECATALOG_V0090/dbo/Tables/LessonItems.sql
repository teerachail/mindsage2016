-- Creating table 'LessonItems'
CREATE TABLE [dbo].[LessonItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Order] int  NOT NULL,
    [Name] varchar(255)  NOT NULL,
    [IconURL] varchar(255)  NOT NULL,
    [Description] varchar(max)  NOT NULL,
    [IsPreviewable] bit  NOT NULL,
    [ContentType] varchar(100)  NOT NULL,
    [ContentURL] varchar(max)  NULL,
    [TeacherLessonId] int  NULL,
    [StudentLessonId] int  NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL
);
GO
-- Creating foreign key on [TeacherLessonId] in table 'LessonItems'
ALTER TABLE [dbo].[LessonItems]
ADD CONSTRAINT [FK_LessonLessonItem]
    FOREIGN KEY ([TeacherLessonId])
    REFERENCES [dbo].[Lessons]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating foreign key on [StudentLessonId] in table 'LessonItems'
ALTER TABLE [dbo].[LessonItems]
ADD CONSTRAINT [FK_LessonLessonItem1]
    FOREIGN KEY ([StudentLessonId])
    REFERENCES [dbo].[Lessons]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'LessonItems'
ALTER TABLE [dbo].[LessonItems]
ADD CONSTRAINT [PK_LessonItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_LessonLessonItem'
CREATE INDEX [IX_FK_LessonLessonItem]
ON [dbo].[LessonItems]
    ([TeacherLessonId]);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_LessonLessonItem1'
CREATE INDEX [IX_FK_LessonLessonItem1]
ON [dbo].[LessonItems]
    ([StudentLessonId]);