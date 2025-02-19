USE [AttendanceSystemDatabase]
GO
/****** Object:  Table [dbo].[CodeTable]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CodeTable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](50) NULL,
	[Code] [nvarchar](20) NULL,
	[Description] [nvarchar](50) NULL,
	[DisplayOrder] [int] NULL,
	[Value] [nvarchar](50) NULL,
	[CreatedTS] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Branch]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branch](
	[BranchID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[PhoneNumber] [nvarchar](100) NOT NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Website] [nvarchar](200) NULL,
	[Map] [nvarchar](200) NULL,
	[Type] [nvarchar](20) NULL,
	[AboutUs] [nvarchar](100) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BranchProfile]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BranchProfile](
	[BranchProfileID] [int] IDENTITY(1,1) NOT NULL,
	[BranchCode] [nvarchar](100) NOT NULL,
	[BranchName] [varchar](100) NOT NULL,
	[BranchAddress] [nvarchar](200) NOT NULL,
	[ContactPerson] [varchar](20) NULL,
	[BranchName] [varchar](100) NULL,
	[ContactNumber] [varchar](20) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[WebSite] [varchar](100) NULL,
	[PanNumber] [varchar](50) NOT NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[DepartmentID] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentCode] [nvarchar](100) NULL,
	[DepartmentName] [varchar](100) NOT NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Designation]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Designation](
	[DesignationID] [int] IDENTITY(1,1) NOT NULL,
	[DesignationName] [varchar](100) NOT NULL,
	[DesignationLevel] [varchar](100) NOT NULL,
	[Salary] [money] NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeID] [int] IDENTITY(1,1) NOT NULL,
	[EnrollID] [nvarchar](max) NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NULL,
	[DesignationID] [int] NOT NULL,
	[Gender] [varchar](5) NOT NULL,
	[DateOfBirth] [datetime] NULL,
	[DepartmentID] [int] NOT NULL,
	[SectionID] [int] NULL,
	[Nationality] [nvarchar](50) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[JoiningDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[TemporaryAddress] [nvarchar](max) NULL,
	[PermanentAddress] [nvarchar](max) NULL,
	[ProfileImageURL] [nvarchar](max) NULL,
	[ActiveAccount] [bit] NOT NULL,
	[CountOT] [bit] NOT NULL,
	[RestOnHolidays] [bit] NOT NULL,
	[CheckClockIn] [varchar](4) NULL,
	[CheckClockOut] [varchar](4) NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FiscalYears]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FiscalYears](
	[FiscalID] [int] IDENTITY(1,1) NOT NULL,
	[FiscalYear] [varchar](50) NOT NULL,
	[StartYear] [datetime] NOT NULL,
	[EndYear] [datetime] NOT NULL,
	[IsCurrentFiscalYear] [bit] NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[FiscalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Kaj]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kaj](
	[KajID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[KajID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LeaveApplication]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeaveApplication](
	[LeaveApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[Code] [nvarchar](20) NULL,
	[DesignationID] [int] NULL,
	[DepartmentID] [int] NULL,
	[SectionID] [int] NULL,
	[LeaveType] [nvarchar](20) NULL,
	[LeaveDay] [nvarchar](20) NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[ApprovedBy] [int] NULL,
	[LeaveRemaining] [int] NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[Remarks] [nvarchar](100) NULL,
	[Status] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[LeaveApplicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LeaveOpeningBalance]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeaveOpeningBalance](
	[LeaveOpeningBalanceID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[Type] [varchar](4) NULL,
	[Value] [decimal](18, 0) NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LeaveOpeningBalanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LeaveSetup]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeaveSetup](
	[LeaveID] [int] IDENTITY(1,1) NOT NULL,
	[LeaveCode] [varchar](100) NULL,
	[LeaveName] [varchar](100) NULL,
	[LeaveDays] [int] NULL,
	[LeaveIncrement] [varchar](50) NULL,
	[ApplicableGender] [varchar](20) NULL,
	[Description] [nvarchar](200) NULL,
	[IsReplacementLeave] [bit] NULL,
	[IsPaidLeave] [bit] NULL,
	[IsLeaveCarryable] [bit] NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LeaveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MenuName] [nvarchar](50) NULL,
	[MenuType] [nvarchar](10) NULL,
	[ParentID] [int] NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[Icon] [nvarchar](50) NULL,
	[Link] [nvarchar](max) NULL,
	[ManagerAccess] [bit] NOT NULL,
	[UserAccess] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OfficeVisit]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OfficeVisit](
	[OfficeVisitID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[OfficeVisitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NULL,
	[ManagerAccess] [bit] NOT NULL,
	[UserAccess] [bit] NOT NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Token]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Token](
	[TokenID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ClientID] [nvarchar](50) NULL,
	[RefreshToken] [nvarchar](max) NULL,
	[ExpiryTime] [datetime] NULL,
	[CreatedTS] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TokenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[BranchID] [int] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[PasswordHash] [varbinary](1024) NULL,
	[PasswordSalt] [varbinary](1024) NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[Designation] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAccess]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAccess](
	[UserID] [int] NOT NULL,
	[MenuId] [int] NOT NULL,
	[ReadAccess] [bit] NOT NULL,
	[ReadWriteAccess] [bit] NOT NULL,
	[ExportAccess] [bit] NOT NULL,
	[PrintAccess] [bit] NOT NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[BranchID] [int] NOT NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkShift]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkShift](
	[ShiftID] [int] IDENTITY(1,1) NOT NULL,
	[ShiftCode] [varchar](100) NULL,
	[ShiftName] [varchar](100) NULL,
	[ShiftStart] [time](0) NULL,
	[ShiftEnd] [time](0) NULL,
	[BeginingIn] [time](0) NULL,
	[EndingIn] [time](0) NULL,
	[BeginingOut] [time](0) NULL,
	[EndingOut] [time](0) NULL,
	[LateIn] [time](0) NULL,
	[LeaveEarly] [varchar](100) NULL,
	[IsMustCheckIn] [bit] NULL,
	[IsMustCheckOut] [bit] NULL,
	[IsLateIn] [bit] NULL,
	[IsEarlyLeave] [bit] NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ShiftID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkShiftType]    Script Date: 7/12/2020 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkShiftType](
	[ShiftTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ShiftID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[StartTime] [time](0) NULL,
	[EndTime] [time](0) NULL,
	[Count] [int] NULL,
	[Duration] [time](0) NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ShiftTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CodeTable] ON 

INSERT [dbo].[CodeTable] ([ID], [TableName], [Code], [Description], [DisplayOrder], [Value], [CreatedTS]) VALUES (1, N'GenderTable', N'GENDER', N'Male', 1, N'Male', CAST(N'2020-07-10T23:12:09.163' AS DateTime))
INSERT [dbo].[CodeTable] ([ID], [TableName], [Code], [Description], [DisplayOrder], [Value], [CreatedTS]) VALUES (2, N'GenderTable', N'GENDER', N'Female', 2, N'Female', CAST(N'2020-07-10T23:12:09.167' AS DateTime))
INSERT [dbo].[CodeTable] ([ID], [TableName], [Code], [Description], [DisplayOrder], [Value], [CreatedTS]) VALUES (3, N'GenderTable', N'GENDER', N'Other', 3, N'Other', CAST(N'2020-07-10T23:12:09.167' AS DateTime))
INSERT [dbo].[CodeTable] ([ID], [TableName], [Code], [Description], [DisplayOrder], [Value], [CreatedTS]) VALUES (4, N'CheckClockInOutTable', N'CLOCKINOUT', N'By Time Zone', 1, N'BTZ', CAST(N'2020-07-10T23:12:09.167' AS DateTime))
INSERT [dbo].[CodeTable] ([ID], [TableName], [Code], [Description], [DisplayOrder], [Value], [CreatedTS]) VALUES (5, N'CheckClockInOutTable', N'CLOCKINOUT', N'Must Clock In', 2, N'MCI', CAST(N'2020-07-10T23:12:09.167' AS DateTime))
INSERT [dbo].[CodeTable] ([ID], [TableName], [Code], [Description], [DisplayOrder], [Value], [CreatedTS]) VALUES (6, N'CheckClockInOutTable', N'CLOCKINOUT', N'Don''t Check', 3, N'DC', CAST(N'2020-07-10T23:12:09.167' AS DateTime))
SET IDENTITY_INSERT [dbo].[CodeTable] OFF
SET IDENTITY_INSERT [dbo].[Menu] ON 

INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (1, N'Branch Setup', N'Setting', 0, CAST(N'2020-07-08T00:00:00.000' AS DateTime), 1, NULL, NULL, N'settings', NULL, 0, 0, 1)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (7, N'Fiscal Year', N'Setting', 1, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'calendar-blank', N'/fiscalYear', 0, 0, 1)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (8, N'Branch Profile', N'Setting', 1, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'equal-box', N'/BranchProfile', 0, 0, 2)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (9, N'Department', N'Setting', 1, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'view-agenda', N'/department', 0, 0, 3)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (10, N'Designation', N'Setting', 1, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'sort-descending', N'/designation', 0, 0, 4)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (11, N'Work Shifts', N'Setting', 1, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'timer', N'/workShifts', 0, 0, 5)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (12, N'Device Manager', N'Setting', 1, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'download', N'/deviceManager', 0, 0, 6)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (13, N'Office Management', N'Management', 0, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'store', NULL, 0, 0, 2)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (15, N'Employee', N'Management', 13, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'account-multiple', N'/employee', 1, 0, 1)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (16, N'Dyanmic Roster', N'Management', 13, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'timer', N'/dyanmicRoster', 1, 0, 2)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (17, N'Weekly Roster', N'Management', 13, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'timer', N'/weeklyRoster', 1, 0, 3)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (18, N'Fixed Roster', N'Management', 13, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'timer', N'/fixedRoster', 1, 0, 4)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (19, N'Office Visit', N'Management', 13, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'baby', N'/officeVisit', 1, 0, 5)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (20, N'Office Approval', N'Management', 13, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'checkbox-marked-circle', N'/officeApproval', 1, 0, 6)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (21, N'Kaj', N'Management', 13, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'baby', N'/kaj', 1, 0, 7)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (22, N'Kaj Approval', N'Management', 13, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'checkbox-marked-circle', N'/kajApproval', 1, 0, 8)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (23, N'Manual Punch', N'Management', 13, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'television', N'/manualPunch', 1, 0, 9)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (24, N'Holiday', N'Management', 13, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'airplane', N'/holiday', 1, 0, 10)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (25, N'Leave Management', N'Management', 0, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'timetable', NULL, 1, 1, 3)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (26, N'Leave Application', N'Management', 25, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'newspaper', N'/leaveApplication', 1, 1, 1)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (27, N'Leave Approval', N'Management', 25, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'checkbox-marked-circle', N'/leaveApproval', 1, 1, 2)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (28, N'Leave Quota', N'Management', 25, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'newspaper', N'/leaveQuota', 1, 1, 3)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (34, N'Leave Opening Balance', N'Management', 25, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'calendar-text', N'/leaveOpeningBalance', 1, 1, 4)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (35, N'Leave Settlement', N'Management', 25, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'calendar-multiple-check', N'/leaveSettlement', 1, 1, 5)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (36, N'User Management', N'Management', 0, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'account-circle', NULL, 1, 1, 4)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (37, N'Payroll', N'Payroll', 0, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'cash-100', NULL, 1, 0, 5)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (39, N'Allowances', N'Payroll', 37, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'folder-plus', N'/allowances', 1, 0, 1)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (40, N'Deducations', N'Payroll', 37, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'folder-remove', N'/deducations', 1, 0, 2)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (41, N'Quota', N'Payroll', 37, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'newspaper', N'/quota', 1, 0, 3)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (42, N'Payslip', N'Payroll', 37, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'cash-100', N'/payslip', 1, 0, 4)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (43, N'Report', N'Report', 0, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'poll', NULL, 1, 0, 6)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (44, N'Atttendance Report', N'Report', 43, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'poll-box', N'/atttendanceReport', 1, 0, 1)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (45, N'Leave Report', N'Report', 43, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'poll-box', N'/leaveReport', 1, 0, 2)
INSERT [dbo].[Menu] ([ID], [MenuName], [MenuType], [ParentID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Icon], [Link], [ManagerAccess], [UserAccess], [DisplayOrder]) VALUES (46, N'Payslip Report', N'Report', 43, CAST(N'2020-07-09T00:00:00.000' AS DateTime), 1, NULL, NULL, N'poll-box', N'/payslipReport', 1, 0, 3)
SET IDENTITY_INSERT [dbo].[Menu] OFF
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([ID], [Name], [ManagerAccess], [UserAccess], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy]) VALUES (1, N'Admin', 1, 1, CAST(N'2020-07-08T00:00:00.000' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[Roles] ([ID], [Name], [ManagerAccess], [UserAccess], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy]) VALUES (2, N'Manager', 0, 1, CAST(N'2020-07-08T00:00:00.000' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[Roles] ([ID], [Name], [ManagerAccess], [UserAccess], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy]) VALUES (3, N'User', 0, 0, CAST(N'2020-07-08T00:00:00.000' AS DateTime), 1, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Roles] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([UserID], [BranchID], [FirstName], [LastName], [UserName], [Email], [Address], [PasswordHash], [PasswordSalt], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [Designation]) VALUES (1, 1, N'Avishek', N'Kunwar', N'admin', N'avishek@gmail.com', N'Kunwar', 0xF8E9E885CF49121079889B324B43D66BA1D04AD6514EA3F827144A8049292B39452D0B568A35373ADEC70B75DDE5987D5E99FA0DD086940C54D9DC79A4179AE8, 0xE0E5FC08E2F2CE61BC5551B80CD70D62749407E242EF64FB46C0D4782BD9AAE4F9E0FC22B4C47748CF7FC49F10A8CB48A4282B5AA83D46DEA3BB249D4A47A1CCBF0F6C65751C4D5616685C545D7452FBB77AE0C23C214C24A91D62AE8C859B869C540154A8F7AA8B19ACE85CB94F8762A504867213D2178787826BF8661442DD, CAST(N'2020-07-08T17:44:21.983' AS DateTime), 1, NULL, NULL, N'Manager')
SET IDENTITY_INSERT [dbo].[User] OFF
INSERT [dbo].[UserRoles] ([UserID], [RoleID], [BranchID], [CreatedTS], [CreatedBy], [ModifiedTS], [ModifiedBy], [IsActive]) VALUES (1, 1, 1, CAST(N'2020-07-08T17:44:26.313' AS DateTime), 1, NULL, NULL, 0)
ALTER TABLE [dbo].[Employee] ADD  DEFAULT ((0)) FOR [ActiveAccount]
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT ((0)) FOR [CountOT]
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT ((0)) FOR [RestOnHolidays]
GO
