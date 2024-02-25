-- ====================
-- Initial Database Schema
-- ==================== 
CREATE TABLE "Users" (
	"UserID"	INTEGER,
	"UserName"	TEXT,
	"Password"	TEXT,
	"IsAdmin"	INTEGER,
	"IsDefaultAdmin"	INTEGER,
	"IsEnabled"	INTEGER,
	"RolesId"	INTEGER,
	FOREIGN KEY("RolesId") REFERENCES "Roles"("RoleId"),
	PRIMARY KEY("UserID" AUTOINCREMENT)
)

CREATE TABLE "Roles" (
	"RoleId"	INTEGER,
	"RoleName"	TEXT UNIQUE,
	PRIMARY KEY("RoleId" AUTOINCREMENT)
)

CREATE TABLE "UserSettings" (
	"UserSettingID"	INTEGER,
	"UserID"	INTEGER,
	"ThemeName"	TEXT,
	"Fontsize"	INTEGER,
	"FontName"	TEXT,
	"FontColor"	TEXT,
	"BackGroundColor"	TEXT,
	"frmSettingsWindowX"	INTEGER,
	"frmSettingsWindowY"	INTEGER,
	"frmSettingsWindowWidth"	INTEGER,
	"frmSettingsWindowHeight"	INTEGER,
	"frmSettingsBackGroundColor"	TEXT,
	"frmSettingsFontColor"	TEXT,
	"frmSettingsFontSize"	INTEGER,
	FOREIGN KEY("UserID") REFERENCES "Users"("UserID"),
	PRIMARY KEY("UserSettingID" AUTOINCREMENT)
)
-- ====================
-- Alterations - Change Set #1
-- ====================
-- Form Window Settings
ALTER TABLE UserSettings ADD COLUMN Form1WindowX INTEGER; 
ALTER TABLE UserSettings ADD COLUMN Form1WindowY INTEGER;
ALTER TABLE UserSettings ADD COLUMN Form1WindowWidth INTEGER;
ALTER TABLE UserSettings ADD COLUMN Form1WindowHeight INTEGER;
ALTER TABLE UserSettings ADD COLUMN Form1BackGroundColor TEXT; 
ALTER TABLE UserSettings ADD COLUMN Form1FontColor TEXT;
ALTER TABLE UserSettings ADD COLUMN Form1FontSize INTEGER;
ALTER TABLE UserSettings ADD COLUMN Form1WindowHeight INTEGER;

-- Data Grid Settings
ALTER TABLE UserSettings ADD COLUMN DataGrid1ColumnOrder TEXT; 
ALTER TABLE UserSettings ADD COLUMN DataGrid1ColumnWidths TEXT;

