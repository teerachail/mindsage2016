
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/25/2016 18:02:23
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

IF OBJECT_ID(N'[dbo].[FK_LessonAdvertisment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advertisments] DROP CONSTRAINT [FK_LessonAdvertisment];
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

IF OBJECT_ID(N'[dbo].[Advertisments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Advertisments];
GO


-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

GO
