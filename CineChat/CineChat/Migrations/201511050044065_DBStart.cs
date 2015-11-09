namespace CineChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBStart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        description = c.String(nullable: false, maxLength: 60),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        password = c.String(),
                        admin_Id = c.String(maxLength: 128),
                        category_ID = c.Int(),
                        movie_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.admin_Id)
                .ForeignKey("dbo.Categories", t => t.category_ID)
                .ForeignKey("dbo.Movies", t => t.movie_ID)
                .Index(t => t.admin_Id)
                .Index(t => t.category_ID)
                .Index(t => t.movie_ID);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImdbID = c.Int(nullable: false),
                        title = c.String(nullable: false, maxLength: 100),
                        releasedate = c.DateTime(nullable: false),
                        duration = c.DateTime(nullable: false),
                        ratingImdb = c.Double(nullable: false),
                        description = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 100),
                        actor_ID = c.Int(nullable: false),
                        movie_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.People", t => t.actor_ID, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.movie_ID)
                .Index(t => t.actor_ID)
                .Index(t => t.movie_ID);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Directors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        person_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.People", t => t.person_ID)
                .Index(t => t.person_ID);
            
            CreateTable(
                "dbo.Rates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        rate = c.Double(nullable: false),
                        movie_ID = c.Int(),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Movies", t => t.movie_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.movie_ID)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.Writers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        person_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.People", t => t.person_ID)
                .Index(t => t.person_ID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        message = c.String(),
                        time = c.DateTime(nullable: false),
                        chat_ID = c.Int(),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Chats", t => t.chat_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.chat_ID)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.MovieCategories",
                c => new
                    {
                        Movie_ID = c.Int(nullable: false),
                        Category_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Movie_ID, t.Category_ID })
                .ForeignKey("dbo.Movies", t => t.Movie_ID, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_ID, cascadeDelete: true)
                .Index(t => t.Movie_ID)
                .Index(t => t.Category_ID);
            
            CreateTable(
                "dbo.DirectorMovies",
                c => new
                    {
                        Director_ID = c.Int(nullable: false),
                        Movie_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Director_ID, t.Movie_ID })
                .ForeignKey("dbo.Directors", t => t.Director_ID, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.Movie_ID, cascadeDelete: true)
                .Index(t => t.Director_ID)
                .Index(t => t.Movie_ID);
            
            CreateTable(
                "dbo.MovieApplicationUsers",
                c => new
                    {
                        Movie_ID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Movie_ID, t.ApplicationUser_Id })
                .ForeignKey("dbo.Movies", t => t.Movie_ID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Movie_ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.WriterMovies",
                c => new
                    {
                        Writer_ID = c.Int(nullable: false),
                        Movie_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Writer_ID, t.Movie_ID })
                .ForeignKey("dbo.Writers", t => t.Writer_ID, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.Movie_ID, cascadeDelete: true)
                .Index(t => t.Writer_ID)
                .Index(t => t.Movie_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chats", "movie_ID", "dbo.Movies");
            DropForeignKey("dbo.Chats", "category_ID", "dbo.Categories");
            DropForeignKey("dbo.Posts", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "chat_ID", "dbo.Chats");
            DropForeignKey("dbo.Writers", "person_ID", "dbo.People");
            DropForeignKey("dbo.WriterMovies", "Movie_ID", "dbo.Movies");
            DropForeignKey("dbo.WriterMovies", "Writer_ID", "dbo.Writers");
            DropForeignKey("dbo.Rates", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rates", "movie_ID", "dbo.Movies");
            DropForeignKey("dbo.MovieApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.MovieApplicationUsers", "Movie_ID", "dbo.Movies");
            DropForeignKey("dbo.Directors", "person_ID", "dbo.People");
            DropForeignKey("dbo.DirectorMovies", "Movie_ID", "dbo.Movies");
            DropForeignKey("dbo.DirectorMovies", "Director_ID", "dbo.Directors");
            DropForeignKey("dbo.Characters", "movie_ID", "dbo.Movies");
            DropForeignKey("dbo.Characters", "actor_ID", "dbo.People");
            DropForeignKey("dbo.MovieCategories", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.MovieCategories", "Movie_ID", "dbo.Movies");
            DropForeignKey("dbo.Chats", "admin_Id", "dbo.AspNetUsers");
            DropIndex("dbo.WriterMovies", new[] { "Movie_ID" });
            DropIndex("dbo.WriterMovies", new[] { "Writer_ID" });
            DropIndex("dbo.MovieApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.MovieApplicationUsers", new[] { "Movie_ID" });
            DropIndex("dbo.DirectorMovies", new[] { "Movie_ID" });
            DropIndex("dbo.DirectorMovies", new[] { "Director_ID" });
            DropIndex("dbo.MovieCategories", new[] { "Category_ID" });
            DropIndex("dbo.MovieCategories", new[] { "Movie_ID" });
            DropIndex("dbo.Posts", new[] { "user_Id" });
            DropIndex("dbo.Posts", new[] { "chat_ID" });
            DropIndex("dbo.Writers", new[] { "person_ID" });
            DropIndex("dbo.Rates", new[] { "user_Id" });
            DropIndex("dbo.Rates", new[] { "movie_ID" });
            DropIndex("dbo.Directors", new[] { "person_ID" });
            DropIndex("dbo.Characters", new[] { "movie_ID" });
            DropIndex("dbo.Characters", new[] { "actor_ID" });
            DropIndex("dbo.Chats", new[] { "movie_ID" });
            DropIndex("dbo.Chats", new[] { "category_ID" });
            DropIndex("dbo.Chats", new[] { "admin_Id" });
            DropTable("dbo.WriterMovies");
            DropTable("dbo.MovieApplicationUsers");
            DropTable("dbo.DirectorMovies");
            DropTable("dbo.MovieCategories");
            DropTable("dbo.Posts");
            DropTable("dbo.Writers");
            DropTable("dbo.Rates");
            DropTable("dbo.Directors");
            DropTable("dbo.People");
            DropTable("dbo.Characters");
            DropTable("dbo.Movies");
            DropTable("dbo.Chats");
            DropTable("dbo.Categories");
        }
    }
}
