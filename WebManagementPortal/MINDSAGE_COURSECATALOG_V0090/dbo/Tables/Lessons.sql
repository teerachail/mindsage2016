-- Creating table 'Lessons'
CREATE TABLE [dbo].[Lessons] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] varchar(255)  NOT NULL,
    [ShortDescription] varchar(max)  NOT NULL,
    [MoreDescription] varchar(max)  NOT NULL,
    [ShortTeacherLessonPlan] varchar(max)  NOT NULL,
    [MoreTeacherLessonPlan] varchar(max)  NOT NULL,
    [PrimaryContentURL] varchar(max)  NOT NULL,
    [ExtraContentUrls] varchar(max)  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL,
    [UnitId] int  NOT NULL
);
GO
-- Creating foreign key on [UnitId] in table 'Lessons'
ALTER TABLE [dbo].[Lessons]
ADD CONSTRAINT [FK_UnitLesson]
    FOREIGN KEY ([UnitId])
    REFERENCES [dbo].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'Lessons'
ALTER TABLE [dbo].[Lessons]
ADD CONSTRAINT [PK_Lessons]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_UnitLesson'
CREATE INDEX [IX_FK_UnitLesson]
ON [dbo].[Lessons]
    ([UnitId]);