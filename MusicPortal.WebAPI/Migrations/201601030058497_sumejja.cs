namespace MusicPortal.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sumejja : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthorSongs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SongId = c.Long(nullable: false),
                        AuthorId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: true)
                .Index(t => t.SongId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Link = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HeartedSongs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        SongId = c.Long(nullable: false),
                        IsHearted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.SongId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.TagUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        TagId = c.Long(),
                        Popularity = c.Int(nullable: false),
                        ParentTagId = c.Long(nullable: false),
                        Tag_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tags", t => t.Tag_Id)
                .ForeignKey("dbo.Tags", t => t.ParentTagId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TagId)
                .Index(t => t.ParentTagId)
                .Index(t => t.Tag_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        ParentId = c.Long(),
                        Popularity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tags", t => t.ParentId)
                .Index(t => t.ParentId, name: "Parent");
            
            CreateTable(
                "dbo.TagSongs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SongId = c.Long(nullable: false),
                        TagId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.SongId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.Playlists",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlaylistSongs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PlaylistId = c.Long(nullable: false),
                        SongId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Playlists", t => t.PlaylistId, cascadeDelete: true)
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: true)
                .Index(t => t.PlaylistId)
                .Index(t => t.SongId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PlaylistSongs", "SongId", "dbo.Songs");
            DropForeignKey("dbo.PlaylistSongs", "PlaylistId", "dbo.Playlists");
            DropForeignKey("dbo.TagUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TagUsers", "TagId", "dbo.Tags");
            DropForeignKey("dbo.TagUsers", "ParentTagId", "dbo.Tags");
            DropForeignKey("dbo.TagUsers", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.TagSongs", "TagId", "dbo.Tags");
            DropForeignKey("dbo.TagSongs", "SongId", "dbo.Songs");
            DropForeignKey("dbo.Tags", "ParentId", "dbo.Tags");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.HeartedSongs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.HeartedSongs", "SongId", "dbo.Songs");
            DropForeignKey("dbo.AuthorSongs", "SongId", "dbo.Songs");
            DropForeignKey("dbo.AuthorSongs", "AuthorId", "dbo.Authors");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PlaylistSongs", new[] { "SongId" });
            DropIndex("dbo.PlaylistSongs", new[] { "PlaylistId" });
            DropIndex("dbo.TagSongs", new[] { "TagId" });
            DropIndex("dbo.TagSongs", new[] { "SongId" });
            DropIndex("dbo.Tags", "Parent");
            DropIndex("dbo.TagUsers", new[] { "Tag_Id" });
            DropIndex("dbo.TagUsers", new[] { "ParentTagId" });
            DropIndex("dbo.TagUsers", new[] { "TagId" });
            DropIndex("dbo.TagUsers", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.HeartedSongs", new[] { "SongId" });
            DropIndex("dbo.HeartedSongs", new[] { "UserId" });
            DropIndex("dbo.AuthorSongs", new[] { "AuthorId" });
            DropIndex("dbo.AuthorSongs", new[] { "SongId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PlaylistSongs");
            DropTable("dbo.Playlists");
            DropTable("dbo.TagSongs");
            DropTable("dbo.Tags");
            DropTable("dbo.TagUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.HeartedSongs");
            DropTable("dbo.Songs");
            DropTable("dbo.AuthorSongs");
            DropTable("dbo.Authors");
        }
    }
}
