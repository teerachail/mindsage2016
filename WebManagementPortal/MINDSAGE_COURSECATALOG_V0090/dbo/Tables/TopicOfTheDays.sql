-- Creating table 'TopicOfTheDays'
CREATE TABLE [dbo].[TopicOfTheDays] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Message] varchar(max)  NOT NULL,
    [SendOnDay] int  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL,
    [LessonId] int  NOT NULL
);
GO
-- Creating foreign key on [LessonId] in table 'TopicOfTheDays'
ALTER TABLE [dbo].[TopicOfTheDays]
ADD CONSTRAINT [FK_LessonTopicOfTheDay]
    FOREIGN KEY ([LessonId])
    REFERENCES [dbo].[Lessons]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'TopicOfTheDays'
ALTER TABLE [dbo].[TopicOfTheDays]
ADD CONSTRAINT [PK_TopicOfTheDays]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_LessonTopicOfTheDay'
CREATE INDEX [IX_FK_LessonTopicOfTheDay]
ON [dbo].[TopicOfTheDays]
    ([LessonId]);