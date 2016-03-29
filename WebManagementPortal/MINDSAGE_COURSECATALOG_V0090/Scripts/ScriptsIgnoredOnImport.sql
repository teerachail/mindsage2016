
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/29/2016 15:11:12
-- Generated from EDMX file: E:\mindsage2016\WebManagementPortal\WebManagementPortal\EF\MindSageDataModels.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO

USE [MINDSAGE_COURSECATALOG_V0090];
GO

IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CourseCatalogSemester]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Semesters] DROP CONSTRAINT [FK_CourseCatalogSemester];
GO

IF OBJECT_ID(N'[dbo].[FK_SemesterUnit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Units] DROP CONSTRAINT [FK_SemesterUnit];
GO

IF OBJECT_ID(N'[dbo].[FK_UnitLesson]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Lessons] DROP CONSTRAINT [FK_UnitLesson];
GO

IF OBJECT_ID(N'[dbo].[FK_LessonAdvertisement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advertisements] DROP CONSTRAINT [FK_LessonAdvertisement];
GO

IF OBJECT_ID(N'[dbo].[FK_LessonTopicOfTheDay]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TopicOfTheDays] DROP CONSTRAINT [FK_LessonTopicOfTheDay];
GO

IF OBJECT_ID(N'[dbo].[FK_ContractLicense]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Licenses] DROP CONSTRAINT [FK_ContractLicense];
GO

IF OBJECT_ID(N'[dbo].[FK_CourseCatalogLicense]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Licenses] DROP CONSTRAINT [FK_CourseCatalogLicense];
GO

IF OBJECT_ID(N'[dbo].[FK_LicenseTeacherKey]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeacherKeys] DROP CONSTRAINT [FK_LicenseTeacherKey];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CourseCatalogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CourseCatalogs];
GO

IF OBJECT_ID(N'[dbo].[Semesters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Semesters];
GO

IF OBJECT_ID(N'[dbo].[Units]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Units];
GO

IF OBJECT_ID(N'[dbo].[Lessons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Lessons];
GO

IF OBJECT_ID(N'[dbo].[Advertisements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Advertisements];
GO

IF OBJECT_ID(N'[dbo].[TopicOfTheDays]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TopicOfTheDays];
GO

IF OBJECT_ID(N'[dbo].[Contracts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contracts];
GO

IF OBJECT_ID(N'[dbo].[Licenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Licenses];
GO

IF OBJECT_ID(N'[dbo].[TeacherKeys]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TeacherKeys];
GO


-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

GO
