USE [master]
GO
/****** Object:  Database [iSpanFinalProject_API]    Script Date: 2023/2/24 上午 09:06:19 ******/
CREATE DATABASE [iSpanFinalProject_API]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'iSpanFinalProject_API', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\iSpanFinalProject_API.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'iSpanFinalProject_API_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\iSpanFinalProject_API_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [iSpanFinalProject_API] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [iSpanFinalProject_API].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [iSpanFinalProject_API] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET ARITHABORT OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [iSpanFinalProject_API] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [iSpanFinalProject_API] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET  DISABLE_BROKER 
GO
ALTER DATABASE [iSpanFinalProject_API] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [iSpanFinalProject_API] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET RECOVERY FULL 
GO
ALTER DATABASE [iSpanFinalProject_API] SET  MULTI_USER 
GO
ALTER DATABASE [iSpanFinalProject_API] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [iSpanFinalProject_API] SET DB_CHAINING OFF 
GO
ALTER DATABASE [iSpanFinalProject_API] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [iSpanFinalProject_API] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [iSpanFinalProject_API] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [iSpanFinalProject_API] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'iSpanFinalProject_API', N'ON'
GO
ALTER DATABASE [iSpanFinalProject_API] SET QUERY_STORE = OFF
GO
USE [iSpanFinalProject_API]
GO
/****** Object:  Table [dbo].[Activities]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activities](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[activityName] [nvarchar](30) NOT NULL,
	[activityStartTime] [datetime] NOT NULL,
	[activityEndTime] [datetime] NOT NULL,
	[activityLocation] [nvarchar](100) NOT NULL,
	[activityTypeId] [int] NOT NULL,
	[activityInfo] [nvarchar](4000) NOT NULL,
	[activityOrganizerId] [int] NOT NULL,
	[activityImagePath] [varchar](50) NOT NULL,
	[publishedStatus] [bit] NOT NULL,
	[checkedById] [int] NULL,
	[updated] [datetime] NOT NULL,
 CONSTRAINT [PK_Activities] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Activity_Tag_Metadata]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activity_Tag_Metadata](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[activityId] [int] NOT NULL,
	[tagId] [int] NOT NULL,
 CONSTRAINT [PK_Activity_Tag_Metadata] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActivityFollows]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityFollows](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[activityId] [int] NOT NULL,
	[memberId] [int] NOT NULL,
 CONSTRAINT [PK_ActivityFollows] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActivityTags]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityTags](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tagName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ActivityTags] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActivityTypes]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityTypes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[typeName] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_ActivityTypes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Admin_Role_Metadata]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin_Role_Metadata](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[adminId] [int] NOT NULL,
	[roleId] [int] NOT NULL,
 CONSTRAINT [PK_Admin_Role_Metadata] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Admins]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admins](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[adminAccount] [varchar](30) NOT NULL,
	[adminEncryptedPassword] [varchar](70) NOT NULL,
	[departmentId] [int] NOT NULL,
 CONSTRAINT [PK_Maintainers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Albums]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Albums](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[albumName] [nvarchar](50) NOT NULL,
	[albumCoverPath] [nvarchar](200) NOT NULL,
	[albumTypeId] [int] NOT NULL,
	[albumGenreId] [int] NOT NULL,
	[released] [date] NOT NULL,
	[description] [nvarchar](3000) NOT NULL,
	[mainArtistId] [int] NULL,
	[mainCreatorId] [int] NULL,
	[albumProducer] [nvarchar](50) NULL,
	[albumCompany] [nvarchar](50) NULL,
 CONSTRAINT [PK_Albums] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlbumTypes]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlbumTypes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[typeName] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_AlbumTypes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArtistFollows]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArtistFollows](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[artistId] [int] NOT NULL,
	[memberId] [int] NOT NULL,
	[created] [datetime] NOT NULL,
 CONSTRAINT [PK_ArtistFollows] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Artists]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artists](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[artistName] [nvarchar](50) NOT NULL,
	[isBand] [bit] NOT NULL,
	[artistGender] [bit] NULL,
	[artistAbout] [nvarchar](500) NOT NULL,
	[artistPicPath] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Artists_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Avatars]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Avatars](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[path] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Avatars] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartItems]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartItems](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cartId] [int] NOT NULL,
	[productId] [int] NOT NULL,
	[qty] [int] NOT NULL,
 CONSTRAINT [PK_CartItems] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Carts]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[memberId] [int] NOT NULL,
 CONSTRAINT [PK_Carts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coupons]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coupons](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[couponText] [nvarchar](50) NOT NULL,
	[startDate] [datetime] NOT NULL,
	[expiredDate] [datetime] NOT NULL,
	[discounts] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Coupons] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CreatorFollows]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreatorFollows](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[creatorId] [int] NOT NULL,
	[memberId] [int] NOT NULL,
	[created] [datetime] NOT NULL,
 CONSTRAINT [PK_CreatorFollows] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Creators]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Creators](
	[id] [int] NOT NULL,
	[creatorName] [nvarchar](50) NOT NULL,
	[creatorGender] [tinyint] NOT NULL,
	[creatorAbout] [nvarchar](500) NOT NULL,
	[memberId] [int] NOT NULL,
	[creatorPicPath] [nvarchar](100) NOT NULL,
	[creatorCoverPath] [nvarchar](100) NULL,
 CONSTRAINT [PK_Creators] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CreditCards]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreditCards](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[creditCardNumber] [int] NOT NULL,
	[creditCardExpiredDate] [date] NOT NULL,
	[creditCardHolderName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CreditCards] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departments]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[departmentName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LikedActivities]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LikedActivities](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[memberId] [int] NOT NULL,
	[activityId] [int] NOT NULL,
	[created] [datetime] NOT NULL,
 CONSTRAINT [PK_LikedActivities] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LikedAlbums]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LikedAlbums](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[albumId] [int] NOT NULL,
	[memberId] [int] NOT NULL,
	[created] [datetime] NOT NULL,
 CONSTRAINT [PK_LikedAlbums] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LikedPlaylists]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LikedPlaylists](
	[id] [int] NOT NULL,
	[playlistId] [int] NOT NULL,
	[memberId] [int] NOT NULL,
	[created] [datetime] NOT NULL,
 CONSTRAINT [PK_LikedPlaylists] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LikedSongs]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LikedSongs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[songId] [int] NOT NULL,
	[memberId] [int] NOT NULL,
	[created] [datetime] NOT NULL,
 CONSTRAINT [PK_LikedSongs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Members]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Members](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[memberNickName] [nvarchar](50) NOT NULL,
	[memberAccount] [nvarchar](50) NOT NULL,
	[memberEncryptedPassword] [nvarchar](100) NULL,
	[memberEmail] [nvarchar](50) NOT NULL,
	[memberAddress] [nvarchar](100) NULL,
	[memberCellphone] [varchar](10) NULL,
	[memberDateOfBirth] [date] NULL,
	[avatarId] [int] NULL,
	[memberReceivedMessage] [bit] NOT NULL,
	[memberSharedData] [bit] NOT NULL,
	[libraryPrivacy] [bit] NOT NULL,
	[calenderPrivacy] [bit] NOT NULL,
	[creditCardId] [int] NULL,
	[isConfirmed] [bit] NOT NULL,
	[confirmCode] [varchar](50) NULL,
 CONSTRAINT [PK_Members] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order_Product_Metadata]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order_Product_Metadata](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[orderId] [int] NOT NULL,
	[productId] [int] NOT NULL,
	[price] [decimal](10, 0) NOT NULL,
	[productName] [nvarchar](50) NOT NULL,
	[qty] [int] NOT NULL,
 CONSTRAINT [PK_Order_Product_Metadata] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[memberId] [int] NOT NULL,
	[couponId] [int] NOT NULL,
	[payments] [int] NOT NULL,
	[orderStatus] [bit] NOT NULL,
	[paid] [bit] NOT NULL,
	[created] [datetime] NOT NULL,
	[receiver] [nvarchar](30) NOT NULL,
	[address] [nvarchar](200) NOT NULL,
	[cellphone] [varchar](10) NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Playlist_Song_Metadata]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Playlist_Song_Metadata](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[playListId] [int] NOT NULL,
	[songId] [int] NOT NULL,
	[displayOrder] [int] NOT NULL,
	[addedTime] [date] NOT NULL,
 CONSTRAINT [PK_PlayList_Music_Metadata] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Playlists]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Playlists](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[listName] [nvarchar](50) NOT NULL,
	[playlistCoverPath] [nchar](200) NULL,
	[memberId] [int] NOT NULL,
	[description] [nvarchar](300) NULL,
	[isPublic] [bit] NOT NULL,
	[created] [datetime] NOT NULL,
 CONSTRAINT [PK_Playlists] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategories]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[categoryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ProductCategories] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[productCategoryId] [int] NOT NULL,
	[productPrice] [decimal](7, 0) NOT NULL,
	[albumId] [int] NOT NULL,
	[productName] [nvarchar](50) NOT NULL,
	[stock] [int] NOT NULL,
	[status] [bit] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Queues]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Queues](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[memberId] [int] NOT NULL,
	[currentSongId] [int] NULL,
	[currentSongTime] [int] NULL,
	[isShuffle] [bit] NOT NULL,
	[isRepeat] [bit] NULL,
	[albumId] [int] NULL,
	[playlistId] [int] NULL,
	[artistId] [int] NULL,
 CONSTRAINT [PK_Queues] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QueueSongs]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QueueSongs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[queueId] [int] NOT NULL,
	[songId] [int] NOT NULL,
	[displayOrder] [int] NOT NULL,
	[fromPlaylist] [bit] NOT NULL,
	[shuffleOrder] [int] NOT NULL,
 CONSTRAINT [PK_QueueSongs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[id] [int] NOT NULL,
	[roleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Song_Artist_Metadata]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Song_Artist_Metadata](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[songId] [int] NOT NULL,
	[artistId] [int] NOT NULL,
 CONSTRAINT [PK_Song_Artist_Metadata] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Song_Creator_Metadata]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Song_Creator_Metadata](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[songId] [int] NOT NULL,
	[creatorId] [int] NOT NULL,
 CONSTRAINT [PK_Music_Artist_Metadata] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SongGenres]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SongGenres](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[genreName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Genres] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SongPlayedRecords]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SongPlayedRecords](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[songId] [int] NOT NULL,
	[memberId] [int] NOT NULL,
	[playedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_SongPlayedRecords] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Songs]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Songs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[songName] [nvarchar](50) NOT NULL,
	[genreId] [int] NOT NULL,
	[duration] [int] NOT NULL,
	[isInstrumental] [bit] NOT NULL,
	[language] [nvarchar](50) NULL,
	[isExplicit] [bit] NULL,
	[released] [datetime] NOT NULL,
	[songWriter] [nvarchar](50) NOT NULL,
	[lyric] [nvarchar](2000) NULL,
	[songCoverPath] [nvarchar](200) NOT NULL,
	[songPath] [nvarchar](200) NOT NULL,
	[status] [bit] NOT NULL,
	[albumId] [int] NULL,
	[displayOrderInAlbum] [int] NULL,
 CONSTRAINT [PK_Musics] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubscriptionPlan]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubscriptionPlan](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[planName] [nvarchar](50) NOT NULL,
	[price] [decimal](7, 0) NOT NULL,
	[numberOfUsers] [tinyint] NOT NULL,
	[description] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_SubscriptionPlan] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubscriptionRecords]    Script Date: 2023/2/24 上午 09:06:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubscriptionRecords](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[memberId] [int] NOT NULL,
	[subscriptionPlanId] [int] NOT NULL,
	[subscribedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SubscriptionRecords] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Artists] ON 

INSERT [dbo].[Artists] ([id], [artistName], [isBand], [artistGender], [artistAbout], [artistPicPath]) VALUES (2, N'告五人', 1, NULL, N'2011年夏天成團於宜蘭，當時的成員包含現任主唱潘雲安、吉他手潘燕山、吉他手連震堂、貝斯手吳尚任、鼓手林正[1]，目前團員為男主唱兼吉他手潘雲安、女主唱犬青及鼓手林哲謙。2017年10月發行首張EP《迷霧之子》[2]，舉行首次巡迴演出，締造北高三場皆SOLD OUT紀錄。該作更於翌年獲得2018金音獎最佳新人（團）獎。2019年展開『島嶼雛形』巡迴演唱會[3]。團名取作「告五人」背後沒有什麼特殊涵義，最初在決定團名時，由團員隨機在法院佈告欄上各指出一個字因而命名[4]。', N'538f27fbcb6047beb7b6a16ec8b7eb98.jpg')
SET IDENTITY_INSERT [dbo].[Artists] OFF
GO
SET IDENTITY_INSERT [dbo].[Song_Artist_Metadata] ON 

INSERT [dbo].[Song_Artist_Metadata] ([id], [songId], [artistId]) VALUES (2, 2, 2)
SET IDENTITY_INSERT [dbo].[Song_Artist_Metadata] OFF
GO
SET IDENTITY_INSERT [dbo].[SongGenres] ON 

INSERT [dbo].[SongGenres] ([id], [genreName]) VALUES (1, N'華語流行')
SET IDENTITY_INSERT [dbo].[SongGenres] OFF
GO
SET IDENTITY_INSERT [dbo].[Songs] ON 

INSERT [dbo].[Songs] ([id], [songName], [genreId], [duration], [isInstrumental], [language], [isExplicit], [released], [songWriter], [lyric], [songCoverPath], [songPath], [status], [albumId], [displayOrderInAlbum]) VALUES (2, N'唯一', 1, 271, 0, N'中文', 0, CAST(N'2023-02-21T00:00:00.000' AS DateTime), N'潘雲安', N'sss', N'8845ded4b1c644f3953b9e9c67cfadef.jpg', N'31c10df65404473dbacc7caf3d966552.mp3', 1, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Songs] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Admins]    Script Date: 2023/2/24 上午 09:06:19 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Admins] ON [dbo].[Admins]
(
	[adminAccount] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_Creators]    Script Date: 2023/2/24 上午 09:06:19 ******/
ALTER TABLE [dbo].[Creators] ADD  CONSTRAINT [UK_Creators] UNIQUE NONCLUSTERED 
(
	[creatorName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Members] ADD  CONSTRAINT [DF_Members_avatarId]  DEFAULT ((1)) FOR [avatarId]
GO
ALTER TABLE [dbo].[Members] ADD  CONSTRAINT [DF_Members_memberReceivedMessage]  DEFAULT ((0)) FOR [memberReceivedMessage]
GO
ALTER TABLE [dbo].[Members] ADD  CONSTRAINT [DF_Members_memberSharedData]  DEFAULT ((0)) FOR [memberSharedData]
GO
ALTER TABLE [dbo].[Members] ADD  CONSTRAINT [DF_Members_libraryPrivacy]  DEFAULT ((1)) FOR [libraryPrivacy]
GO
ALTER TABLE [dbo].[Members] ADD  CONSTRAINT [DF_Members_calenderPrivacy]  DEFAULT ((1)) FOR [calenderPrivacy]
GO
ALTER TABLE [dbo].[Members] ADD  CONSTRAINT [DF_Members_isConfirmed]  DEFAULT ((0)) FOR [isConfirmed]
GO
ALTER TABLE [dbo].[Playlist_Song_Metadata] ADD  CONSTRAINT [DF_PlayList_Song_Metadata_addedTime]  DEFAULT (getdate()) FOR [addedTime]
GO
ALTER TABLE [dbo].[Activities]  WITH CHECK ADD  CONSTRAINT [FK_Activities_ActivityTypes] FOREIGN KEY([activityTypeId])
REFERENCES [dbo].[ActivityTypes] ([id])
GO
ALTER TABLE [dbo].[Activities] CHECK CONSTRAINT [FK_Activities_ActivityTypes]
GO
ALTER TABLE [dbo].[Activities]  WITH CHECK ADD  CONSTRAINT [FK_Activities_Admins] FOREIGN KEY([checkedById])
REFERENCES [dbo].[Admins] ([id])
GO
ALTER TABLE [dbo].[Activities] CHECK CONSTRAINT [FK_Activities_Admins]
GO
ALTER TABLE [dbo].[Activities]  WITH CHECK ADD  CONSTRAINT [FK_Activities_Members] FOREIGN KEY([activityOrganizerId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[Activities] CHECK CONSTRAINT [FK_Activities_Members]
GO
ALTER TABLE [dbo].[Activity_Tag_Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Activity_Tag_Metadata_Activities] FOREIGN KEY([activityId])
REFERENCES [dbo].[Activities] ([id])
GO
ALTER TABLE [dbo].[Activity_Tag_Metadata] CHECK CONSTRAINT [FK_Activity_Tag_Metadata_Activities]
GO
ALTER TABLE [dbo].[Activity_Tag_Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Activity_Tag_Metadata_ActivityTags] FOREIGN KEY([tagId])
REFERENCES [dbo].[ActivityTags] ([id])
GO
ALTER TABLE [dbo].[Activity_Tag_Metadata] CHECK CONSTRAINT [FK_Activity_Tag_Metadata_ActivityTags]
GO
ALTER TABLE [dbo].[ActivityFollows]  WITH CHECK ADD  CONSTRAINT [FK_ActivityFollows_Activities] FOREIGN KEY([activityId])
REFERENCES [dbo].[Activities] ([id])
GO
ALTER TABLE [dbo].[ActivityFollows] CHECK CONSTRAINT [FK_ActivityFollows_Activities]
GO
ALTER TABLE [dbo].[ActivityFollows]  WITH CHECK ADD  CONSTRAINT [FK_ActivityFollows_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[ActivityFollows] CHECK CONSTRAINT [FK_ActivityFollows_Members]
GO
ALTER TABLE [dbo].[Admin_Role_Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Admin_Role_Metadata_Admins] FOREIGN KEY([adminId])
REFERENCES [dbo].[Admins] ([id])
GO
ALTER TABLE [dbo].[Admin_Role_Metadata] CHECK CONSTRAINT [FK_Admin_Role_Metadata_Admins]
GO
ALTER TABLE [dbo].[Admin_Role_Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Admin_Role_Metadata_Roles] FOREIGN KEY([roleId])
REFERENCES [dbo].[Roles] ([id])
GO
ALTER TABLE [dbo].[Admin_Role_Metadata] CHECK CONSTRAINT [FK_Admin_Role_Metadata_Roles]
GO
ALTER TABLE [dbo].[Admins]  WITH CHECK ADD  CONSTRAINT [FK_Admins_Departments] FOREIGN KEY([departmentId])
REFERENCES [dbo].[Departments] ([id])
GO
ALTER TABLE [dbo].[Admins] CHECK CONSTRAINT [FK_Admins_Departments]
GO
ALTER TABLE [dbo].[Albums]  WITH CHECK ADD  CONSTRAINT [FK_Albums_AlbumTypes] FOREIGN KEY([albumTypeId])
REFERENCES [dbo].[AlbumTypes] ([id])
GO
ALTER TABLE [dbo].[Albums] CHECK CONSTRAINT [FK_Albums_AlbumTypes]
GO
ALTER TABLE [dbo].[Albums]  WITH CHECK ADD  CONSTRAINT [FK_Albums_Artists] FOREIGN KEY([mainArtistId])
REFERENCES [dbo].[Artists] ([id])
GO
ALTER TABLE [dbo].[Albums] CHECK CONSTRAINT [FK_Albums_Artists]
GO
ALTER TABLE [dbo].[Albums]  WITH CHECK ADD  CONSTRAINT [FK_Albums_Creators] FOREIGN KEY([mainCreatorId])
REFERENCES [dbo].[Creators] ([id])
GO
ALTER TABLE [dbo].[Albums] CHECK CONSTRAINT [FK_Albums_Creators]
GO
ALTER TABLE [dbo].[Albums]  WITH CHECK ADD  CONSTRAINT [FK_Albums_SongGenres] FOREIGN KEY([albumGenreId])
REFERENCES [dbo].[SongGenres] ([id])
GO
ALTER TABLE [dbo].[Albums] CHECK CONSTRAINT [FK_Albums_SongGenres]
GO
ALTER TABLE [dbo].[ArtistFollows]  WITH CHECK ADD  CONSTRAINT [FK_ArtistFollows_Artists] FOREIGN KEY([artistId])
REFERENCES [dbo].[Artists] ([id])
GO
ALTER TABLE [dbo].[ArtistFollows] CHECK CONSTRAINT [FK_ArtistFollows_Artists]
GO
ALTER TABLE [dbo].[ArtistFollows]  WITH CHECK ADD  CONSTRAINT [FK_ArtistFollows_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[ArtistFollows] CHECK CONSTRAINT [FK_ArtistFollows_Members]
GO
ALTER TABLE [dbo].[CartItems]  WITH CHECK ADD  CONSTRAINT [FK_CartItems_CartItems] FOREIGN KEY([cartId])
REFERENCES [dbo].[Carts] ([id])
GO
ALTER TABLE [dbo].[CartItems] CHECK CONSTRAINT [FK_CartItems_CartItems]
GO
ALTER TABLE [dbo].[CartItems]  WITH CHECK ADD  CONSTRAINT [FK_CartItems_Products] FOREIGN KEY([productId])
REFERENCES [dbo].[Products] ([id])
GO
ALTER TABLE [dbo].[CartItems] CHECK CONSTRAINT [FK_CartItems_Products]
GO
ALTER TABLE [dbo].[Carts]  WITH CHECK ADD  CONSTRAINT [FK_Carts_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[Carts] CHECK CONSTRAINT [FK_Carts_Members]
GO
ALTER TABLE [dbo].[CreatorFollows]  WITH CHECK ADD  CONSTRAINT [FK_CreatorFollows_Creators] FOREIGN KEY([creatorId])
REFERENCES [dbo].[Creators] ([id])
GO
ALTER TABLE [dbo].[CreatorFollows] CHECK CONSTRAINT [FK_CreatorFollows_Creators]
GO
ALTER TABLE [dbo].[CreatorFollows]  WITH CHECK ADD  CONSTRAINT [FK_CreatorFollows_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[CreatorFollows] CHECK CONSTRAINT [FK_CreatorFollows_Members]
GO
ALTER TABLE [dbo].[Creators]  WITH CHECK ADD  CONSTRAINT [FK_Creators_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[Creators] CHECK CONSTRAINT [FK_Creators_Members]
GO
ALTER TABLE [dbo].[LikedActivities]  WITH CHECK ADD  CONSTRAINT [FK_LikedActivities_Activities] FOREIGN KEY([activityId])
REFERENCES [dbo].[Activities] ([id])
GO
ALTER TABLE [dbo].[LikedActivities] CHECK CONSTRAINT [FK_LikedActivities_Activities]
GO
ALTER TABLE [dbo].[LikedActivities]  WITH CHECK ADD  CONSTRAINT [FK_LikedActivities_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[LikedActivities] CHECK CONSTRAINT [FK_LikedActivities_Members]
GO
ALTER TABLE [dbo].[LikedAlbums]  WITH CHECK ADD  CONSTRAINT [FK_LikedAlbums_LikedAlbums] FOREIGN KEY([albumId])
REFERENCES [dbo].[Albums] ([id])
GO
ALTER TABLE [dbo].[LikedAlbums] CHECK CONSTRAINT [FK_LikedAlbums_LikedAlbums]
GO
ALTER TABLE [dbo].[LikedAlbums]  WITH CHECK ADD  CONSTRAINT [FK_LikedAlbums_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[LikedAlbums] CHECK CONSTRAINT [FK_LikedAlbums_Members]
GO
ALTER TABLE [dbo].[LikedPlaylists]  WITH CHECK ADD  CONSTRAINT [FK_LikedPlaylists_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[LikedPlaylists] CHECK CONSTRAINT [FK_LikedPlaylists_Members]
GO
ALTER TABLE [dbo].[LikedPlaylists]  WITH CHECK ADD  CONSTRAINT [FK_LikedPlaylists_Playlists] FOREIGN KEY([playlistId])
REFERENCES [dbo].[Playlists] ([id])
GO
ALTER TABLE [dbo].[LikedPlaylists] CHECK CONSTRAINT [FK_LikedPlaylists_Playlists]
GO
ALTER TABLE [dbo].[LikedSongs]  WITH CHECK ADD  CONSTRAINT [FK_LikedSongs_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[LikedSongs] CHECK CONSTRAINT [FK_LikedSongs_Members]
GO
ALTER TABLE [dbo].[LikedSongs]  WITH CHECK ADD  CONSTRAINT [FK_LikedSongs_Songs] FOREIGN KEY([songId])
REFERENCES [dbo].[Songs] ([id])
GO
ALTER TABLE [dbo].[LikedSongs] CHECK CONSTRAINT [FK_LikedSongs_Songs]
GO
ALTER TABLE [dbo].[Members]  WITH CHECK ADD  CONSTRAINT [FK_Members_Avatars] FOREIGN KEY([avatarId])
REFERENCES [dbo].[Avatars] ([id])
GO
ALTER TABLE [dbo].[Members] CHECK CONSTRAINT [FK_Members_Avatars]
GO
ALTER TABLE [dbo].[Members]  WITH CHECK ADD  CONSTRAINT [FK_Members_CreditCards] FOREIGN KEY([creditCardId])
REFERENCES [dbo].[CreditCards] ([id])
GO
ALTER TABLE [dbo].[Members] CHECK CONSTRAINT [FK_Members_CreditCards]
GO
ALTER TABLE [dbo].[Order_Product_Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Order_Product_Metadata_Orders] FOREIGN KEY([orderId])
REFERENCES [dbo].[Orders] ([id])
GO
ALTER TABLE [dbo].[Order_Product_Metadata] CHECK CONSTRAINT [FK_Order_Product_Metadata_Orders]
GO
ALTER TABLE [dbo].[Order_Product_Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Order_Product_Metadata_Products] FOREIGN KEY([productId])
REFERENCES [dbo].[Products] ([id])
GO
ALTER TABLE [dbo].[Order_Product_Metadata] CHECK CONSTRAINT [FK_Order_Product_Metadata_Products]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Coupons] FOREIGN KEY([couponId])
REFERENCES [dbo].[Coupons] ([id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Coupons]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Members]
GO
ALTER TABLE [dbo].[Playlist_Song_Metadata]  WITH CHECK ADD  CONSTRAINT [FK_PlayList_Music_Metadata_Musics] FOREIGN KEY([songId])
REFERENCES [dbo].[Songs] ([id])
GO
ALTER TABLE [dbo].[Playlist_Song_Metadata] CHECK CONSTRAINT [FK_PlayList_Music_Metadata_Musics]
GO
ALTER TABLE [dbo].[Playlist_Song_Metadata]  WITH CHECK ADD  CONSTRAINT [FK_PlayList_Music_Metadata_Playlists] FOREIGN KEY([playListId])
REFERENCES [dbo].[Playlists] ([id])
GO
ALTER TABLE [dbo].[Playlist_Song_Metadata] CHECK CONSTRAINT [FK_PlayList_Music_Metadata_Playlists]
GO
ALTER TABLE [dbo].[Playlists]  WITH CHECK ADD  CONSTRAINT [FK_Playlists_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[Playlists] CHECK CONSTRAINT [FK_Playlists_Members]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Albums] FOREIGN KEY([albumId])
REFERENCES [dbo].[Albums] ([id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Albums]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_ProductCategories] FOREIGN KEY([productCategoryId])
REFERENCES [dbo].[ProductCategories] ([id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_ProductCategories]
GO
ALTER TABLE [dbo].[Queues]  WITH CHECK ADD  CONSTRAINT [FK_Queues_Albums] FOREIGN KEY([albumId])
REFERENCES [dbo].[Albums] ([id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Queues] CHECK CONSTRAINT [FK_Queues_Albums]
GO
ALTER TABLE [dbo].[Queues]  WITH CHECK ADD  CONSTRAINT [FK_Queues_Artists] FOREIGN KEY([artistId])
REFERENCES [dbo].[Artists] ([id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Queues] CHECK CONSTRAINT [FK_Queues_Artists]
GO
ALTER TABLE [dbo].[Queues]  WITH CHECK ADD  CONSTRAINT [FK_Queues_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[Queues] CHECK CONSTRAINT [FK_Queues_Members]
GO
ALTER TABLE [dbo].[Queues]  WITH CHECK ADD  CONSTRAINT [FK_Queues_Playlists] FOREIGN KEY([playlistId])
REFERENCES [dbo].[Playlists] ([id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Queues] CHECK CONSTRAINT [FK_Queues_Playlists]
GO
ALTER TABLE [dbo].[Queues]  WITH CHECK ADD  CONSTRAINT [FK_Queues_Songs] FOREIGN KEY([currentSongId])
REFERENCES [dbo].[Songs] ([id])
GO
ALTER TABLE [dbo].[Queues] CHECK CONSTRAINT [FK_Queues_Songs]
GO
ALTER TABLE [dbo].[QueueSongs]  WITH CHECK ADD  CONSTRAINT [FK_QueueSongs_Queues] FOREIGN KEY([queueId])
REFERENCES [dbo].[Queues] ([id])
GO
ALTER TABLE [dbo].[QueueSongs] CHECK CONSTRAINT [FK_QueueSongs_Queues]
GO
ALTER TABLE [dbo].[QueueSongs]  WITH CHECK ADD  CONSTRAINT [FK_QueueSongs_Songs] FOREIGN KEY([songId])
REFERENCES [dbo].[Songs] ([id])
GO
ALTER TABLE [dbo].[QueueSongs] CHECK CONSTRAINT [FK_QueueSongs_Songs]
GO
ALTER TABLE [dbo].[Song_Artist_Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Song_Artist_Metadata_Artists] FOREIGN KEY([artistId])
REFERENCES [dbo].[Artists] ([id])
GO
ALTER TABLE [dbo].[Song_Artist_Metadata] CHECK CONSTRAINT [FK_Song_Artist_Metadata_Artists]
GO
ALTER TABLE [dbo].[Song_Artist_Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Song_Artist_Metadata_Songs] FOREIGN KEY([songId])
REFERENCES [dbo].[Songs] ([id])
GO
ALTER TABLE [dbo].[Song_Artist_Metadata] CHECK CONSTRAINT [FK_Song_Artist_Metadata_Songs]
GO
ALTER TABLE [dbo].[Song_Creator_Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Song_Creator_Metadata_Creators] FOREIGN KEY([creatorId])
REFERENCES [dbo].[Creators] ([id])
GO
ALTER TABLE [dbo].[Song_Creator_Metadata] CHECK CONSTRAINT [FK_Song_Creator_Metadata_Creators]
GO
ALTER TABLE [dbo].[Song_Creator_Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Song_Creator_Metadata_Songs] FOREIGN KEY([songId])
REFERENCES [dbo].[Songs] ([id])
GO
ALTER TABLE [dbo].[Song_Creator_Metadata] CHECK CONSTRAINT [FK_Song_Creator_Metadata_Songs]
GO
ALTER TABLE [dbo].[SongPlayedRecords]  WITH CHECK ADD  CONSTRAINT [FK_SongPlayedRecords_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[SongPlayedRecords] CHECK CONSTRAINT [FK_SongPlayedRecords_Members]
GO
ALTER TABLE [dbo].[SongPlayedRecords]  WITH CHECK ADD  CONSTRAINT [FK_SongPlayedRecords_Songs] FOREIGN KEY([songId])
REFERENCES [dbo].[Songs] ([id])
GO
ALTER TABLE [dbo].[SongPlayedRecords] CHECK CONSTRAINT [FK_SongPlayedRecords_Songs]
GO
ALTER TABLE [dbo].[Songs]  WITH CHECK ADD  CONSTRAINT [FK_Musics_Genres] FOREIGN KEY([genreId])
REFERENCES [dbo].[SongGenres] ([id])
GO
ALTER TABLE [dbo].[Songs] CHECK CONSTRAINT [FK_Musics_Genres]
GO
ALTER TABLE [dbo].[Songs]  WITH CHECK ADD  CONSTRAINT [FK_Songs_Albums] FOREIGN KEY([albumId])
REFERENCES [dbo].[Albums] ([id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Songs] CHECK CONSTRAINT [FK_Songs_Albums]
GO
ALTER TABLE [dbo].[SubscriptionRecords]  WITH CHECK ADD  CONSTRAINT [FK_SubscriptionRecords_Members] FOREIGN KEY([memberId])
REFERENCES [dbo].[Members] ([id])
GO
ALTER TABLE [dbo].[SubscriptionRecords] CHECK CONSTRAINT [FK_SubscriptionRecords_Members]
GO
ALTER TABLE [dbo].[SubscriptionRecords]  WITH CHECK ADD  CONSTRAINT [FK_SubscriptionRecords_SubscriptionPlan] FOREIGN KEY([subscriptionPlanId])
REFERENCES [dbo].[SubscriptionPlan] ([id])
GO
ALTER TABLE [dbo].[SubscriptionRecords] CHECK CONSTRAINT [FK_SubscriptionRecords_SubscriptionPlan]
GO
USE [master]
GO
ALTER DATABASE [iSpanFinalProject_API] SET  READ_WRITE 
GO
