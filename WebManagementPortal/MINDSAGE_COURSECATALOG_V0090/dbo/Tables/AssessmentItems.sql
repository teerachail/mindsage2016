-- Creating table 'AssessmentItems'
CREATE TABLE [dbo].[AssessmentItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Order] int  NOT NULL,
    [Name] varchar(255)  NOT NULL,
    [IconURL] varchar(255)  NOT NULL,
    [IsPreviewable] bit  NOT NULL,
    [PreLessonId] int  NULL,
    [PostLessonId] int  NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL
);
GO
-- Creating foreign key on [PreLessonId] in table 'AssessmentItems'
ALTER TABLE [dbo].[AssessmentItems]
ADD CONSTRAINT [FK_LessonAssessmentItem]
    FOREIGN KEY ([PreLessonId])
    REFERENCES [dbo].[Lessons]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating foreign key on [PostLessonId] in table 'AssessmentItems'
ALTER TABLE [dbo].[AssessmentItems]
ADD CONSTRAINT [FK_LessonAssessmentItem1]
    FOREIGN KEY ([PostLessonId])
    REFERENCES [dbo].[Lessons]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'AssessmentItems'
ALTER TABLE [dbo].[AssessmentItems]
ADD CONSTRAINT [PK_AssessmentItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_LessonAssessmentItem'
CREATE INDEX [IX_FK_LessonAssessmentItem]
ON [dbo].[AssessmentItems]
    ([PreLessonId]);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_LessonAssessmentItem1'
CREATE INDEX [IX_FK_LessonAssessmentItem1]
ON [dbo].[AssessmentItems]
    ([PostLessonId]);